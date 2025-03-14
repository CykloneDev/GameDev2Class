using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
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
    [SerializeField] private bool _useProjectile;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _projectileSpeed;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _shootDistance;
    [SerializeField] private int _shootDamage;
    [SerializeField] private LayerMask _damageLayer;
    Ray _projectileRay;

    private Vector3 _moveDirection;
    private Vector3 _playerVelocity;
    private int _jumpCount;
    private float _shootTimer;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        _maxHitPoints = _hitPoints;   
        UpdatePlayerUI();
    }

    // Terrence Edit
    void FixedUpdate()
    {
        _projectileRay.origin = _shotPoint.position;
        _projectileRay.direction = _shotPoint.forward;
    }
    // Terrence Edit

    private void Update()
    {
        _shootTimer += Time.deltaTime;
        Debug.DrawRay(_shotPoint.position, _shotPoint.forward * _shootDistance);

        if (Input.GetButton("Fire1") && _shootTimer >= _shootRate)
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
        if(Input.GetButtonDown("Jump") && _jumpCount < _jumpMax)
        {
            ++_jumpCount;
            _playerVelocity.y = _jumpSpeed;
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
        if(_useProjectile)
        {
            var bullet = Instantiate(_bulletPrefab, _shotPoint.position, _shotPoint.rotation);
            bullet.transform.LookAt(_projectileRay.GetPoint(100), Vector3.up);
            bullet.layer = LayerMask.NameToLayer("Player Bullet");
            bullet.GetComponent<Damage>().InitBullet(_shootDamage, _projectileSpeed, 3f);
        }

        else
        {
            RaycastHit hit;

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit,
                _shootDistance, _damageLayer))
            {
                IDamage damage;
                hit.collider.TryGetComponent<IDamage>(out damage);
                if (damage != null)
                {
                    damage.TakeDamage(_shootDamage);
                }
            }
        }        
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
