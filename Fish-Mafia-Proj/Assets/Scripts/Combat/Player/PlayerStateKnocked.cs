using Combat2D;

public class PlayerStateKnocked : BasePlayerState
{
    public PlayerStateKnocked(PlayerStateMachine.EPlayerState key, PlayerStateMachine playerStateMachine, PlayerChar pChar) : base(key, playerStateMachine, pChar)
    {
        
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return PlayerStateMachine.EPlayerState.Knocked;
    }

    public override void EnterState()
    {
        base.EnterState();
        PlayerChar.PlayerCombat.animator.Play("Knocked_Aclip");
    }

    public override void ExitState()
    {
        base.ExitState();
        PlayerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.Engage);
    }
}