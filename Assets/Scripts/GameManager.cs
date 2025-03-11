using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController playerController;
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
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
}
