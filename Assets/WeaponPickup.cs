using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponStats weaponStats;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerWeaponController>().GetWeaponStats(weaponStats);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        var rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 10f, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
    }
}
