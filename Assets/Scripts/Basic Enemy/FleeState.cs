using UnityEngine;
using UnityEngine.AI;

public class FleeState : EnemyBaseState
{
    public FleeState(EnemyContext context, EnemyMachine.EnemyState key, 
        float fleeSpeed, float fleeRadius) : base(context, key)
    {
        _fleeSpeed = fleeSpeed;
        _fleeRadius = fleeRadius;
    }

    private readonly int RunHash = Animator.StringToHash("Run");
    private float _fleeSpeed;
    private float _fleeRadius;
    private bool _arrived;
    private Vector3 fleePoint;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.speed = _fleeSpeed;
        _arrived = false;
        fleePoint = GetValidPoint();
        agent.SetDestination(fleePoint);
        animator.CrossFade(RunHash, 0.02f);
    }

    public override void UpdateState()
    {
        var agent = _context.GetAgent();

        agent.SetDestination(fleePoint);

        if (agent.remainingDistance <= 0.3f)
        {
            _arrived = true;
        }
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        if (_arrived) return EnemyMachine.EnemyState.FocusIdle;

        return EnemyMachine.EnemyState.Flee;
    }

    Vector3 GetValidPoint() // Will return a random point within _fleeRadius
    {
        var position = _context.GetTransform().position;
        Vector3 result = new Vector3(
            Random.Range(-_fleeRadius, _fleeRadius) + position.x, 
            position.y,
            Random.Range(-_fleeRadius, _fleeRadius) + position.z);

        Debug.Log("Flee position: " + result.ToString());
        NavMeshHit hit;
        while (!NavMesh.SamplePosition(result, out hit, 1f, LayerMask.NameToLayer("Navigation")))
        {

            result = new Vector3(
                Random.Range(-_fleeRadius, _fleeRadius) + position.x,
                position.y,
                Random.Range(-_fleeRadius, _fleeRadius) + position.z);
            Debug.Log("New Flee position: " + result.ToString());
        }

        return hit.position;
    }
}
