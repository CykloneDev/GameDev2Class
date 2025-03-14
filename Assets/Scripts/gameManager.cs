using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public playerController playerController;
    Transform playerTransform;

    private bool _isPaused;
    private bool _win;
    private bool _loss;
    private float _defaultTimeScale;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject lossMenu;
    public GameObject playerDamageScreen;
    public Image playerHPBar;
    public TMP_Text goalCountText;

    [SerializeField] int goalCount;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<playerController>();
        playerTransform = playerController.transform;
        _defaultTimeScale = Time.timeScale;
        _isPaused = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")
            && !_loss
            && !_win)
        {
            if (activeMenu == null)
            {
                PauseState();
                activeMenu = pauseMenu;
                activeMenu.SetActive(true);
            }

            else
            {
                UnpauseState();
            }
        }
    }

    public Transform GetPlayerTransform() => playerTransform;

    public void PauseState()
    {
        _isPaused = !_isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnpauseState()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _defaultTimeScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void Lose()
    {
        PauseState();
        activeMenu = lossMenu;
        activeMenu.SetActive(true);
    }

    public void UpdateGameGoal(int amount)
    {
        goalCount += amount;
        goalCountText.text = goalCount.ToString("F0");

        // Checking here so we don't poll each frame in Update
        if (goalCount <= 0)
        {
            if (_win) return;
            if (activeMenu != null) return;

            PauseState();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            activeMenu = winMenu;
            activeMenu.SetActive(true);
            _win = true;
            _isPaused = true;
        }
    }

   public void OnPowerUpCollected(powerUps.PowerUpType type)  
    {
        if(playerTransform.TryGetComponent(out playerController player))
        {
            switch (type)
            {
                case powerUps.PowerUpType.givehealth:
                    player.Health += player.giveHealth;
                    Debug.Log("Health has been added to the player");
                break;
                case powerUps.PowerUpType.speedboost:
                    player.Speed += player.maxSpeed;
                    Debug.Log("Speedboost has been added to the player");
                break;
                case powerUps.PowerUpType.damageup:
                    //Logic not implemented
                    Debug.Log("Damageboost has been added to the player");
                break;
                case powerUps.PowerUpType.increasejump:
                    player.Jumps += player.setJumpsMax;
                    Debug.Log("Increasejumps has been added to the player");
                break;
                case powerUps.PowerUpType.gravity:
                player.Gravity = player.maxGravity; 
                    Debug.Log("Gravity has been changed");
                break;
                case powerUps.PowerUpType.increasemaxhealth:
                    player.MaxHealth += player.newMaxHealth;
                    Debug.Log("MaxHealth has been increased!");
                break;
                default:
                    Debug.LogWarning("Unknown power-up type collected");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Player component not found on playerTransform!");
        }

   }
}
        
    
