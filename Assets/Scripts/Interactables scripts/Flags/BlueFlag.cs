using UnityEngine;

public class BlueFlag : MonoBehaviour, IInteractable
{
    //la classe red flag dovrà aggiungere un punto UI al giocatore rosso e 500 allo score, quando interagita
    [SerializeField] private AudioSource flagCollected;

    public int redFlagPoint = 1;
    public int scorePoint = 500;

    private bool isCollected = false;

    public void OnInteraction()
    {
        if (isCollected) return;

        flagCollected.Play();

        isCollected = true;

        ScoreManager.Instance.AddScore(scorePoint);
    }
}
