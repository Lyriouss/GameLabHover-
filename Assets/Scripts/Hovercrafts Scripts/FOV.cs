using UnityEngine;

public class FOV : MonoBehaviour
{
    Object ObjectOnMap;
    MeshRenderer MeshForObject;
    private void OnTriggerStay(Collider other)
    {
        ObjectOnMap = other.GetComponent<Object>();
        MeshForObject = other.GetComponent<MeshRenderer>();

        if (!ObjectOnMap.Visualized)
        { 
            ObjectOnMap.Visualized = true;
            MeshForObject.enabled = true;
        }
    }
}
