using System;

public class WallBubble : Bubbles, IInteractable
{
    private bool isCollected = false;
    public static event Action<int> collectPowerup;

    public void OnInteraction()
    {
        if (isCollected) return;

        collectPowerup?.Invoke(1);

        //UIManager.Instance.powerUpS_quantity += 1;
        //UIManager.Instance.powerUpS_TXT.text = UIManager.Instance.powerUpS_quantity.ToString();

        isCollected = true;
    }
}
