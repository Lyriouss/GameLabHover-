using UnityEngine;

public class HasteBubble : MonoBehaviour, IInteractable
{
    private bool isCollected = false;


    public void OnInteraction()
    {
        if (isCollected) return;

        PowerUpsManager.Instance.Haste();
        isCollected = true;
    }
}
