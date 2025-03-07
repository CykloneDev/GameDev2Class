using UnityEngine;

public class ChaseState : EnemyBaseState
{
    public ChaseState(EnemyContext context, EnemyMachine.EnemyState key, float chaseSpeed, 
        float stopRadius, float navRefreshTime) : 
        base(context, key)
    {
        _chaseSpeed = chaseSpeed;
        _stopRadius = stopRadius;
        _navRefeshLimit = navRefreshTime;
    }

    private float _chaseSpeed;
    private float _stopRadius;
    private readonly int RunHash = Animator.StringToHash("Run");
    private float _navRefeshLimit;
    private float _currentNavRefresh;
    private Transform _playerTransform;
    private bool _inRange;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();
        _playerTransform = GameManager.instance.GetPlayerTransform();

        animator.CrossFade(RunHash, 0.02f);
        agent.speed = _chaseSpeed;
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.SetDestination(_playerTransform.position);
        _currentNavRefresh = 0;
        _inRange = false;
    }

    public override void UpdateState()
    {
        var agent = _context.GetAgent();

        _currentNavRefresh += Time.deltaTime;
        if(_currentNavRefresh >= _navRefeshLimit)
        {
            agent.SetDestination(_playerTransform.position);
            _currentNavRefresh = 0;
        }

        if(agent.remainingDistance <= _stopRadius)
        {
            _inRange = true;
        }
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        if (_inRange) return EnemyMachine.EnemyState.FocusIdle;

        return EnemyMachine.EnemyState.Chase;
    }
}
