using System;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState key)
    {
        StateKey = key;
    }

    public EState StateKey { get; private set; }

    // EnterState
    public abstract void EnterState();

    // UpdateState
    public abstract void UpdateState();

    // ExitState
    public abstract void ExitState();

    // GetNextState
    public abstract EState GetNextState();

    // OnTriggerEnter
    public abstract void OnTriggerEnter(Collider collider);

    // OnTriggerStay
    public abstract void OnTriggerStay(Collider collider);

    // OnTriggerExit
    public abstract void OnTriggerExit(Collider collider);

    // OnCollisionEnter
    public abstract void OnCollisionEnter(Collision collision);

    // OnCollisionStay
    public abstract void OnCollisionStay(Collision collision);

    // OnCollisionExit
    public abstract void OnCollisionExit(Collision collision);

}
