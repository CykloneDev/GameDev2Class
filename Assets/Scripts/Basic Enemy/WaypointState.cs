using UnityEngine;

public class WaypointState : EnemyBaseState
{
    public WaypointState(EnemyContext context, EnemyMachine.EnemyState key, Transform[] waypoints, 
        float walkSpeed) : base(context, key)
    {
        _index = 0;
        _waypoints = waypoints;
        _walkSpeed = walkSpeed;
    }

    private Transform[] _waypoints;
    private int _index;
    private readonly int WalkHash = Animator.StringToHash("Walk");
    private bool _waypointReached;
    private float _walkSpeed;

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
        agent.speed = _walkSpeed;
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
        var playerDetected = _context.GetPlayerDetector().PlayerDetected();

        var chase = _context.UseChase();

        if (playerDetected)
        {
            if (chase) return EnemyMachine.EnemyState.Chase;
            // flee
        }

        if (_waypointReached) return EnemyMachine.EnemyState.RandomIdle;

        return EnemyMachine.EnemyState.Waypoint;
    }
}
