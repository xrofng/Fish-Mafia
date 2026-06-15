using Combat2D;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateDodge : BasePlayerState
{
    private Vector2 _moveInput;
    private HurtBox hurtBox;

    public DodgeSetting Setting;

    private IEnumerator _dodgeRoutine;

    [System.Serializable]
    public class DodgeSetting
    {
        public float DodgeDuration = 0.25f;
        public float Distance = 3f;
        public float iFrameDuration = 0.2f;
    }

    public PlayerStateDodge(PlayerStateMachine.EPlayerState key, PlayerStateMachine playerStateMachine, PlayerChar pChar) : base(key, playerStateMachine, pChar)
    {
        hurtBox = PlayerChar.PlayerCombat.HurtBox;
        Setting = pChar.Dodge;
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return PlayerStateMachine.EPlayerState.Dodge;
    }

    public override void EnterState()
    {
        base.EnterState();
        DoDodge();
        _dodgeRoutine = null;
    }

    private void DoDodge()
    {
        PlayerChar.PlayerCombat.animator.Play("Dodge_Aclip");
        if (_dodgeRoutine != null)
        {
            PlayerChar.CoroutineRunner.StopCoroutine(_dodgeRoutine);
        }
        _dodgeRoutine = DodgeRoutine();
        PlayerChar.CoroutineRunner.StartCoroutine(_dodgeRoutine);
    }

    protected override void OnDodgeClick(InputAction.CallbackContext ctx)
    {
        base.OnDodgeClick(ctx);
        DoDodge();
    }

    protected override void OnMoveInputUpdate(Vector2 inputDir)
    {
        base.OnMoveInputUpdate(inputDir);
        _moveInput = inputDir.normalized;
        PlayerChar.PlayerCombat.SetFacing(_moveInput);
    }

    private IEnumerator DodgeRoutine()
    {
        // delay 1 frame to wait for _moveInputX update
        yield return null;
        float timer = 0f;

        Vector3 start = PlayerChar.transform.position;

        Vector3 target = start + ((Vector3)_moveInput * Setting.Distance);

        // Disable hurtbox (i-frame)
        if (hurtBox != null)
            hurtBox.enabled = false;

        while (timer < Setting.DodgeDuration)
        {
            float t = timer / Setting.DodgeDuration;

            // Smooth dash (ease out feels better)
            float easeT = 1 - Mathf.Pow(1 - t, 2);

            PlayerChar.transform.position = Vector3.Lerp(start, target, easeT);

            timer += Time.deltaTime;

            // Re-enable hurtbox after iframe window
            if (timer >= Setting.iFrameDuration && hurtBox != null && !hurtBox.enabled)
            {
                hurtBox.enabled = true;
            }

            yield return null;
        }

        // Safety: ensure hurtbox is re-enabled
        if (hurtBox != null)
            hurtBox.enabled = true;

        // Transition back (usually idle or move)
        PlayerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Engage);
    }
}
