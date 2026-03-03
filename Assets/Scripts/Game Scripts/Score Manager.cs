using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int Highscore;
    int scorePoint;
    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        
    }

    public void AddScore(int score)
    {
        scorePoint += score;
        UIManager.Instance.score.text = scorePoint.ToString();
    }
}
