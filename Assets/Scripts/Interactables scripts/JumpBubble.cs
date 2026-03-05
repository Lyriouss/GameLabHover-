using UnityEngine;

public class JumpBubble : MonoBehaviour, IInteractable
{
    private bool isCollected = false;
    public void OnInteraction()
    {
        if (isCollected) return;

        UIManager.Instance.powerUpA_quantity += 1;
        UIManager.Instance.powerUpA_TXT.text = UIManager.Instance.powerUpA_quantity.ToString();
        isCollected = true;
    }
}
