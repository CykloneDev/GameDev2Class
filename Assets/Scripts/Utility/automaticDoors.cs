using UnityEngine;

public class automaticDoors : MonoBehaviour
{
    [SerializeField] Animator anim;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || (other.CompareTag("Enemy")))
        {
            anim.SetBool("character_nearby", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || (other.CompareTag("Enemy")))
        {
            anim.SetBool("character_nearby", false);
        }
    }
}