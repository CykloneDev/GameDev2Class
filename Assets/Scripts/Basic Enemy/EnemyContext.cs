using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public EnemyContext(Transform transform, NavMeshAgent agent, Rigidbody rb, PlayerDetector playerDetector,
        bool useWaypoints)
    {
        _transform = transform;
        _agent = agent;
        _rb = rb;   
        _playerDetector = playerDetector;
        _useWaypoints = useWaypoints;
    }

    private Transform _transform;
    private NavMeshAgent _agent;
    private Rigidbody _rb;
    private Animator _animator;
    private PlayerDetector _playerDetector;
    private bool _useWaypoints;

    public Transform GetTransform() => _transform;
    public NavMeshAgent GetAgent() => _agent;
    public Rigidbody GetRigidBody() => _rb;
    public Animator GetAnimator() => _animator;
    public PlayerDetector GetPlayerDetector() => _playerDetector;
    public bool UseWaypoints() => _useWaypoints;
}
