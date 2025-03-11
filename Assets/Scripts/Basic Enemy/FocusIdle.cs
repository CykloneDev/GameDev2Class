using UnityEngine;

public class FocusIdle : EnemyBaseState
{
    public FocusIdle(EnemyContext context, EnemyMachine.EnemyState key, float radius,
        float rotationSpeed) : base(context, key)
    {
        _radius = radius;
        _rotationSpeed = rotationSpeed;
    }

    private readonly int IdleHash = Animator.StringToHash("Idle");
    private Transform _playerTransform;
    private float _radius;
    private float _rotationSpeed;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        _playerTransform = GameManager.instance.GetPlayerTransform();
        agent.isStopped = true;
        agent.Warp(_context.GetTransform().position);
        agent.updatePosition = true;
        agent.updateRotation = false;

        animator.CrossFade(IdleHash, 0.02f);
    }

    public override void UpdateState()
    {
        var transform = _context.GetTransform();

        var direction = _playerTransform.position - transform.position;
        direction.y = 0;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
            _rotationSpeed * Time.deltaTime);
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        var distance = Vector3.Distance(_context.GetTransform().position, _playerTransform.position);
        var playerDetected = _context.GetPlayerDetector().PlayerDetected();
        var chase = _context.UseChase();
        var flee = _context.UseFlee();
        var attack = _context.UseAttack();

        if (!playerDetected) return EnemyMachine.EnemyState.RandomIdle;

        if(chase)
        {
            if (distance >= _radius) return EnemyMachine.EnemyState.Chase;
        }

        if(flee)
        {
            if (distance <= _radius) return EnemyMachine.EnemyState.Flee;
        }

        if (attack)
        {
            if (distance <= _radius) return EnemyMachine.EnemyState.Attack;
        }

        return EnemyMachine.EnemyState.FocusIdle;
    }
}