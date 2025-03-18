using UnityEngine;

public class Grenade : MonoBehaviour
{

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
        foreach (Collider collider2 in collider)
        {
            Rigidbody rb = collider2.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);//force for grenade explosion
            }


            Destroy(gameObject); //Removes the grenade after explosion
        }

    }
}