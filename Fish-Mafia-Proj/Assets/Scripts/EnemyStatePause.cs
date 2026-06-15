using Combat2D;

public class EnemyStatePause : BaseEnemyState
{
    public EnemyStatePause(KokonutStateMachine.EEnemyState key, KokonutStateMachine kokonutStateMachine, PlayerChar pChar) : base(key, kokonutStateMachine, pChar)
    {
    }

    public override KokonutStateMachine.EEnemyState GetNextState()
    {
        return KokonutStateMachine.EEnemyState.Pause;
    }
}
