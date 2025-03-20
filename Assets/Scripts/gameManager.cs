using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    public GameObject playerHealScreen;
    public Image playerHPBar;
    public TMP_Text goalCountText;

    public List<GameObject> waypointList = new List<GameObject>();

    [SerializeField] int goalCount;
    [SerializeField] int enemiesDefeated;

    [SerializeField] int healAmount;
    [SerializeField] int speedIncrease;
    [SerializeField] int jumpIncrease;
    [SerializeField] int maxHPIncrease;
    [SerializeField] int gravityIncrease;

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

        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (var waypoint in waypoints)
        {
            waypointList.Add(waypoint);
        }
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
        _isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; 
    }

    public void UnpauseState()
    {
        _isPaused = false;
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

    public void OnEnemyDefeated()
    {
        ++enemiesDefeated;
    }

    public int EnemiesDefeated() => enemiesDefeated;

    public void ResetDefeatedCount() { enemiesDefeated = 0; }

    public void OnPowerUpCollected(powerUps.PowerUpType type)
    {
        if (playerTransform.TryGetComponent(out playerController player))
        {
            switch (type)
            {
                case powerUps.PowerUpType.givehealth:
                    player.HealDamage(healAmount);
                    player.UpdatePlayerUI();
                    Debug.Log("Health has been added to the player");
                    break;
                case powerUps.PowerUpType.speedboost:
                    player.AddSpeed(speedIncrease);
                    Debug.Log("Speedboost has been added to the player");
                    break;
                case powerUps.PowerUpType.damageup:
                    //Logic not implemented
                    Debug.Log("Damageboost has been added to the player");
                    break;
                case powerUps.PowerUpType.increasejump:
                    player.SetJumps(jumpIncrease);
                    Debug.Log("Increasejumps has been added to the player");
                    break;
                case powerUps.PowerUpType.gravity:
                    player.SetGravity(gravityIncrease);
                    Debug.Log("Gravity has been changed");
                    break;
                case powerUps.PowerUpType.increasemaxhealth:
                    player.SetMaxHP(maxHPIncrease);
                    player.UpdatePlayerUI();
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
