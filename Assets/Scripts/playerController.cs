using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    //Variables for Player Movement
    [Range(2, 15)][SerializeField] int speed; //General speed of player.
    [Range(2, 5)][SerializeField] int sprintMult; //Multiplier for the speed while sprinting.
    [Range(5, 20)][SerializeField] int jumpSpeed; //
    [Range(5, 20)][SerializeField] int slideSpeed;
    [Range(1, 3)][SerializeField] int jumpsMax; // Sets the max amount of time the player can jump.
    [Range(15, 45)][SerializeField] int gravity; // Sets the gravity while the player is in the air.

    //Counters and Cooldowns
    int jumpCount;
    float slideCooldown; // Once the character slides, adds a cooldown until they can slide again (to not cause any jagged camera issues)
    float crouchCooldown; // Once the character crouches, adds a cooldown until they can crouch again (to not cause any jagged camera issues)

    //Vectors
    Vector3 moveDirection;
    Vector3 playerVelocity;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/* Future Function Ideas

 * Sliding Feature
 * Wall Jumping
 * BHops?

General Movement Function*/
void movement()
{

}

//Jumping Function

void jump()
{
    //
}

//Sprinting Function
void sprint()
{

}

//Crouch Function
void crouch()
{

}

//Sliding Function
void slide()
{

}