using Combat2D;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateField : PlayerStateMain, IEventSubcriber<PlayerStateAttacking.EvsAttackInitiated>
{
    PlayerChar.FallingSetting fallingSetting;

    public PlayerStateField(PlayerStateMachine.EPlayerState key, PlayerStateMachine playerStateMachine, PlayerChar pChar, PlayerChar.FallingSetting setting) : base(key, playerStateMachine, pChar)
    {
        this.fallingSetting = setting;
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return  PlayerStateMachine.EPlayerState.Field;
    }

    public override void EnterState()
    {
        base.EnterState();
        PlayerChar.Rigdibody.gravityScale = fallingSetting.GravityAtZero;
        fallingSetting.Timer.StartTimer();
        //EventBusRegister.EventBusSubcribe(this);
    }

    public override void ExitState()
    {
        base.ExitState();
        //EventBusRegister.EventBusUnscribe(this);
    }

    public void OnEventBusTrigger(PlayerStateAttacking.EvsAttackInitiated eventType)
    {
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        fallingSetting.Timer.UpdateTimer(Time.deltaTime);
        PlayerChar.Rigdibody.gravityScale = fallingSetting.ComputeGravity();
    }
}
