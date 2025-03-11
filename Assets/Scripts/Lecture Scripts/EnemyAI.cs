using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _followRange;
    [SerializeField] private Renderer _model;
    [SerializeField] private int _HitPoints;
    [SerializeField] private float _shootRate;
    [SerializeField] private int _faceTargetSpeed;
    [SerializeField] private float _animationSpeed;
    private float _shotTimer;
    [SerializeField] private Transform _shootPosition;
    [SerializeField] private GameObject _bulletPrefab;

    private bool _playerInRange;
    Vector3 _playerDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        _shotTimer += Time.deltaTime;
        if (_shotTimer > _shootRate) Shoot();

        SetAnimLocomotion();

        if(_playerInRange)
        {
            var playerPos = GameManager.instance.playerController.transform.position; 

            _agent.SetDestination(playerPos);

            _playerDirection = playerPos - transform.position;

            if(_agent.remainingDistance <= _agent.stoppingDistance)
            {
                FaceTarget();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }

    void FaceTarget()
    {
        var rot = Quaternion.LookRotation(new Vector3(_playerDirection.x, transform.position.y, _playerDirection.z));    
        transform.rotation = Quaternion.Lerp(transform.rotation, rot,
            Time.deltaTime * _faceTargetSpeed);
    }

    void SetAnimLocomotion()
    {
        var agentSpeed = _agent.velocity.normalized.magnitude;
        var animSpeedCurrent = _animator.GetFloat("speed");
        _animator.SetFloat("speed", Mathf.Lerp(animSpeedCurrent, agentSpeed, Time.deltaTime * _animationSpeed));
    }

    public void TakeDamage(int damage)
    {
        _HitPoints -= damage;
        StartCoroutine("FlashRed");
        var playerPos = GameManager.instance.playerController.transform.position;
        _agent.SetDestination(playerPos);

        if (_HitPoints <= 0)
        {
            GameManager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        var color = _model.material.color;
        _model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _model.material.color = color;
        yield return null;
    }

    void Shoot()
    {
        _shotTimer = 0;
        Instantiate(_bulletPrefab, _shootPosition.position, transform.rotation);
    }
}
