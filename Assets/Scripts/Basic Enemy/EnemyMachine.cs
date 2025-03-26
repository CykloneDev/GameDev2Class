using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Rigidbody))]
public class EnemyMachine : StateMachine<EnemyMachine.EnemyState>, IDamage
{
    public enum EnemyState
    {
        [InspectorName(null)]
        RandomIdle,
        [InspectorName(null)]
        FocusIdle,
        Waypoint,
        Wander,
        Chase,
        Reposition,
        Cover,
        Attack,
        Melee,
        [InspectorName(null)]
        Damage,
        [InspectorName(null)]
        Death
    }

    [SerializeField] private EnemyContext _context;
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    private Animator _animator;
    private PlayerDetector _playerDetector;
    [SerializeField] private Renderer[] _model;
    [SerializeField] private int _currentHP;
    [SerializeField] private int _maxHP;
    [SerializeField] private bool _dead;
    [SerializeField] private float _minRandomWait, _maxRandomWait;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _chaseStopRadius;
    [SerializeField] private float _chaseStartRadius;
    [SerializeField] private float _chaseRefreshTime;
    [SerializeField] private float _focusIdleRotationSpeed;
    [SerializeField] private float _repositionRadius;
    [SerializeField] private float _coverSearchRange;
    [SerializeField] private float _minHideTime, _maxHideTime;
    public List<EnemyState> StatesUsed;   
    [SerializeField] private float _waypointsRange;
    [SerializeField] private float _wanderRange;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private ParticleSystem _shotEffect;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _shotFrequency;
    [SerializeField] private int _shotSpeed;
    [SerializeField] private int _shotCount;
    [SerializeField] private int _shotDamageAmount;
    [SerializeField] private float _attackRotationSpeed;
    [SerializeField] private float _meleeComboTime;
    [SerializeField] string _damageLayer;
    [SerializeField] GameObject _meleeDamage;
    [SerializeField] GameObject _deathEffect;
    [SerializeField] private Transform _deathPoint;
    private float _currentShotTime;

    public EnemyState currentState;

#region Unity Methods
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>(); 
        _rb = GetComponent<Rigidbody>();
        _playerDetector = GetComponentInChildren<PlayerDetector>();
    }
    
    void Start()
    {
        _context = new EnemyContext(this, transform, _agent, _animator, _rb, 
            _playerDetector, _meleeComboTime);
        InitializeStates();

        _currentHP = _maxHP;
        _dead = false;
        GameManager.instance.UpdateGameGoal(1);
    }

    public override void Update()
    {
        base.Update();
        currentState = CurrentState.StateKey;
    }

    #endregion

    #region EnemyMachine Methods
    void InitializeStates()
    {
        States = new Dictionary<EnemyState, BaseState<EnemyState>>();

        //Debug.Log("Added Idle State to " + gameObject.name);
        States.Add(EnemyState.RandomIdle, new IdleState(_context, EnemyState.RandomIdle,
            _minRandomWait, _maxRandomWait));

       // Debug.Log("Added Damage State to " + gameObject.name);
        States.Add(EnemyState.Damage, new DamageState(_context, EnemyState.Damage, .5f));


       // Debug.Log("Added Death State to " + gameObject.name);
        States.Add(EnemyState.Death, new DeathState(_context, EnemyState.Death));

        // Debug.Log("Added Focus Idle State to " + gameObject.name);
        States.Add(EnemyState.FocusIdle, new FocusIdle(_context, EnemyState.FocusIdle, _chaseStartRadius,
                _focusIdleRotationSpeed, _attackRange));

        if (StatesUsed.Contains(EnemyState.Waypoint))
        {
            //Debug.Log("Added Waypoint State to " + gameObject.name);
            States.Add(EnemyState.Waypoint, new WaypointState(_context, EnemyMachine.EnemyState.Waypoint, 
                _waypointsRange, _walkSpeed)); 
        }

        if(StatesUsed.Contains(EnemyState.Wander))
        {
            States.Add(EnemyState.Wander, new WanderState(_context, EnemyState.Wander, _wanderRange));
        }

        if(StatesUsed.Contains(EnemyState.Chase))
        {
            States.Add(EnemyState.Chase, new ChaseState(_context, EnemyState.Chase, _runSpeed,
                _chaseStopRadius, _chaseRefreshTime));
        }

        if(StatesUsed.Contains(EnemyState.Reposition))
        {
            States.Add(EnemyState.Reposition, new RepositionState(_context, EnemyState.Reposition, _runSpeed, _repositionRadius));
        }

        if(StatesUsed.Contains(EnemyState.Attack))
        {
            States.Add(EnemyState.Attack, new AttackState(_context, EnemyState.Attack, _bulletPrefab, _shotFrequency, 
                _shotCount, _attackRotationSpeed, _damageLayer));
        }

        if(StatesUsed.Contains(EnemyState.Cover))
        {
            States.Add(EnemyState.Cover, new CoverState(_context, EnemyState.Cover, _minHideTime, _maxHideTime, 
                _coverSearchRange, _runSpeed));
        }

        if (StatesUsed.Contains(EnemyState.Melee))
        {           
            States.Add(EnemyState.Melee, new MeleeState(_context, EnemyState.Melee, _attackRotationSpeed));
        }

        CurrentState = States[EnemyState.RandomIdle];
    }

    public bool HasState(EnemyState enemyState)
    {
        return States.ContainsKey(enemyState);
    }

    public void Shoot()
    {
        //_shotEffect?.Play();
        var bullet = Instantiate(_bulletPrefab, _shotPoint.position, _context.GetTransform().rotation);
        bullet.layer = LayerMask.NameToLayer(_damageLayer);
        bullet.GetComponent<Damage>().InitBullet(_shotDamageAmount, _shotSpeed, 3f);
    }

    public void TakeDamage(int damage)
    {
        if(_dead) return;

        _currentHP -= damage;

        if(_currentHP <= 0)
        {
            // Die
            _context.SetDead(true);
            _dead = true;
            GameManager.instance.OnEnemyDefeated();
            if (_deathEffect != null)
                Instantiate(_deathEffect, _deathPoint.position, _deathPoint.rotation);

            return;
        }
        _context.SetDamage(true);
        _context.GetPlayerDetector().SeePlayer();
        StartCoroutine(FlashRed());
    }

    public void HealDamage(int value)
    {
        _currentHP += value;
        if(_currentHP > _maxHP)
            _currentHP = _maxHP;

        StartCoroutine(FlashGreen());
    }

    IEnumerator HitboxRoutine()
    {
        _meleeDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _meleeDamage.SetActive(false);
    }

    void ActivateHitbox()
    {
        StartCoroutine(HitboxRoutine());
    }

    IEnumerator FlashRed()
    {
        foreach(Renderer rend in _model)
        {
            rend.material.color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Renderer rend in _model)
        {
            rend.material.color = Color.white;
        }
        yield return null;
    }

    IEnumerator FlashGreen()
    {
        foreach (Renderer rend in _model)
        {
            rend.material.color = Color.green;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Renderer rend in _model)
        {
            rend.material.color = Color.white;
        }
        yield return null;
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _waypointsRange);


        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _coverSearchRange);
    }
}
