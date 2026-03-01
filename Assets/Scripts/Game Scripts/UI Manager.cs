using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; 
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject gameOverMenu;
    [SerializeField] public GameObject WinConditionWindow;
    [SerializeField] public GameObject StartGame;

    private void Awake()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        WinConditionWindow.SetActive(false);
        StartGame.SetActive(true);
    }

    public void OnStart()
    {
        Destroy(StartGame);
    }
    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void WinConditionWin()
    {
        WinConditionWindow.SetActive(true);
    }
}
