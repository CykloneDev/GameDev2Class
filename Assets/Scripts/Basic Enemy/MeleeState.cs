using UnityEngine;

public class MeleeState : EnemyBaseState
{
    public MeleeState(EnemyContext context, EnemyMachine.EnemyState key,  float rotationSpeed) : base(context, key)
    {
        _rotationSpeed = rotationSpeed;
    }

    private float _rotationSpeed;
    private string _damageLayer;
    private readonly int MeleeHash = Animator.StringToHash("Combo 1");
    private int _currentShotCount;
    private bool _attacking;
    private float _attackTime;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        agent.isStopped = true;
        agent.updateRotation = false;

        animator.Play(MeleeHash, 0, 0f);
        _attackTime = 0;
        _attacking = true;
    }

    public override void UpdateState()
    {
        var transform = _context.GetTransform();
        var animator = _context.GetAnimator();
        var playerTransform = GameManager.instance.GetPlayerTransform();
        var direction = playerTransform.position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            _rotationSpeed * Time.deltaTime);

        _attackTime += Time.deltaTime;
        if (_attackTime > _context.GetMeleeTime())
            _attacking = false;
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        var cover = _context.UseCover();

        if (!_attacking)
        {
            if (cover) return EnemyMachine.EnemyState.Cover;
            else return EnemyMachine.EnemyState.FocusIdle;
        }

        return EnemyMachine.EnemyState.Melee;
    }

    public override void ExitState()
    {
        var agent = _context.GetAgent();

        agent.Warp(_context.GetTransform().position);
    }
}
