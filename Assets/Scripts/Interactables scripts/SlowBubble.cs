public class SlowBubble : Bubbles, IInteractable
{
    private bool isCollected = false;

    public void OnInteraction()
    {
        if (isCollected) return;

        PowerUpsManager.Instance.SlowedDown();
        isCollected = true;
    }
}
