using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Transform playerTransform;


    private void Awake()
    {
        // There can be only one
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Transform GetPlayerTransform() => playerTransform;
}
