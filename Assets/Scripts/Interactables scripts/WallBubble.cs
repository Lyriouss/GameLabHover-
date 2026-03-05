using UnityEngine;

public class WallBubble : MonoBehaviour, IInteractable
{
    private bool isCollected = false;
    public void OnInteraction()
    {
        if (isCollected) return;

        UIManager.Instance.powerUpS_quantity += 1;
        UIManager.Instance.powerUpS_TXT.text = UIManager.Instance.powerUpS_quantity.ToString();

        isCollected = true;
    }
}
