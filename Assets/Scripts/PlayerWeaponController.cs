using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    Camera _camera;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _shootDistance;
    [SerializeField] private LayerMask _damageLayer;
    [SerializeField] private GameObject _gunModel;
    [SerializeField] private GameObject muzzle;
    ParticleSystem muzzleFlash;
    private float _shootTimer;
    Ray _projectileRay;
    AudioSource _source;

    [SerializeField] List<WeaponStats> weaponList = new List<WeaponStats>();
    [SerializeField] int _weaponIndex;
    [SerializeField] int _maxWeaponEnergy;
    [SerializeField] float _rechargeRate;
    float _currentWeaponEnergy;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _camera = Camera.main;
        ChangeGun();
        _currentWeaponEnergy = _maxWeaponEnergy;
    }

    private void Update()
    {
        if (GameManager.instance.IsPaused) return;

        _projectileRay.origin = _shotPoint.position;
        _projectileRay.direction = _camera.transform.forward;
        _shotPoint.LookAt(_projectileRay.GetPoint(100));
        _shootTimer += Time.deltaTime;
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * _shootDistance);

        if (Input.GetButton("Fire1") && _shootTimer >= _shootRate)
        {
            Shoot();
        }

        SelectGun();


        if (_currentWeaponEnergy < _maxWeaponEnergy)
        {
            _currentWeaponEnergy += Time.deltaTime * (_maxWeaponEnergy * _rechargeRate);
            UpdatePlayerUI();

            if (_currentWeaponEnergy > _maxWeaponEnergy)
                _currentWeaponEnergy = _maxWeaponEnergy;
        }
    }

    void Shoot()
    {
        var currentWeapon = weaponList[_weaponIndex];
        if (_currentWeaponEnergy < currentWeapon.shotEnergy) return;
        _currentWeaponEnergy -= currentWeapon.shotEnergy;
        UpdatePlayerUI();
        _shootTimer = 0;

        var index = Random.Range(0, currentWeapon.shotSounds.Length);
        _source.PlayOneShot(currentWeapon.shotSounds[index]);
        muzzleFlash.Play();

        var prefab = currentWeapon.bulletPrefab;
        var damage = currentWeapon.damage;

        if (currentWeapon.useProjectile)
        {            
            var projectileSpeed = currentWeapon.shotSpeed;
            var bullet = Instantiate(prefab, _shotPoint.position, _shotPoint.rotation);
            bullet.layer = LayerMask.NameToLayer("Player Bullet");
            bullet.GetComponent<Rigidbody>().AddForce(projectileSpeed * _camera.transform.forward);
        }

        else
        {
            RaycastHit hit;

            if (Physics.Raycast(_shotPoint.transform.position, _camera.transform.forward, out hit,
                _shootDistance, _damageLayer))
            {
                IDamage target;
                hit.collider.TryGetComponent<IDamage>(out target);
                if (target != null)
                {
                    target.TakeDamage(damage);
                }

                var hitEffect = currentWeapon.hitPrefab;
                Instantiate(hitEffect, hit.point, Quaternion.Euler(hit.normal));
            }
        }
    }

    public void GetWeaponStats(WeaponStats gun)
    {
        weaponList.Add(gun);
        _weaponIndex = weaponList.Count - 1;
        ChangeGun();
    }

    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ++_weaponIndex;
            if (_weaponIndex > weaponList.Count - 1)
                _weaponIndex = 0;

            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            --_weaponIndex;
            if (_weaponIndex < 0)
            {
                _weaponIndex = weaponList.Count - 1;
            }

            ChangeGun();
        }
    }

    void ChangeGun()
    {
        if (weaponList.Count == 0) return;

        var gun = weaponList[_weaponIndex];
        _shootDistance = gun.shotDistance;
        _shootRate = gun.shotRate;

        if(muzzleFlash != null) Destroy(muzzleFlash);

        muzzleFlash = Instantiate(weaponList[_weaponIndex].muzzleFlash.gameObject, muzzle.transform).GetComponent<ParticleSystem>();

        _gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        _gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;
        GameManager.instance.currentWeaponName.text = weaponList[_weaponIndex].name;
    }
    public void UpdatePlayerUI()
    {
        GameManager.instance.playerWPBar.fillAmount = (float)_currentWeaponEnergy / _maxWeaponEnergy;
    }
}
