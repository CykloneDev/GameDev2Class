using UnityEngine;

public class WaypointState : EnemyBaseState
{
    public WaypointState(EnemyContext context, EnemyMachine.EnemyState key, Transform[] waypoints) : base(context, key)
    {
        _index = 0;
        _waypoints = waypoints;
    }

    private Transform[] _waypoints;
    private int _index;
    private readonly int WalkHash = Animator.StringToHash("Walk");
    private bool _waypointReached;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        // Check if the waypoint is valid
        if (_index >= _waypoints.Length)
        {
            _index = 0;
        }

        _waypointReached = false;
        animator.CrossFade(WalkHash, 0.2f);
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.SetDestination(_waypoints[_index].position);
        ++_index;
    }

    public override void UpdateState()
    {
        var agent = _context.GetAgent();

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            _waypointReached = true;
        }
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        if (_waypointReached) return EnemyMachine.EnemyState.Idle;

        return EnemyMachine.EnemyState.Waypoint;
    }
}
