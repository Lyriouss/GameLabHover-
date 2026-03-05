using UnityEngine;

public class SlowBubble : MonoBehaviour, IInteractable
{
    private bool isCollected = false;


    public void OnInteraction()
    {
        if (isCollected) return;

        PowerUpsManager.Instance.SlowedDown();
        isCollected = true;
    }
}
