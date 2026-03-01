using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; 
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject gameOverMenu;
    [SerializeField] public GameObject StartGame;
    [SerializeField] public TMP_Text score;

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
        StartGame.SetActive(true);
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

}
