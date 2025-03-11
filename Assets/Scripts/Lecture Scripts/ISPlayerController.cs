using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ISPlayerController : MonoBehaviour, IDamage
{
    [SerializeField] private CharacterController _character;
    Camera _camera;
    [SerializeField, Range(0, 100)] private int _hitPoints;
    private int _maxHitPoints;
    [SerializeField, Range(0, 100)] private int _speed;
    [SerializeField, Range(3, 6)] private int _sprintMod;
    [SerializeField, Range(0, 100)] private int _jumpSpeed;
    [SerializeField, Range(0, 100)] private int _jumpMax;
    [SerializeField, Range(0, 100)] private int _gravity;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _shootDistance;
    [SerializeField] private int _shootDamage;
    [SerializeField] private LayerMask _damageLayer;

    [SerializeField] private InputAction _moveAction;
    [SerializeField] private InputAction _jumpAction;
    [SerializeField] private InputAction _shootAction;

    private Vector3 _moveDirection;
    private Vector3 _playerVelocity;
    private int _jumpCount;
    private float _shootTimer;
    private bool _shooting;
    private bool _jumping;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        _maxHitPoints = _hitPoints;   
        UpdatePlayerUI();
    }

    void Enable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _shootAction.Enable();

        _moveAction.performed += OnMove;
        _jumpAction.performed += OnJump;
        _shootAction.performed += OnShoot;
    }

    void Disable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _shootAction.Disable();

        _moveAction.performed -= OnMove;
        _jumpAction.performed -= OnJump;
        _shootAction.performed -= OnShoot;
    }


    private void Update()
    {
        _shootTimer += Time.deltaTime;
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * _shootDistance);

        if (_shooting && _shootTimer >= _shootRate)
        {
            Shoot();
        }

        Movement();
        Sprint();
    }

    private void Movement()
    {
        if (_character.isGrounded) 
        { 
            _jumpCount = 0; 
            _playerVelocity = Vector3.zero;
        }

        _moveDirection = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;

        _character.Move(_moveDirection * _speed * Time.deltaTime);

        Jump();

        _character.Move(_playerVelocity * Time.deltaTime);

        _playerVelocity.y -= _gravity * Time.deltaTime;
    }

    void Jump()
    {
        if(_jumping && _jumpCount < _jumpMax)
        {
            ++_jumpCount;
            _playerVelocity.y = _jumpSpeed;
            _jumping = false;
        }
    }

    void Sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            _speed *= _sprintMod;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            _speed /= _sprintMod;
        }
    }

    void Shoot()
    {
        _shootTimer = 0;
        RaycastHit hit;

        if(Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit,
            _shootDistance, _damageLayer))
        {
            IDamage damage; 
            hit.collider.TryGetComponent<IDamage>(out damage);
            if(damage != null)
            {
                damage.TakeDamage(_shootDamage);
            }
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed) _jumping = true;
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        _shooting = context.ReadValueAsButton();
    }

    public void TakeDamage(int amount)
    {
        _hitPoints -= amount;
        StartCoroutine(FlashDamageScreen());

        if(_hitPoints <= 0)
        {
            GameManager.instance.Lose();
        }

        UpdatePlayerUI();
    }

    IEnumerator FlashDamageScreen()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        GameManager.instance.playerDamageScreen.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount =(float)_hitPoints / _maxHitPoints;
    }
}
