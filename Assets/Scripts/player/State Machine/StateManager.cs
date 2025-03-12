using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{

    //Dictionary and Current State Definition
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;

    //Bool
    protected bool IsTransitioningState = false;

    private void Start() {
        CurrentState.EnterState();
    }

    private void Update() {
        EState nextStateKey = CurrentState.GetNextState();

        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        CurrentState.UpdateState();
        else
        {
            TransitionToState(nextStateKey);
        }
    }


    public void TransitionToState(EState stateKey)
    {
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    private void OnTriggerEnter(Collider other) { 
    
    }
    private void OnTriggerStay(Collider other) { }
    private void OnTriggerExit(Collider other) { }
}
