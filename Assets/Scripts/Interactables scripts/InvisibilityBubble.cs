using System;

public class InvisibiltyBubble : Bubbles, IInteractable
{
    private bool isCollected = false;
    public static event Action<int> collectPowerup;

    public void OnInteraction()
    {
        if (isCollected) return;

        collectPowerup?.Invoke(2);

        //UIManager.Instance.powerUpD_quantity += 1;
        //UIManager.Instance.powerUpD_TXT.text = UIManager.Instance.powerUpD_quantity.ToString();

        isCollected = true;
    }
}