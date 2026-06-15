using Combat2D;
using UnityEngine;
using Xrofng;

public class PlayerStateEngage : PlayerStateMain
{
    PlayerChar.FallingSetting fallingSetting;

    public PlayerStateEngage(PlayerStateMachine.EPlayerState key, PlayerStateMachine playerStateMachine, PlayerChar pChar, PlayerChar.FallingSetting setting) : base(key, playerStateMachine, pChar)
    {
        this.fallingSetting = setting;
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        if (PlayerChar.HasNearbyEnemy == false)
        {
            return PlayerStateMachine.EPlayerState.Field;
        }
        return PlayerStateMachine.EPlayerState.Engage;
    }

    public override void EnterState()
    {
        base.EnterState();
        fallingSetting.Timer.StartTimer();
        PlayerChar.Rigdibody.gravityScale = fallingSetting.GravityAtZero;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        fallingSetting.Timer.UpdateTimer(Time.deltaTime);
        PlayerChar.Rigdibody.gravityScale = fallingSetting.ComputeGravity();
        if (fallingSetting.Timer.CompletionRatio >= 1)
        {
            PlayerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Field);
        }
    }
}
