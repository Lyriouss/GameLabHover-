using System;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{
    Running,
    Paused,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] KeyCode StartButton = KeyCode.F2;
    [SerializeField] KeyCode PauseButton = KeyCode.F3;

    KeyCode[] easterEggCode = { KeyCode.L, KeyCode.U, KeyCode.C, KeyCode.A };
    int index = 0;

    [SerializeField] AudioSource blueFlagCollected;
    [SerializeField] AudioSource redFlagCollected;
    [SerializeField] AudioSource blueFlagStolen;
    [SerializeField] AudioSource redFlagStolen;

    public float capturedBlueFlags;
    public float capturedRedFlags;

    public static event Action spawnNewBlueFlag, spawnNewRedFlag;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private void OnEnable()
    {
        BlueFlag.collectBlueFlag += AddBlueFlags;
        RedFlag.collectRedFlag += AddRedFlags;
    }

    private void OnDisable()
    {
        BlueFlag.collectBlueFlag -= AddBlueFlags;
        RedFlag.collectRedFlag -= AddRedFlags;
    }

    private void GameStatus(GameState Status)
    {
        switch (Status)
        {
            case GameState.Running:
                Time.timeScale = 1;
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                break;

        }
    }

    public void Start()
    {
        GameStatus(GameState.Paused);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && Input.GetKeyDown(StartButton))
        {
            GameStatus(GameState.Running);

            SceneManager.LoadScene(1);
        }

        //per attivare la scena easter egg :P
        if (UIManager.Instance.startGame.activeSelf && Input.GetKeyDown(easterEggCode[index]))
        {
            index++;

            if (index == easterEggCode.Length)
            {
                UIManager.Instance.startGame.SetActive(false);
                GameStatus(GameState.Running);
            }

        }

        if (Input.GetKeyDown(PauseButton))
        {

            if (UIManager.Instance.pauseMenu.activeSelf)
            {
                GameStatus(GameState.Running);
                UIManager.Instance.pauseMenu.SetActive(false);
            }
            else
            {
                GameStatus(GameState.Paused);
                UIManager.Instance.TogglePauseMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            UIManager.Instance.PowerUpA_Interaction();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UIManager.Instance.PowerUpS_Interaction();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            UIManager.Instance.PowerUpD_Interaction();
        }
    }

    public void AddBlueFlags()
    {
        blueFlagCollected.Play();
        capturedBlueFlags++;

        if (capturedBlueFlags >= 3)
        {
            WinMenu();
        }
    }

    public void AddRedFlags()
    {
        redFlagCollected.Play();
        capturedRedFlags++;

        if (capturedRedFlags >= 3)
        {
            GameOver();
        }
    }

    public void RemoveBlueFlags()
    {
        if (capturedBlueFlags > 0)
        {
            //blueFlagStolen.Play();
            capturedBlueFlags--;
            spawnNewBlueFlag?.Invoke();
        }
    }

    public void RemoveRedFlags()
    {
        if (capturedRedFlags > 0)
        {
            //redFlagStolen.Play();
            capturedRedFlags--;
            spawnNewRedFlag?.Invoke();
        }
    }

    public void Pause()
    {
        GameStatus(GameState.Paused);
        UIManager.Instance.TogglePauseMenu();

    }
    public void WinMenu()
    {
        GameStatus(GameState.Paused);
        UIManager.Instance.GameOverMenu();
    }
    public void GameOver()
    {
        GameStatus(GameState.Paused);
        UIManager.Instance.GameOverMenu();
    }

}