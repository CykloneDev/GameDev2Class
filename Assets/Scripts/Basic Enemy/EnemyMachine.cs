using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Rigidbody))]
public class EnemyMachine : StateMachine<EnemyMachine.EnemyState>
{
    public enum EnemyState
    {
        RandomIdle,
        FocusIdle,
        Waypoint,
        Chase,
        Flee,
        Cover,
        Attack
    }

    [SerializeField] private EnemyContext _context;
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    private Animator _animator;
    private PlayerDetector _playerDetector;
    [SerializeField] private float _minRandomWait, _maxRandomWait;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _chaseStopRadius;
    [SerializeField] private float _chaseStartRadius;
    [SerializeField] private float _chaseRefreshTime;
    [SerializeField] private float _focusIdleRotationSpeed;
    [SerializeField] private float _fleeRadius;

    [SerializeField] private List<EnemyState> StatesUsed;   
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shotPoint;
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

        Debug.Log("Added Idle State to " + gameObject.name);
        States.Add(EnemyState.RandomIdle, new IdleState(_context, EnemyState.RandomIdle,
            _minRandomWait, _maxRandomWait));

        if (StatesUsed.Contains(EnemyState.Waypoint))
        {
            Debug.Log("Added Waypoint State to " + gameObject.name);
            _context.SetUseWaypoints(true);
            States.Add(EnemyState.Waypoint, new WaypointState(_context, EnemyMachine.EnemyState.Waypoint, 
                _waypoints, _walkSpeed)); 
        }

        if(StatesUsed.Contains(EnemyState.Chase))
        {
            // Must also use FocusIdle
            if(!States.ContainsKey(EnemyState.FocusIdle))
                States.Add(EnemyState.FocusIdle, new FocusIdle(_context, EnemyState.FocusIdle, _chaseStartRadius,
                _focusIdleRotationSpeed));

            States.Add(EnemyState.Chase, new ChaseState(_context, EnemyState.Chase, _runSpeed,
                _chaseStopRadius, _chaseRefreshTime));
            _context.SetUseChase(true);
        }

        if(StatesUsed.Contains(EnemyState.Flee))
        {
            // Must also use FocusIdle
            if (!States.ContainsKey(EnemyState.FocusIdle))
                States.Add(EnemyState.FocusIdle, new FocusIdle(_context, EnemyState.FocusIdle, _chaseStartRadius,
                _focusIdleRotationSpeed));

            States.Add(EnemyState.Flee, new FleeState(_context, EnemyState.Flee, _runSpeed, _fleeRadius));
            _context.SetUseFlee(true);
        }

        if(StatesUsed.Contains(EnemyState.Attack))
        {
            // Must also use FocusIdle
            if (!States.ContainsKey(EnemyState.FocusIdle))
                States.Add(EnemyState.FocusIdle, new FocusIdle(_context, EnemyState.FocusIdle, _chaseStartRadius,
                _focusIdleRotationSpeed));

            States.Add(EnemyState.Attack, new AttackState(_context, EnemyState.Attack, _bulletPrefab, _shotFrequency, 
                _shotCount, _shotRotationSpeed, _damageLayer));

            _context.SetUseAttack(true);
        }

        CurrentState = States[EnemyState.RandomIdle];
    }

    public void Shoot()
    {
        var bullet = Instantiate(_bulletPrefab, _shotPoint.position, _context.GetTransform().rotation);
        bullet.layer = LayerMask.NameToLayer(_damageLayer);
        bullet.GetComponent<Damage>().InitBullet(_shotDamageAmount, _shotSpeed, 3f);
    }

    #endregion
}
