using UnityEngine;
using System;


public abstract class BaseState<EState> where EState : Enum //EState has to be of the type "Enum"
{
    public BaseState(EState key){
        StateKey = key;
        }

    public EState StateKey { get; private set; }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState(); //EState is a generic (Placeholder)
    public abstract void OnTriggerEnter(Collider other);
    public abstract void onTriggerStay(Collider other);
    public abstract void onTriggerExit(Collider other);

}