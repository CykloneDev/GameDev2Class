using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller; // Player's movement controller
    //Unity Editable Variables for Player Movement

    //Health Variables
    [SerializeField] int health, maxHealth;
    //Speed Variables
    [Range(2, 10)][SerializeField] float speed; // Player's Base Speed
    [Range(2, 5)][SerializeField] float sprintMultiplier; // Player's Speed Multiplier when Running
    [Range(2, 10)][SerializeField] public float maxSpeed; //Set how much you want the players maxSpeed to increase through speedboost powerups
    [Range(0.2f, 0.8f)][SerializeField] float crouchMultiplier; // Player's Speed Multiplier when Running
    [Range(5, 20)][SerializeField] float jumpSpeed; // Jump Velocity


    [Range(1, 3)][SerializeField] int jumpsMax; // Max Jumps
    [Range(15, 45)][SerializeField] int gravity; //Newtons Law 
    [Range(1, 100)][SerializeField] public int maxGravity; //Set how much gravity you want the player to have (1 through 100 to give m

    //Slide Variables
    [Range(1, 10)][SerializeField] float slideSpeed; // How fast the slide will propel you
    [Range(0.5f, 5f)][SerializeField] float slideMultiplier; //Player's Speed Multiplier when Sliding
    [Range(0.5f, 5f)][SerializeField] float slideDuration; //Duration of how long the slide will last
    [Range(0.5f, 10f)][SerializeField] float slideFriction; //How much friction there will be when sliding.
    float slideTimer; // Takes in the slideDuration as a reusable timer.
    float slideHeight = 1f; // The height when the player is sliding


    //Static Values and Timers
    int jumpCount; //a jump counter.
    float speedOriginal;
    float normalHeight = 2f; // The height when the player spawns
    float crouchHeight = 1f; // The height when the player is crouching\
    Vector3 normalCenter = new Vector3(0.0f, 1.1f, 0.0f);
    Vector3 crouchedCenter = new Vector3(0.0f, 0.5f, 0.0f);
    Vector3 slideCenter = new Vector3(0.0f, 0.5f, 0.0f);
    private float shootTimer;


    //Bools
    bool isSliding = false;
    bool isCrouching = false;
    bool isSprinting = false;
    bool isJumping = false;
    bool isWalking = false;




    //Vectors
    Vector3 moveDirection;
    Vector3 playerVelocity;
    Vector3 tempMoveDirection;


    [SerializeField] Animator anim;
    AudioSource _source;
    [SerializeField] private AudioClip _walkClip, _jumpClip, _slideClip, _crouchClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _source = GetComponent<AudioSource>();
        health = maxHealth;
        speedOriginal = speed; // Setting up a temp speed var.
        UpdatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        movement();
        sprint();
        slideManager();
    }

    void movement()
    {
        if (controller.isGrounded) //Checks if grounded, if so then resets counter for multiple jumps
        {
            anim.SetBool("isJumping", false);
            isJumping = false;
            jumpCount = 0;
        }


        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +   //Handles the input
                         (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDirection * speed * Time.deltaTime); //calls for the Controller to move.

        jump(); //Checks to see if Jump is being called.
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime; //Adds gravity to the Y velocity.

        crouch();


        float moveDirectionSpeed = moveDirection.magnitude;//Calculates speed to blend anims
        anim.SetFloat("MoveSpeed", moveDirectionSpeed);

    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && !isSliding)
        {
            anim.SetBool("isSprinting", true);
            isSprinting = true;
            speed *= sprintMultiplier;
        }
        else if (Input.GetButtonUp("Sprint") && !isSliding)
        {
            anim.SetBool("isSprinting", false);
            speed = speedOriginal;
            isSprinting = false;
        }
    }


    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpsMax)
        {
            anim.SetBool("isJumping", true);
            isJumping = true;
            jumpCount++;
            playerVelocity.y = jumpSpeed;
            
            if(_jumpClip != null) _source.PlayOneShot(_jumpClip);
        }
    }

    void crouch()
    {
        if (!isSliding && !isSprinting && !isJumping)
        {
            if (Input.GetButtonDown("Crouch") && controller.isGrounded)
            {
                anim.SetBool("isCrouching", true);
                isCrouching = true;
                speed *= crouchMultiplier;
                controller.height = crouchHeight;
                controller.center = crouchedCenter;
                if (_crouchClip != null) _source.PlayOneShot(_crouchClip);
            }
            else if (Input.GetButtonUp("Crouch") || Input.GetButtonDown("Jump") || Input.GetButtonDown("Sprint") || Input.GetButtonDown("Slide"))
            {
                anim.SetBool("isCrouching", false);
                isCrouching = false;
                speed = speedOriginal;
                controller.height = normalHeight;
                controller.center = normalCenter;
                if (_crouchClip != null) _source.PlayOneShot(_crouchClip);
            }
        }
    }

    void slideManager()
    {
        if (Input.GetButtonDown("Slide") && isSprinting && controller.isGrounded)
        {
            startSlide();
            if (_slideClip != null) _source.PlayOneShot(_slideClip);
        }


        if (isSliding)
        {
            if (slideTimer <= 0 || moveDirection.magnitude <= 0.5f || Input.GetButtonUp("Slide")
               || Input.GetButtonDown("Sprint") || Input.GetButtonDown("Crouch") || Input.GetButtonDown("Jump"))
            {
                endSlide();
            }
            else
            {
                sliding();
            }
        }
    }


    void startSlide()
    {
        //Timers and Bools
        anim.SetBool("isSliding", true);
        anim.SetBool("isSprinting", false);
        isSliding = true;
        slideTimer = slideDuration;

        speed *= slideMultiplier;
        controller.height = slideHeight;
        controller.center = slideCenter;
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
        anim.SetBool("isSliding", false);
        isSliding = false;
        controller.height = normalHeight;
        controller.center = normalCenter;
        speed = speedOriginal;
    }

    public void TakeDamage(int value)
    {
        health -= value;
        StartCoroutine(FlashDamageScreen());

        if (health <= 0)
        {
            GameManager.instance.Lose();
        }

        UpdatePlayerUI();
    }

    public void HealDamage(int value)
    {
        health += value;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void AddSpeed(float value)
    {
        speed += value;
    }

    public void RemoveSpeed(float value)
    {
        speed -= value;
    }

    public void SetJumps(int value)
    {
        jumpsMax = value;
    }

    public void SetGravity(int value)
    {
        gravity = value;
    }

    public void SetMaxHP(int value)
    {
        maxHealth = value;
    }

    IEnumerator FlashDamageScreen()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        GameManager.instance.playerDamageScreen.SetActive(false);
    }

    IEnumerator FlashHealScreen()
    {
        GameManager.instance.playerHealScreen.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        GameManager.instance.playerHealScreen.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)health / maxHealth;
    }


    void PlayStep()
    {
        if (_walkClip != null) _source.PlayOneShot(_walkClip);
    }

}