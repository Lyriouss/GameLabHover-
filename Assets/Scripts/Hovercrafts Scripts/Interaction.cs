using UnityEngine;

public class Interaction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        
        if (interactable != null)
        {
            interactable.OnInteraction();
            Destroy(other.gameObject);
        }
    }
}
