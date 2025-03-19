using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _forgetTime; // Used to determine when a target is forgotten
    [SerializeField] private bool _playerDetected;
    [SerializeField] private bool _inRange;
    [SerializeField] private float _fov;

    [SerializeField] private float angleToPlayer;
    [SerializeField] private Vector3 playerDir;
    public LayerMask lineOfSightMask;

    private float _currentForgetTime;

    private void Start()
    {
        _player = GameManager.instance.GetPlayerTransform();
        _playerDetected = false;
        _inRange = false;
        _currentForgetTime = 0;
    }

    private void Update()
    {
        if(_inRange)
        {
            playerDir = _player.transform.position - transform.position;
            angleToPlayer = Vector3.Angle(transform.forward, playerDir);
            RaycastHit hit;
            Physics.Raycast(transform.position, playerDir, out hit, float.PositiveInfinity, lineOfSightMask);
            if(angleToPlayer <= _fov
                && hit.collider.CompareTag("Player"))
            {
                _playerDetected = true;
            }
            else
            {
                _playerDetected = false;
            }
        }

        if (_playerDetected && !_inRange)
        {
            _currentForgetTime += Time.deltaTime;
            if(_currentForgetTime >= _forgetTime)
            {
                // Forget
                _playerDetected = false;
                _inRange = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
        }
    }

    public bool PlayerDetected() => _playerDetected;

    public void SeePlayer()
    {
        _playerDetected = true;
    }

    void OnDrawGizmosSelected()
    {
        float rayRange = 10.0f;
        float halfFOV = _fov / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }
}
