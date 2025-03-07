using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyContext
{
    public EnemyContext(Transform transform, NavMeshAgent agent, Animator animator, Rigidbody rb, 
        PlayerDetector playerDetector)
    {
        _transform = transform;
        _agent = agent;
        _rb = rb;   
        _animator = animator;
        _playerDetector = playerDetector;
    }

    [SerializeField] private Transform _transform;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private bool _useWaypoints = false;
    [SerializeField] private bool _useChase = false;
    [SerializeField] private bool _useFlee = false;

    public Transform GetTransform() => _transform;
    public NavMeshAgent GetAgent() => _agent;
    public Rigidbody GetRigidBody() => _rb;
    public Animator GetAnimator() => _animator;
    public PlayerDetector GetPlayerDetector() => _playerDetector;
    public bool UseWaypoints() => _useWaypoints;
    public void SetUseWaypoints(bool useWaypoints) { _useWaypoints = useWaypoints; }
    public bool UseChase() => _useChase;
    public void SetUseChase(bool useChase) { _useChase = useChase; }
    public bool UseFlee() => _useFlee;
    public void SetUseFlee(bool useFlee) { _useFlee = useFlee; }
}
