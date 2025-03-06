using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Rigidbody))]
public class EnemyMachine : StateMachine<EnemyState>
{
    [SerializeField] private EnemyContext _context;
    private Rigidbody _rb;
    private NavMeshAgent _agent;
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private float _minRandomWait, _maxRandomWait;

    [SerializeField] private List<EnemyState> StatesUsed;   
    [SerializeField] private Transform[] _waypoints;

    [SerializeField] private bool _useWaypoints = false;

    #region Unity Methods
    void Start()
    {
        _context = new EnemyContext(transform, _agent, _rb, _playerDetector, _useWaypoints);
        InitializeStates();
    }

    void Update()
    {
        
    }
    #endregion

    #region EnemyMachine Methods
    void InitializeStates()
    {
        States = new Dictionary<EnemyState, BaseState<EnemyState>>();

        if(StatesUsed.Contains(EnemyState.Idle))
            States.Add(EnemyState.Idle, new IdleState(_context, EnemyState.Idle, 
                _minRandomWait, _maxRandomWait));

        if (StatesUsed.Contains(EnemyState.Waypoint))
        { 
            _useWaypoints = true;
            States.Add(EnemyState.Waypoint, new WaypointState(_context, EnemyState.Waypoint, _waypoints)); 
        }
    }
    #endregion
}
