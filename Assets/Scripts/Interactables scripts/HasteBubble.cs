using UnityEngine;

public class HasteBubble : Bubbles, IInteractable
{
    private bool isCollected = false;

    public void OnInteraction()
    {
        if (isCollected) return;

        PowerUpsManager.Instance.Haste();
        isCollected = true;
    }
}
