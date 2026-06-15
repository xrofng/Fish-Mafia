using Combat2D;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateAttacking : BasePlayerState
{
    private bool _attacking;
    private bool _attackingInitiatedFromEnterState;

    public struct EvsAttackInitiated
    {

    }

    public PlayerStateAttacking(PlayerStateMachine.EPlayerState key, PlayerStateMachine playerStateMachine, PlayerChar pChar) : base(key, playerStateMachine, pChar)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _attackingInitiatedFromEnterState = true;
        InitateAttackMove(PlayerChar.PlayerCombat.InputOnEnter, PlayerChar.PlayerCombat.InputOnEnter == "RightAttack");
        PlayerChar.Rigdibody.linearVelocity = Vector3.zero;
        _attacking = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        PlayerChar.CoroutineRunner.StopCoroutine(PlayerChar.PlayerCombat.CurrAttackMulator);
        // disable hit boxes incase of force exit state
        PlayerChar.PlayerCombat.ClearBuffer();
        PlayerChar.PlayerCombat.SetHitBoxesEnabled(false);
        _attacking = false;
        _attackingInitiatedFromEnterState = false;
    }

    private void InitateAttackMove(string inputName, bool forceVoidTarget = true)
    {
        if (_attacking)
        {
            return;
        }
        _attacking = true;
        //Debug.Log($"InitateAttackMove | {Time.time} | ");

        PlayerTargeting.FindTarget();
        Transform target = PlayerTargeting.GetTarget();
        if (forceVoidTarget == true)
        {
            target = PlayerTargeting.VoidTarget;
        }
        PlayerChar.Rigdibody.linearVelocity = Vector3.zero;
        PlayerChar.CoroutineRunner.Run(AttackRoutine(target, inputName));
        EventBus.TriggerEvent(new EvsAttackInitiated());
    }

    protected override void OnLeftClick(InputAction.CallbackContext ctx)
    {
        base.OnLeftClick(ctx);
        //Debug.Log($"Left press while Attacking | {Time.time} | ");
        if (_attackingInitiatedFromEnterState == false)
        {
            InitateAttackMove(ctx.action.name, false);
        }
    }

    protected override void OnRightClick(InputAction.CallbackContext ctx)
    {
        base.OnRightClick(ctx);
        if (PlayerChar.PlayerCombat.BulletMana.Consume())
        {
            InitateAttackMove(ctx.action.name, true);
        }
        else
        {
            Debug.Log("Not enough mana");
        }
    }

    protected override void OnDodgeClick(InputAction.CallbackContext ctx)
    {
        base.OnDodgeClick(ctx);
        PlayerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Dodge);
    }

    IEnumerator AttackRoutine(Transform target, string inputName)
    {
        yield return null;
        MoveData _attackingMove = PlayerChar.PlayerCombat.InvokeAttack(PlayerChar.CurrHoldDir, target, inputName);

        if (_attackingMove != null)
        {
            _attacking = true;
            float t = 0;
            while (t < _attackingMove.duration)
            {
                yield return null;
                t += Time.deltaTime;
                //Debug.Log($"attack move timer : {t}");
            }
        }
        if (PlayerChar.PlayerCombat.BufferedAttack == false)
        {
            _attacking = false;
            _attackingInitiatedFromEnterState = false;
        }
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (_attacking == false)
        {
            Debug.Log($"_attacking == falsee to ENgage | {Time.time} |");
            return PlayerStateMachine.EPlayerState.Engage;
        }
        return PlayerStateMachine.EPlayerState.Attack;
    }
}
