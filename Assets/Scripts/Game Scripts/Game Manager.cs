using UnityEngine;
using System;
using UnityEditor;

enum GameState
{
    Running,
    Paused,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] KeyCode StartButton = KeyCode.F2;
    [SerializeField] KeyCode PauseButton = KeyCode.F3;
    private void GameStatus(GameState Status)
    {
         switch(Status)
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
        if (Input.GetKeyDown(StartButton))
        {
            GameStatus(GameState.Running);
            UIManager.Instance.StartGame.SetActive(false);
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
    }

    public void Pause()
    {
        GameStatus(GameState.Paused);
        UIManager.Instance.TogglePauseMenu();

    }
    public void GameOver()
    {
        GameStatus(GameState.Paused);
        UIManager.Instance.GameOverMenu();
    }

}