using System;
using UnityEngine;

public class powerUps : MonoBehaviour
{
   public enum PowerUpType {givehealth, speedboost, damageup, increasejump, increasemaxhealth, gravity}

    public PowerUpType powerupType;


    [SerializeField] private float duration = 5f; // Set the duration of the powerUp (if one is possibly added)



    private void OnTriggerEnter(Collider other)
      {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.OnPowerUpCollected(powerupType);
            Destroy(gameObject);
            Debug.Log("works");
        }
      }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
