using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Rigidbody))]
public class EnemyMachine : StateMachine<EnemyMachine.EnemyState>
{
    public enum EnemyState
    {
        Idle,
        Waypoint,
        Chase,
        Flee,
        Cover
    }

    [SerializeField] private EnemyContext _context;
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    private Animator _animator;
    private PlayerDetector _playerDetector;
    [SerializeField] private float _minRandomWait, _maxRandomWait;

    [SerializeField] private List<EnemyState> StatesUsed;   
    [SerializeField] private Transform[] _waypoints;

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
        _context = new EnemyContext(transform, _agent, _animator, _rb, _playerDetector);
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
        States.Add(EnemyState.Idle, new IdleState(_context, EnemyState.Idle,
            _minRandomWait, _maxRandomWait));

        if (StatesUsed.Contains(EnemyState.Waypoint))
        {
            Debug.Log("Added Waypoint State to " + gameObject.name);
            _context.SetUseWaypoints(true);
            States.Add(EnemyState.Waypoint, new WaypointState(_context, EnemyMachine.EnemyState.Waypoint, 
                _waypoints)); 
        }

        CurrentState = States[EnemyState.Idle];
    }


    #endregion
}
