using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public GameObject model;
    public int damage;
    public int shotEnergy;
    public float shotDistance; // For raycast shots
    public float shotRate;
    public int shotSpeed;
    public GameObject bulletPrefab;
    public ParticleSystem hitPrefab;
    public ParticleSystem muzzleFlash;
    public AudioClip[] shotSounds;
    public bool useProjectile;
}
