using UnityEngine;
using UnityEngine.AI;

public class RepositionState : EnemyBaseState
{
    public RepositionState(EnemyContext context, EnemyMachine.EnemyState key, 
        float repositionSpeed, float repositionRadius) : base(context, key)
    {
        _repositionSpeed = repositionSpeed;
        _repositionRadius = repositionRadius;
    }

    private readonly int RunHash = Animator.StringToHash("Run");
    private float _repositionSpeed;
    private float _repositionRadius;
    private bool _arrived;
    private Vector3 repositionPoint;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.speed = _repositionSpeed;
        _arrived = false;
        repositionPoint = GetValidPoint();
        agent.SetDestination(repositionPoint);
        animator.CrossFade(RunHash, 0.02f);
    }

    public override void UpdateState()
    {
        var agent = _context.GetAgent();

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            _arrived = true;
        }
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        var damage = _context.GetDamage();
        var dead = _context.GetDead();

        if (dead) return EnemyMachine.EnemyState.Death;

        if (damage) return EnemyMachine.EnemyState.Damage;

        if (_arrived) return EnemyMachine.EnemyState.FocusIdle;

        return EnemyMachine.EnemyState.Reposition;
    }

    Vector3 GetValidPoint() // Will return a random point within _repositionRadius
    {
        var position = _context.GetTransform().position;
        Vector3 result = new Vector3(
            Random.Range(-_repositionRadius, _repositionRadius) + position.x, 
            position.y,
            Random.Range(-_repositionRadius, _repositionRadius) + position.z);

        Debug.Log("Reposition position: " + result.ToString());
        NavMeshHit hit;
        while (!NavMesh.SamplePosition(result, out hit, 1f, LayerMask.NameToLayer("Navigation")))
        {

            result = new Vector3(
                Random.Range(-_repositionRadius, _repositionRadius) + position.x,
                position.y,
                Random.Range(-_repositionRadius, _repositionRadius) + position.z);
            Debug.Log("New Reposition position: " + result.ToString());
        }

        return hit.position;
    }
}
