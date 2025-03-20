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
        RandomIdle,
        FocusIdle,
        Waypoint,
        Wander,
        Chase,
        Flee,
        Cover,
        Attack,
        Damage,
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
    [SerializeField] private float _fleeRadius;
    [SerializeField] private float _coverSearchRange;
    [SerializeField] private float _minHideTime, _maxHideTime;
    [SerializeField] private List<EnemyState> StatesUsed;   
    [SerializeField] private float _waypointsRange;
    [SerializeField] private float _wanderRange;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _shotFrequency;
    [SerializeField] private int _shotSpeed;
    [SerializeField] private int _shotCount;
    [SerializeField] private int _shotDamageAmount;
    [SerializeField] private float _shotRotationSpeed;
    [SerializeField] string _damageLayer;
    private float _currentShotTime;

    public EnemyState currentState;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>(); 
        _rb = GetComponent<Rigidbody>();
        _playerDetector = GetComponentInChildren<PlayerDetector>();
    }

    #region Unity Methods
    void Start()
    {
        _context = new EnemyContext(this, transform, _agent, _animator, _rb, _playerDetector);
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

        if (StatesUsed.Contains(EnemyState.Waypoint))
        {
            //Debug.Log("Added Waypoint State to " + gameObject.name);
            _context.SetUseWaypoints(true);
            States.Add(EnemyState.Waypoint, new WaypointState(_context, EnemyMachine.EnemyState.Waypoint, 
                _waypointsRange, _walkSpeed)); 
        }

        if(StatesUsed.Contains(EnemyState.Wander))
        {
            _context.SetUseWaypoints(false);
            States.Add(EnemyState.Wander, new WanderState(_context, EnemyState.Wander, _wanderRange));
        }

        if(StatesUsed.Contains(EnemyState.Chase))
        {
            // Must also use FocusIdle
            if(!States.ContainsKey(EnemyState.FocusIdle))
                States.Add(EnemyState.FocusIdle, new FocusIdle(_context, EnemyState.FocusIdle, _chaseStartRadius,
                _focusIdleRotationSpeed, _attackRange));

            States.Add(EnemyState.Chase, new ChaseState(_context, EnemyState.Chase, _runSpeed,
                _chaseStopRadius, _chaseRefreshTime));
            _context.SetUseChase(true);
        }

        if(StatesUsed.Contains(EnemyState.Flee))
        {
            // Must also use FocusIdle
            if (!States.ContainsKey(EnemyState.FocusIdle))
                States.Add(EnemyState.FocusIdle, new FocusIdle(_context, EnemyState.FocusIdle, _chaseStartRadius,
                _focusIdleRotationSpeed, _attackRange));

            States.Add(EnemyState.Flee, new FleeState(_context, EnemyState.Flee, _runSpeed, _fleeRadius));
            _context.SetUseFlee(true);
        }

        if(StatesUsed.Contains(EnemyState.Attack))
        {
            // Must also use FocusIdle
            if (!States.ContainsKey(EnemyState.FocusIdle))
                States.Add(EnemyState.FocusIdle, new FocusIdle(_context, EnemyState.FocusIdle, _chaseStartRadius,
                _focusIdleRotationSpeed, _attackRange));

            States.Add(EnemyState.Attack, new AttackState(_context, EnemyState.Attack, _bulletPrefab, _shotFrequency, 
                _shotCount, _shotRotationSpeed, _damageLayer));

            _context.SetUseAttack(true);
        }

        if(StatesUsed.Contains(EnemyState.Cover))
        {
            States.Add(EnemyState.Cover, new CoverState(_context, EnemyState.Cover, _minHideTime, _maxHideTime, 
                _coverSearchRange, _runSpeed));
            _context.SetUseCover(true);
        }

        CurrentState = States[EnemyState.RandomIdle];
    }

    public void Shoot()
    {
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
