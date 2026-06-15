using Combat2D;
using UnityEngine;

public class BaseEnemyState : BaseState<KokonutStateMachine.EEnemyState>
{
    protected KokonutStateMachine.EEnemyState Statkey;
    protected KokonutStateMachine StateMachine;
    protected PlayerChar PlayerChar;
    protected Kokonut Kokonut;


    public BaseEnemyState(KokonutStateMachine.EEnemyState key, KokonutStateMachine kokonutStateMachine, PlayerChar pChar) : base(key)
    {
        Statkey = key;
        PlayerChar = pChar;
        StateMachine = kokonutStateMachine;
        Kokonut = kokonutStateMachine.Kokonut;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override KokonutStateMachine.EEnemyState GetNextState()
    {
        return KokonutStateMachine.EEnemyState.Pause;
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }

    public override void OnTriggerStay(Collider other)
    {
    }

    public override void UpdateState()
    {
        
    }
}