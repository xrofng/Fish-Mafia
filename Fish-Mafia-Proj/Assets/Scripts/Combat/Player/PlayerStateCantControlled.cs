using Combat2D;

public class PlayerStateCantControlled : BasePlayerState
{
    public PlayerStateCantControlled(PlayerStateMachine.EPlayerState key, PlayerStateMachine playerStateMachine, PlayerChar pChar) : base(key, playerStateMachine, pChar)
    {
    }
}