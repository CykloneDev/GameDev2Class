using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 4f;

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
            Debug.Log("Kaboom");
        }
}