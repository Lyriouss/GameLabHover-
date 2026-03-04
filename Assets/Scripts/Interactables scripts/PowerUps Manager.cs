using Unity.VisualScripting;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public GameObject player;

    [Header("Invisibilty Power Up")]
    public float timer = 0f;
    public float timerDuration = 5f;
    private bool isInvisible = false;


    public void Update()
    {
        if (isInvisible)
        {
            timer += Time.deltaTime;

            UIManager.Instance.powerUpD_fillBar.fillAmount = 1f - (timer / timerDuration);

            if (timer > timerDuration)
            {
                player.GetComponent<Renderer>().enabled = true;

                timer = 0f;
                UIManager.Instance.powerUpD_Screen.SetActive(false);
                UIManager.Instance.powerUpD_fillBar.fillAmount = 0;
                isInvisible = false;
            }
        }
    }

    public void Invisibilty()
    {
        player.GetComponent<Renderer>().enabled = false;

        timer = 0f;
        UIManager.Instance.powerUpD_Screen.SetActive(true);
        isInvisible = true;
    }
}
