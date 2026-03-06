using UnityEngine;

public class Object : MonoBehaviour
{
    public bool Visualized = false;
    MeshRenderer MeshForObject;

    private void Start()
    {
        MeshForObject = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Visualized = false;
            MeshForObject.enabled = false;
        }
    }
}
