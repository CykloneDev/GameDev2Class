using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States;

    [SerializeField] protected BaseState<EState> _CurrentState;

    private bool _isTransitioning = false;

    void Start()
    {
        _CurrentState.EnterState();
    }


    private void Update()
    {
        EState nextStateKey = _CurrentState.GetNextState();

        if (nextStateKey.Equals(_CurrentState.StateKey))
            _CurrentState.UpdateState();
        else if(!_isTransitioning)
            TransitionToState(nextStateKey);
    }

    public void TransitionToState(EState stateKey)
    {
        _isTransitioning = true;
        _CurrentState.ExitState();
        _CurrentState = States[stateKey];
        _CurrentState.EnterState();
        _isTransitioning = false;
    }

    public virtual void ExitState()
    {

    }
}
