using UnityEngine;

public class Interaction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueFlag") || other.CompareTag("Powerup"))
        {
            IInteractable interactable = other.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.OnInteraction();
                Destroy(other.gameObject);
            }
        }
    }
}
