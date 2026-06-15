using Combat2D;
using System;
using UnityEngine;
using Xrofng;

public class KokonutStateMachine : StateManager<KokonutStateMachine.EEnemyState>
{
    public EEnemyState InitialState;

    private PlayerChar playerChar;

    public Kokonut Kokonut;

    public KokonutHivemind HiveMind { get; private set; }

    public enum EEnemyState
    {
        Pause,
        Surround,
        Attack,
        FleeAround
    }

    protected override void Awake()
    {
        base.Awake();
        Kokonut = GetComponent<Kokonut>();
        playerChar = FindAnyObjectByType<PlayerChar>();
    }

    protected override void SetUpStateDict()
    {
        SetUpState(new EnemyStatePause(EEnemyState.Pause, this, playerChar));
        SetUpState(new EnemyStateSurround(EEnemyState.Surround, this, playerChar, Kokonut.SurroundSetting));
        SetUpState(new EnemyStateAttack(EEnemyState.Attack, this, playerChar, Kokonut.AttackSetting, Kokonut.HitBox));
        SetUpState(new EnemyStateFleeAround(EEnemyState.FleeAround, this, playerChar, Kokonut.FleeAroundSetting));
    }

    protected override void SetInitialState()
    {
        SetState(InitialState);
    }

    public void RegisterHiveMind(KokonutHivemind kokonutHivemind)
    {
        HiveMind = kokonutHivemind;
    }
}
