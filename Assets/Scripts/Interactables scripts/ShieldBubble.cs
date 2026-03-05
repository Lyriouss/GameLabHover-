using UnityEngine;

public class ShieldBubble : MonoBehaviour, IInteractable
{
    private bool isCollected = false;

    public void OnInteraction()
    {
        if (isCollected) return;

        PowerUpsManager.Instance.Shield();
        isCollected = true;
    }
}
