using UnityEngine;

public class IdleState : EnemyBaseState
{
    public IdleState(EnemyContext context, EnemyMachine.EnemyState key, float randomMin, float randomMax) : base(context, key)
    {
        _randomMin = randomMin;
        _randomMax = randomMax;
    }

    private readonly int IdleHash = Animator.StringToHash("Idle");
    private float _randomMin;
    private float _randomMax;
    private float _waitTime;

    public override void EnterState()
    {
        // Used to set what should happen when we enter the Idle state
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();
        // For this project, I'm going to keep things simple and not use Root Motion for any movement animations.
        // This isn't super hard to setup and if the team wants, I'll add it in. 

        agent.isStopped = true;
        agent.Warp(_context.GetTransform().position);

        animator.CrossFade(IdleHash, 0.2f);
        _waitTime = Random.Range(_randomMin, _randomMax);
    }

    public override void ExitState()
    {
        // Anything we need to reset before leaving the Idle state should be done here
        // Idle is pretty simple, so nothing yet. 
    }

    public override void UpdateState()
    {
        // Here we can put a timer to play a idle/bored animation
        // Or anything we want to keep track of each frame in the Idle state
        _waitTime -= Time.deltaTime;
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        // Here we define what are the requirements to transition to another state
        // We will use the members of the context to determine this
        var waypoints = _context.UseWaypoints();
        var playerDetected = _context.GetPlayerDetector().PlayerDetected();
        var chase = _context.UseChase();

        if(_waitTime < 0 && waypoints) return EnemyMachine.EnemyState.Waypoint;

        if(playerDetected)
        {
            if (chase) return EnemyMachine.EnemyState.Chase;
            // flee
        }

        return EnemyMachine.EnemyState.RandomIdle;
    }
}
