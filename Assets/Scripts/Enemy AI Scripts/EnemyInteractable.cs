using UnityEngine;

public class EnemyInteractable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedFlag"))
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
