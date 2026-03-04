using UnityEngine;

public class InvisibiltyBubble : MonoBehaviour, IInteractable
{
    private bool isCollected = false;
    public void OnInteraction()
    {
        if (isCollected) return;

        isCollected = true;

        UIManager.Instance.powerUpD_quantity += 1;
        UIManager.Instance.powerUpD_TXT.text = UIManager.Instance.powerUpD_quantity.ToString();
    }
}
