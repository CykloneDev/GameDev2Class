using UnityEngine;
using System;

public abstract class PBaseState<PState> where PState : Enum
{
    public PBaseState(PState key)
    {

    }

    public PState StateKey { get; private set; }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract PState GetNextState();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}
