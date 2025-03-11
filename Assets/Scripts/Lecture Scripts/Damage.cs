using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] DamageType _type;
    [SerializeField] Rigidbody _rb;
    [SerializeField] int _amount;
    [SerializeField] int _speed;
    [SerializeField] float _destroyTime;
    [SerializeField] float _damageFrequency;

    bool _isDamaging;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg == null) return;
        if (_type == DamageType.DamageOverTime) return;
        dmg.TakeDamage(_amount);

        if (_type != DamageType.Moving) return;
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {        
        if (other.isTrigger) return;

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg == null) return;

        if (_type != DamageType.DamageOverTime) return;
        if (_isDamaging) return;

        StartCoroutine(DamageOther(dmg));
    }

    IEnumerator DamageOther(IDamage d)
    {
        _isDamaging = true;

        d.TakeDamage(_amount);

        yield return new WaitForSeconds(_damageFrequency);

        _isDamaging = false;
    }

    public void InitBullet(int damageAmount, int speed, float destroyTime)
    {
        _amount = damageAmount;
        _speed = speed;
        _destroyTime = destroyTime;
        _type = DamageType.Moving;
        _rb.linearVelocity = transform.forward * _speed;
        Destroy(gameObject, _destroyTime);
    }
}
