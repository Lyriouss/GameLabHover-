using UnityEngine;

public class RedFlag : MonoBehaviour, IInteractable
{
    //la classe red flag dovrà aggiungere un punto UI al giocatore rosso e 500 allo score, quando interagita

    public int redFlagPoint = 1;
    public int scorePoint = 500;

    private bool isCollected = false;


    public void OnInteraction()
    {
        if (isCollected) return;

        isCollected = true;

        ScoreManager.Instance.AddScore(scorePoint);

    }
}
