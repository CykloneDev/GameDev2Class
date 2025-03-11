using UnityEngine;

public class AttackState : EnemyBaseState
{
    public AttackState(EnemyContext context, EnemyMachine.EnemyState key, GameObject bulletPrefab, 
        float shotFrequency, int shotSpeed, int shotCount, int shotDamage, LayerMask damageLayer) : base(context, key)
    {
        _bulletPrefab = bulletPrefab;
        _shotFrequency = shotFrequency;
        _shotSpeed = shotSpeed;
        _shotCount = shotCount;
        _shotDamage = shotDamage;
        _damageLayer = damageLayer;
    }

    private GameObject _bulletPrefab;
    private float _shotFrequency;
    private int _shotSpeed;
    private int _shotCount;
    private int _shotDamage;
    private LayerMask _damageLayer;
}
