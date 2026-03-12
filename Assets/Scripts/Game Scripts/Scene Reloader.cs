using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void OkButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
