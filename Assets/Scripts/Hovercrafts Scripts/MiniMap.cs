using UnityEngine;

public class MiniMap : MonoBehaviour
{
    Object ObjectOnMap;
    MeshRenderer MeshForObject;

    private void OnTriggerEnter(Collider other)
    {
        ObjectOnMap = other.GetComponent<Object>();
        MeshForObject = other.GetComponent<MeshRenderer>();
        
        if (ObjectOnMap.Visualized)
        {
            MeshForObject.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ObjectOnMap = other.GetComponent<Object>();
        MeshForObject = other.GetComponent<MeshRenderer>();

        if (ObjectOnMap.Visualized)
        {
            MeshForObject.enabled = false;
        }
    }
}
