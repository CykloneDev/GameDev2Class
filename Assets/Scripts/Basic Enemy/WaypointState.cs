using UnityEngine;
using System.Collections.Generic;
using System.Drawing;

public class WaypointState : EnemyBaseState
{
    public WaypointState(EnemyContext context, EnemyMachine.EnemyState key, float waypointRange, 
        float walkSpeed) : base(context, key)
    {
        _index = 0;
        _walkSpeed = walkSpeed;
        var position = _context.GetTransform().position;

        // Add waypoints from GameManager that are within range
        foreach (var point in GameManager.instance.waypointList)
        {
            var dist = Vector3.Distance(position, point.transform.position);
            if (dist <= waypointRange)
                _waypoints.Add(point.transform.position);
        }

        // Sort waypoints based on distance from enemy
        _waypoints.Sort((a, b) => a.sqrMagnitude.CompareTo(b.sqrMagnitude));
    }

    private List<Vector3> _waypoints = new List<Vector3>();
    private int _index;
    private readonly int WalkHash = Animator.StringToHash("Walk");
    private bool _waypointReached;
    private float _walkSpeed;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        // Check if the waypoint is valid
        if (_index >= _waypoints.Count)
        {
            _index = 0;
        }

        _waypointReached = false;
        animator.CrossFade(WalkHash, 0.2f);
        agent.speed = _walkSpeed;
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.SetDestination(_waypoints[_index]);
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
        var flee = _context.UseFlee();
        var dead = _context.GetDead();

        if (dead) return EnemyMachine.EnemyState.Death;

        if (playerDetected)
        {
            if (chase) return EnemyMachine.EnemyState.Chase;
            if (flee) return EnemyMachine.EnemyState.Flee;
        }

        if (_waypointReached) return EnemyMachine.EnemyState.RandomIdle;

        return EnemyMachine.EnemyState.Waypoint;
    }
}
