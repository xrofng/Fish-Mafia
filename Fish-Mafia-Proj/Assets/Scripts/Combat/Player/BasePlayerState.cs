using Combat2D;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayerState : BaseState<PlayerStateMachine.EPlayerState>
{
    protected PlayerStateMachine PlayerStateMachine;
    protected InputController InputController;
    protected PlayerChar PlayerChar;
    protected PlayerTargeting PlayerTargeting;

    public struct EvsPlayerStateEntered
    {
        public PlayerStateMachine.EPlayerState PrevState;
        public PlayerStateMachine.EPlayerState EnteredState;

        public EvsPlayerStateEntered(PlayerStateMachine.EPlayerState prevState, PlayerStateMachine.EPlayerState enteredState)
        {
            PrevState = prevState;
            EnteredState = enteredState;
        }

        public bool IsTransitionOf(PlayerStateMachine.EPlayerState from, PlayerStateMachine.EPlayerState to)
        {
            return PrevState == from && EnteredState == to;
        }
    }

    public struct EvsPlayerStateExited
    {
        public PlayerStateMachine.EPlayerState ExitedState;

        public EvsPlayerStateExited(PlayerStateMachine.EPlayerState exitedState)
        {
            ExitedState = exitedState;
        }
    }

    public BasePlayerState(PlayerStateMachine.EPlayerState key, PlayerStateMachine playerStateMachine, PlayerChar pChar) : base(key)
    {
        InputController = playerStateMachine.inputController;
        PlayerChar = pChar;
        PlayerTargeting = pChar.PlayerTargeting;
        PlayerStateMachine = playerStateMachine;
    }

    public override void EnterState()
    {
        ClearStateVariable();
        EventBus.TriggerEvent(new EvsPlayerStateEntered(PlayerStateMachine.PrevStateKey, StateKey));
        InputController.SetLeftClick(OnLeftClick);
        InputController.SetRightClick(OnRightClick);
        InputController.SetDodgeClick(OnDodgeClick);
        InputController.SetMoveUpdate(OnMoveInputUpdate);
    }

    protected virtual void ClearStateVariable()
    {

    }

    public override void ExitState()
    {
        EventBus.TriggerEvent(new EvsPlayerStateExited(StateKey));
    }

    public override PlayerStateMachine.EPlayerState GetNextState()
    {
        return PlayerStateMachine.EPlayerState.Field;
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

    protected virtual void OnLeftClick(InputAction.CallbackContext ctx)
    {
        // no implementation
    }

    protected virtual void OnRightClick(InputAction.CallbackContext ctx)
    {
        // no implementation
    }

    protected virtual void OnDodgeClick(InputAction.CallbackContext ctx)
    {
        // no implementation
    }

    protected virtual void OnMoveInputUpdate(Vector2 vector)
    {
        // no implementation
    }
}
