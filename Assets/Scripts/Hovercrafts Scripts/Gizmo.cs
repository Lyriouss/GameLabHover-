using UnityEngine;

public class Gizmo : MonoBehaviour
{
    [SerializeField] Mesh MeshCollider;
    [SerializeField] Color color;
    private int LookRadius;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}