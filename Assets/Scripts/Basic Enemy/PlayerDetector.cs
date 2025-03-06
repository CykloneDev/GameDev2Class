using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private Collider _sphereCollider;
    [SerializeField] private Transform _player;
    [SerializeField] private float _forgetTime; // Used to determine when a target is forgotten
    [SerializeField] private bool _playerDetected;
    [SerializeField] private bool _inRange;

    private float _currentForgetTime;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _sphereCollider.enabled = false;
        _playerDetected = false;
        _inRange = false;
        _currentForgetTime = 0;
    }

    private void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if(!_playerDetected)
        {
            if(other.CompareTag("Player"))
            {
                _player = other.transform;
                _sphereCollider.enabled = true;
                _playerDetected = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(_playerDetected)
        {
            if (other.CompareTag("Player"))
            {
                _currentForgetTime = 0;
                _inRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(_inRange)
        {
            if (other.CompareTag("Player"))
                _inRange = false;
        }
    }

    public bool PlayerDetected() => _playerDetected;
}
