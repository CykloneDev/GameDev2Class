using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States;

    public BaseState<EState> CurrentState { get; protected set; }

    private bool _isTransitioning = false;

    void Start()
    {
        CurrentState.EnterState();
    }


    public virtual void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.StateKey))
            CurrentState.UpdateState();

        else if(!nextStateKey.Equals(CurrentState.StateKey) && !_isTransitioning)
            TransitionToState(nextStateKey);
    }

    private void TransitionToState(EState stateKey)
    {
        _isTransitioning = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        _isTransitioning = false;
    }
}
