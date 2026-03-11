using System;
using UnityEngine;

public class JumpBubble : Bubbles, IInteractable
{
    private bool isCollected = false;
    public static event Action<int> collectPowerup;

    public void OnInteraction()
    {
        if (isCollected) return;

        collectPowerup?.Invoke(0);

        //UIManager.Instance.powerUpA_quantity += 1;
        //UIManager.Instance.powerUpA_TXT.text = UIManager.Instance.powerUpA_quantity.ToString();

        isCollected = true;
    }
}
