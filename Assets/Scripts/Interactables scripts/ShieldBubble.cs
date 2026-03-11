public class ShieldBubble : Bubbles, IInteractable
{
    private bool isCollected = false;

    public void OnInteraction()
    {
        if (isCollected) return;

        PowerUpsManager.Instance.Shield();
        isCollected = true;
    }
}
