using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class PStateManager<PState> : MonoBehaviour where PState : Enum
{
    protected Dictionary<PState, PBaseState<PState>> States = new Dictionary<PState, PBaseState<PState>>();
    protected PBaseState<PState> CurrentState;

    protected bool IsTransitioningState = false;

    void Start()
    {
        CurrentState.EnterState();
    }

    void Update()
    {
        PState nextStateKey = CurrentState.GetNextState();

        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        
        } else if (!IsTransitioningState){
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(PState stateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }

}
