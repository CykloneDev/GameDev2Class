using UnityEngine;

public class FocusIdle : EnemyBaseState
{
    public FocusIdle(EnemyContext context, EnemyMachine.EnemyState key, float radius,
        float rotationSpeed, float attackRange) : base(context, key)
    {
        _radius = radius;
        _rotationSpeed = rotationSpeed;
        _attackRange = attackRange;
    }

    private readonly int IdleHash = Animator.StringToHash("Idle");
    private Transform _playerTransform;
    private float _radius;
    private float _rotationSpeed;
    private float _attackRange;

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
        var damage = _context.GetDamage();
        var dead = _context.GetDead();

        if (dead) return EnemyMachine.EnemyState.Death;

        if (damage) return EnemyMachine.EnemyState.Damage;

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
            if (distance <= _attackRange) return EnemyMachine.EnemyState.Attack;
        }

        return EnemyMachine.EnemyState.FocusIdle;
    }
}