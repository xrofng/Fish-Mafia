using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : BetterMonoBehaviour where EState : System.Enum
{
    protected Dictionary<EState, BaseState<EState>> States =
        new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;

    public EState PrevStateKey;

    [ReadOnly]
    public string CurrentStateId;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        SetUpStateDict();
        SetInitialState();
        CurrentState.EnterState();
    }

    protected abstract void SetUpStateDict();

    /// <summary>
    /// Method to call at Awake of each StateMachine to setup state dict
    /// </summary>
    /// <param name="state"></param>
    protected void SetUpState(BaseState<EState> state)
    {
        States.Add(state.StateKey, state);
    }

    protected override void LateUpdate()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(EState stateKey)
    {
        IsTransitioningState = true;

        PrevStateKey = CurrentState.StateKey;
        CurrentState.ExitState();

        SetState(stateKey);
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    protected override void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }

    protected void SetState(EState state)
    {
        CurrentState = States[state];
        CurrentStateId = state.ToString();
    }

    protected abstract void SetInitialState();
}
