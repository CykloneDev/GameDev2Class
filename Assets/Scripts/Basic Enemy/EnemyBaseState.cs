using UnityEngine;

public abstract class EnemyBaseState : BaseState<EnemyMachine.EnemyState>
{
    public EnemyBaseState(EnemyContext context, EnemyMachine.EnemyState key) : base(key)
    {
        _context = context;
        StateKey = key;
    }
        
    [SerializeField] protected EnemyMachine.EnemyState StateKey { get; private set; }
    protected EnemyContext _context;

    // EnterState
    public override void EnterState() { }

    // UpdateState
    public override void UpdateState() { }

    // ExitState
    public override void ExitState() { }

    // GetNextState
    public override EnemyMachine.EnemyState GetNextState() { return EnemyMachine.EnemyState.Idle; }

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
