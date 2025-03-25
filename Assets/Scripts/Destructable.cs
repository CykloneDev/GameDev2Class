using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destoryItem;


    [SerializeField] int hitPoints;

    public GameObject destroyItem;
    int value;

    public void Break()
    {
        hitPoints = value;

        if (hitPoints >= 0)
        {
            Instantiate(destoryItem, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    }

    
