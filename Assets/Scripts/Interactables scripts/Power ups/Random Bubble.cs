public class RandomBubble : Bubbles, IInteractable
{
    private bool isCollected = false;

    public void OnInteraction()
    {
        if (isCollected) return;

        PowerUpsManager.Instance.RandomPowerUp();
        isCollected = true;
    }
}
