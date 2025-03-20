using System;
using UnityEngine;

public class Throwing : MonoBehaviour
{

    public float throwObject;

    private GameObject Grenade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown("Throw"))
        {
            throwGrenade();
        } 
    }

    void throwGrenade()
    {
        GameObject grenade = Instantiate(Grenade, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwObject);
    }

    
}
