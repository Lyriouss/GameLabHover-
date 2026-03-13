using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Menus")]
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject gameOverMenu;
    [SerializeField] public GameObject startGame;

    [Header("Power Up management")]
    public int powerUpA_quantity;
    public int powerUpS_quantity;
    public int powerUpD_quantity;
    [SerializeField] public Button powerUpA, powerUpS, powerUpD;
    [SerializeField] public TMP_Text powerUpA_TXT, powerUpS_TXT, powerUpD_TXT;
    [SerializeField] public Image powerUpA_fillBar, powerUpS_fillBar, powerUpD_fillBar, shield_fillBar, haste_fillBar, slow_fillBar;
    [SerializeField] public GameObject powerUpD_Screen;

    [Header("Score management")]
    [SerializeField] public TMP_Text score;
    [SerializeField] public Image flagScoreBlu;
    [SerializeField] public Image flagScoreRed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        startGame.SetActive(true);
    }

    private void Start()
    {
        powerUpA_quantity = 0;
        powerUpS_quantity = 0;
        powerUpD_quantity = 0;
        powerUpA_fillBar.fillAmount = 0;
        powerUpS_fillBar.fillAmount = 0;
        powerUpD_fillBar.fillAmount = 0;
        powerUpD_Screen.SetActive(false);
        shield_fillBar.fillAmount = 0;
        haste_fillBar.fillAmount = 0;
        slow_fillBar.fillAmount = 0;
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            startGame.SetActive(false);
        }

        flagScoreBlu.fillAmount = GameManager.Instance.capturedBlueFlags / 3;
        flagScoreRed.fillAmount = GameManager.Instance.capturedRedFlags / 3;
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void PowerUpA_Interaction()
    {
        if (powerUpA_quantity > 0)
        {
            bool jumped = PowerUpsManager.Instance.Jump();

            if (jumped)
            {
                powerUpA_quantity -= 1;
                powerUpA_TXT.text = powerUpA_quantity.ToString();
            }
        }
    }

    public void PowerUpS_Interaction()
    {
        if (powerUpS_quantity > 0)
        {
            PowerUpsManager.Instance.TempWall();

            powerUpS_quantity -= 1;
            powerUpS_TXT.text = powerUpS_quantity.ToString();
        }
    }

    public void PowerUpD_Interaction()
    {
        if (powerUpD_quantity > 0)
        {
            PowerUpsManager.Instance.Invisibilty();

            powerUpD_quantity -= 1;
            powerUpD_TXT.text = powerUpD_quantity.ToString();
        }
    }
}
