using UnityEngine;
using UnityEngine.AI;

public class WanderState : EnemyBaseState
{
    public WanderState(EnemyContext context, EnemyMachine.EnemyState key, float range) : base(context, key)
    {
        _range = range;
    }

    private float _range;
    private readonly int WalkHash = Animator.StringToHash("Walk");
    private bool _complete;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.stoppingDistance = 0.1f;
        agent.SetDestination(GetValidPosition());

        animator.CrossFade(WalkHash, 0.2f);
        _complete = false;
    }

    public override void UpdateState()
    {
        var agent = _context.GetAgent();

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            _complete = true;
        }
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        var playerDetected = _context.GetPlayerDetector().PlayerDetected();

        if(playerDetected)
        {
            if (_context.UseChase()) return EnemyMachine.EnemyState.Chase;
            else if (_context.UseFlee()) return EnemyMachine.EnemyState.Flee;
        }

        if (_complete) return EnemyMachine.EnemyState.RandomIdle;

        return EnemyMachine.EnemyState.Wander;
    }

    Vector3 GetValidPosition()
    {
        Vector3 result = _context.GetTransform().position;
        result.x += Random.Range(-_range, _range);
        result.z += Random.Range(-_range, _range);
        NavMeshHit hit;
        while (!NavMesh.SamplePosition(result, out hit, 0, 1))
        {
            result.x += Random.Range(-_range, _range);
            result.z += Random.Range(-_range, _range);
        }

        return hit.position;
    }
}
