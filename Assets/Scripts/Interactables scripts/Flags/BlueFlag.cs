using System;
using UnityEngine;

public class BlueFlag : MonoBehaviour, IInteractable
{
    //la classe red flag dovrà aggiungere un punto UI al giocatore rosso e 500 allo score, quando interagita
    public static event Action collectBlueFlag;

    public int scorePoint = 500;

    private bool isCollected = false;

    public void OnInteraction()
    {
        if (isCollected) return;

        collectBlueFlag?.Invoke();

        isCollected = true;

        ScoreManager.Instance.AddScore(scorePoint);
    }
}
