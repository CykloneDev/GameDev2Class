using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    Camera _camera;
    [SerializeField] private bool _useProjectile;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _projectileSpeed;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _shootDistance;
    [SerializeField] private int _shootDamage;
    [SerializeField] private LayerMask _damageLayer;
    private float _shootTimer;
    Ray _projectileRay;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        _projectileRay.origin = _shotPoint.position;
        _projectileRay.direction = _camera.transform.forward;
        _shotPoint.LookAt(_projectileRay.GetPoint(100));
        _shootTimer += Time.deltaTime;
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * _shootDistance);

        if (Input.GetButton("Fire1") && _shootTimer >= _shootRate)
        {
            Shoot();
        }
    }

    // Terrence Edit

    void Shoot()
    {
        _shootTimer = 0;
        if (_useProjectile)
        {
            var bullet = Instantiate(_bulletPrefab, _shotPoint.position, _shotPoint.rotation);
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
}
