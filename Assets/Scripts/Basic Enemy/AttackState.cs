using UnityEngine;

public class AttackState : EnemyBaseState
{
    public AttackState(EnemyContext context, EnemyMachine.EnemyState key, GameObject bulletPrefab, float shotFrequency, 
       int shotCount, float rotationSpeed, string damageLayer) : base(context, key)
    {
        _bulletPrefab = bulletPrefab;
        _shotFrequency = shotFrequency;
        _shotCount = shotCount;
        _damageLayer = damageLayer;
        _rotationSpeed = rotationSpeed;
    }

    private GameObject _bulletPrefab;
    private Transform _shotPoint;
    private float _shotFrequency;
    private float _currentShotFrequency;
    private int _shotSpeed;
    private int _shotCount;
    private int _shotDamage;
    private float _rotationSpeed;
    private string _damageLayer;
    private readonly int SingleShotHash = Animator.StringToHash("Single Shot");
    private int _currentShotCount;
    private bool _shooting;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        agent.isStopped = true;
        agent.updateRotation = false;

        _currentShotCount = _shotCount; 
        animator.Play(SingleShotHash, 0, 0f);
        _context.GetMachine().Shoot();
        --_currentShotCount;
        _currentShotFrequency = 0;
        _shooting = true;
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


        if(_currentShotCount > 0)
        {
            _currentShotFrequency += Time.deltaTime;
            if(_currentShotFrequency > _shotFrequency)
            {
                animator.Play(SingleShotHash, 0, 0f);
                _context.GetMachine().Shoot();
                --_currentShotCount;
                _currentShotFrequency = 0;
            }
        }
        else if(_currentShotCount <= 0 )
        {
            _shooting = false;
        }
    }

    public override EnemyMachine.EnemyState GetNextState()
    {
        if (!_shooting) return EnemyMachine.EnemyState.FocusIdle;

        return EnemyMachine.EnemyState.Attack;
    }
}
