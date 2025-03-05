using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States;

    protected BaseState<EState> _CurrentState;

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

    public void ExitState()
    {

    }


    public void TransitionToState(EState stateKey)
    {
        _isTransitioning = true;
        _CurrentState.ExitState();
        _CurrentState = States[stateKey];
        _CurrentState.EnterState();
        _isTransitioning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _CurrentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        _CurrentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _CurrentState.OnTriggerExit(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _CurrentState.OnCollisionEnter(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        _CurrentState.OnCollisionStay(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        _CurrentState.OnCollisionExit(collision);
    }
}
