using Combat2D;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Xrofng;

public class PlayerStateMain : BasePlayerState
{
    private float _moveInputX;

    public PlayerStateMain(PlayerStateMachine.EPlayerState key, PlayerStateMachine playerStateMachine, PlayerChar pChar) : base(key, playerStateMachine, pChar)
    {
    }

    public enum InputHoldDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    public override void EnterState()
    {
        base.EnterState();
        //InputController.SetDodge(PlayerChar.OnDodge);
    }

    public override void ExitState()
    {
        base.ExitState();
        PlayerChar.CurrHoldDir = InputHoldDirection.None;
    }

    protected override void OnLeftClick(InputAction.CallbackContext ctx)
    {
        base.OnLeftClick(ctx);
        GoToAttackState(ctx.action.name, true);
    }
    protected override void OnRightClick(InputAction.CallbackContext ctx)
    {
        base.OnRightClick(ctx);
        if (PlayerChar.PlayerCombat.BulletMana.Consume())
        {
            GoToAttackState(ctx.action.name, false);
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

    private void GoToAttackState(string inputActionName, bool requireTarget = true)
    {
        PlayerChar.PlayerCombat.InputOnEnter = inputActionName;
        PlayerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Attack);
    }

    protected override void OnMoveInputUpdate(Vector2 inputDir)
    {
        base.OnMoveInputUpdate(inputDir);
        PlayerChar.CurrHoldDir = ResolveFaceHoldDirection(inputDir);
        _moveInputX = inputDir.x;
        HandleHorizontalMove();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        //HandleHorizontalMove();
    }

    protected virtual void HandleHorizontalMove()
    {
        if (Mathf.Abs(_moveInputX) < 0.01f){ return; }

        Vector3 move = new Vector3(_moveInputX, 0, 0);

        PlayerChar.transform.position += move * PlayerChar.MidAirMoveSpeed * Time.deltaTime;
    }

    private InputHoldDirection ResolveFaceHoldDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.01f)
            return InputHoldDirection.Down; // 👈 default = Spot Dodge

        dir.Normalize();

        if (dir.y > 0.5f)
            return InputHoldDirection.Up;

        if (dir.y < -0.5f)
            return InputHoldDirection.Down;

        if (dir.x < -0.5f)
            return InputHoldDirection.Left;

        if (dir.x > 0.5f)
            return InputHoldDirection.Right;

        return InputHoldDirection.None;
    }
}
