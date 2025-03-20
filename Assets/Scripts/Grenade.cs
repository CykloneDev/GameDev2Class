using UnityEngine;

public class Grenade : MonoBehaviour
{

    [SerializeField] int _amount;

    public float delay;
    public float radius;
    public float force;

    public GameObject explodeEffect;

    float countDown;
    bool explosion = false;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0f && !explosion)
        {
            Explode();
            explosion = true;
        }
    }

    void Explode()
    {

        Instantiate(explodeEffect, transform.position, transform.rotation);   //Shows explosion effect

        //Blows up near by objects
        Collider[] collider = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearObject in collider)
        {
            Rigidbody rb = nearObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);//force for grenade explosion
            }

            IDamage damage =  nearObject.GetComponent<IDamage>();
            if (damage != null)
            {
                
                damage.TakeDamage(_amount);
            }
            Destroy(gameObject); //Removes the grenade after explosion
        }

    }
}