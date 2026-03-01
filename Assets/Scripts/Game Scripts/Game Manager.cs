using UnityEngine;
using System;

enum GameState
{
    Running,
    Paused,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] KeyCode StartButton = KeyCode.F2;
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