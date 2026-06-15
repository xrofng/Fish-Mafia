using Combat2D;
using System;
using UnityEngine;

public class EnemyStateFleeAround : EnemyStateSurround
{
    public override bool Attackable => false;

    public EnemyStateFleeAround(KokonutStateMachine.EEnemyState key, KokonutStateMachine kokonutStateMachine, PlayerChar pChar, SurroundSetting set) : base(key, kokonutStateMachine, pChar, set)
    {

    }

    protected override void OnSurroundEnd()
    {
        base.OnSurroundEnd();
        StateMachine.TransitionToState(KokonutStateMachine.EEnemyState.Surround);
    }

    public override void ExitState()
    {
        base.ExitState();
        // release attack slot in hivemind
        if (StateMachine.HiveMind)
        {
            StateMachine.HiveMind.EndAttack(StateMachine);
        }
    }

    public override KokonutStateMachine.EEnemyState GetNextState()
    {
        return KokonutStateMachine.EEnemyState.FleeAround;
    }
}