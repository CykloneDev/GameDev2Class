using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public EnemyContext(Transform transform, NavMeshAgent agent, Rigidbody rb)
    {
        _transform = transform;
        _agent = agent;
        _rb = rb;   
    }

    private Transform _transform;
    private NavMeshAgent _agent;
    private Rigidbody _rb;
    private Animator _animator;

    public Transform GetTransform() => _transform;
    public NavMeshAgent GetAgent() => _agent;
    public Rigidbody GetRigidBody() => _rb;
    public Animator GetAnimator() => _animator;
}
