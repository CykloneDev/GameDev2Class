using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyContext
{
    public EnemyContext(EnemyMachine machine, Transform transform, NavMeshAgent agent, Animator animator, Rigidbody rb, 
        PlayerDetector playerDetector, float meleeTime)
    {
        _enemyMachine = machine;
        _transform = transform;
        _agent = agent;
        _rb = rb;   
        _animator = animator;
        _playerDetector = playerDetector;
        _meleeTime = meleeTime;
    }

    [SerializeField] private EnemyMachine _enemyMachine;
    [SerializeField] private Transform _transform;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private bool _damage;
    [SerializeField] private bool _dead;
    [SerializeField] private float _meleeTime;

    public EnemyMachine GetMachine() => _enemyMachine;
    public Transform GetTransform() => _transform;
    public NavMeshAgent GetAgent() => _agent;
    public Rigidbody GetRigidBody() => _rb;
    public Animator GetAnimator() => _animator;
    public PlayerDetector GetPlayerDetector() => _playerDetector;
    public bool UseWaypoints() => _enemyMachine.StatesUsed.Contains(EnemyMachine.EnemyState.Waypoint);
    public bool UseChase() => _enemyMachine.StatesUsed.Contains(EnemyMachine.EnemyState.Chase);
    public bool UseFlee() => _enemyMachine.StatesUsed.Contains(EnemyMachine.EnemyState.Flee);
    public bool UseAttack() => _enemyMachine.StatesUsed.Contains(EnemyMachine.EnemyState.Attack);
    public bool UseMelee() => _enemyMachine.StatesUsed.Contains(EnemyMachine.EnemyState.Melee);
    public bool UseCover() => _enemyMachine.StatesUsed.Contains(EnemyMachine.EnemyState.Cover);
    public bool GetDamage() => _damage;
    public void SetDamage(bool damage) { _damage = damage; }    
    public bool GetDead() => _dead;
    public void SetDead(bool dead) { _dead = dead; }
    public float GetMeleeTime() => _meleeTime;
}
