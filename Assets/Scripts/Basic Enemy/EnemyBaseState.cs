using UnityEngine;

public abstract class EnemyBaseState : BaseState<EnemyState>
{
    public EnemyBaseState(EnemyContext context, EnemyState key) : base(key)
    {
        _context = context;
        StateKey = key;
    }

    public EnemyState StateKey { get; private set; }
    protected EnemyContext _context;

    // EnterState
    public override void EnterState() { }

    // UpdateState
    public override void UpdateState() { }

    // ExitState
    public override void ExitState() { }

    // GetNextState
    public override EnemyState GetNextState() { return EnemyState.Idle; }

    // OnTriggerEnter
    public override void OnTriggerEnter(Collider collider) { }

    // OnTriggerStay
    public override void OnTriggerStay(Collider collider) { }

    // OnTriggerExit
    public override void OnTriggerExit(Collider collider) { }

    // OnCollisionEnter
    public override void OnCollisionEnter(Collision collision) { }

    // OnCollisionStay
    public override void OnCollisionStay(Collision collision) { }

    // OnCollisionExit
    public override void OnCollisionExit(Collision collision) { }
}
