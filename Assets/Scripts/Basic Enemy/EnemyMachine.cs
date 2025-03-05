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

    #region Unity Methods
    void Start()
    {
        _context = new EnemyContext(transform, _agent, _rb);
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

        States.Add(EnemyState.Idle, new IdleState(_context, EnemyState.Idle));
    }
    #endregion
}
