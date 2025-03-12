using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    //Unity Editable Variables for Player Movement
    [SerializeField] CharacterController controller; // Player's movement controller
    [Range(1, 10)][SerializeField] int health; // Player's Health 
    [Range(2, 10)][SerializeField] float speed; // Player's Base Speed
    [Range(2, 5)][SerializeField] float sprintMultiplier; // Player's Speed Multiplier when Running
    [Range(0.2f, 0.8f)][SerializeField] float crouchMultiplier; // Player's Speed Multiplier when Running
    [Range(5, 20)][SerializeField] float jumpSpeed; // Jump Velocity
    [Range(1, 3)][SerializeField] int jumpsMax; // Max Jumps
    [Range(15, 45)][SerializeField] int gravity; //Newtons Law 


    //Slide Variables
    [Range(1, 10)][SerializeField] float slideSpeed; // How fast the slide will propel you
    [Range(0.5f, 5f)][SerializeField] float slideMultiplier; //Player's Speed Multiplier when Sliding
    [Range(0.5f, 5f)][SerializeField] float slideDuration; //Duration of how long the slide will last
    [Range(0.5f, 10f)][SerializeField] float slideFriction; //How much friction there will be when sliding.
    float slideTimer; // Takes in the slideDuration as a reusable timer.
    float slideHeight = 0.8f; // The height when the player is sliding


    //Static Values and Timers
    int jumpCount; //a jump counter.
    int healthOriginal; //Default Health
    float normalHeight = 2f; // The height when the player spawns
    float crouchHeight = 1f; // The height when the player is crouching
    float tempSpeed; //Temp speed from coming out of the slide.


    //Bools
    bool isSliding = false;
    bool isCrouching = false;
    bool isSprinting = false;
    bool isJumping = false;


    //Vectors
    Vector3 moveDirection;
    Vector3 playerVelocity;
    Vector3 tempMoveDirection;


    //Animations
    [SerializeField] Animator anim;
    [SerializeField] int animTransitionSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        slideManager();
    }

    void movement()
    {
        if (controller.isGrounded) //Checks if grounded, if so then resets counter for multiple jumps
        {
            isJumping = false;
            jumpCount = 0;
        }


        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +   //Handles the input
                         (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDirection * speed * Time.deltaTime); //calls for the Controller to move.

        jump(); //Checks to see if Jump is being called.

        //Gravity
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime; //Adds gravity to the Y velocity.

        crouch();



    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            speed *= sprintMultiplier;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMultiplier;
            isSprinting = false;
        }
    }


    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpsMax)
        {
            isJumping = true;
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }
    }

    void crouch()
    {
        if (!isSliding && !isSprinting && !isJumping)
        {
            if (Input.GetButtonDown("Crouch") && controller.isGrounded)
            {
                isCrouching = true;
                speed *= crouchMultiplier;
                controller.height = crouchHeight;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                isCrouching = false;
                speed /= crouchMultiplier;
                controller.height = normalHeight;
            }
        }
    }

    void slideManager()
    {
        if (Input.GetButtonDown("Slide") && isSprinting && controller.isGrounded)
        {
            startSlide();
        }


        if(isSliding)
        {
            if (slideTimer <= 0 || moveDirection.magnitude <= 0.5f || Input.GetButtonUp("Slide") || Input.GetButtonDown("Sprint"))
            {
                endSlide();
            }
            else{
                sliding();
            }
        }
    }


    void startSlide()
    {
        //Timers and Bools
        isSliding = true;
        slideTimer = slideDuration;

        speed *= slideMultiplier;
        controller.height = slideHeight;
        moveDirection = transform.forward * slideSpeed;
    }

    void sliding()
    {
        slideTimer -= Time.deltaTime;

        if (slideTimer > 0)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, slideFriction * Time.deltaTime);
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    void endSlide()
    {
        isSliding = false;
        controller.height = normalHeight;
        speed /= slideMultiplier;
    }


}