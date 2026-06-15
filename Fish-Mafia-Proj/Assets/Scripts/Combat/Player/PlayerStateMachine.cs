using Combat2D;
using System;
using UnityEngine;
using Xrofng;

public class PlayerStateMachine : StateManager<PlayerStateMachine.EPlayerState>
{
    public InputController inputController;
    public PlayerChar playerChar;

    public enum EPlayerState
    {
        Field,
        Engage,
        Attack,
        Dodge,
        Knocked,
        CantControlled,
    }

    protected override void Awake()
    {
        base.Awake();
        inputController = FindAnyObjectByType<InputController>();
        playerChar = FindAnyObjectByType<PlayerChar>();
    }

    protected override void SetUpStateDict()
    {
        SetUpState(new PlayerStateField(EPlayerState.Field, this, playerChar, playerChar.OnField));
        SetUpState(new PlayerStateEngage(EPlayerState.Engage, this, playerChar, playerChar.Engaging));
        SetUpState(new PlayerStateAttacking(EPlayerState.Attack, this, playerChar));
        SetUpState(new PlayerStateDodge(EPlayerState.Dodge, this, playerChar));
        SetUpState(new PlayerStateKnocked(EPlayerState.Knocked, this, playerChar));
        SetUpState(new PlayerStateCantControlled(EPlayerState.CantControlled, this, playerChar));
    }

    protected override void SetInitialState()
    {
        SetState(EPlayerState.Field);
    }
}
