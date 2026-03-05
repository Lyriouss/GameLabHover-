using Unity.VisualScripting;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public static PowerUpsManager Instance;

    public GameObject player;

    //Jump Power Up stats
    public int jumpForce = 25;

    //Invisibilty Power Up stats
    private float invTimer = 0f;
    public float invTimerDuration = 5f;
    private bool isInvisible = false;

    //Haste Power Up stats
    private float hasTimer = 0f;
    public float hasTimerDuration = 5f;
    private bool isFast = false;
    private float originalSpeed;

    //Slowed Down Power Up stats
    private float sloTimer = 0f;
    public float sloTimerDuration = 5f;
    private bool isSlow = false;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        originalSpeed = PlayerMovement.Instance.maxSpeed;
    }
    public void Update()
    {
        if (isInvisible)
        {
            invTimer += Time.deltaTime;

            UIManager.Instance.powerUpD_fillBar.fillAmount = 1f - (invTimer / invTimerDuration);

            if (invTimer > invTimerDuration)
            {
                player.GetComponent<Renderer>().enabled = true;

                invTimer = 0f;
                UIManager.Instance.powerUpD_Screen.SetActive(false);
                UIManager.Instance.powerUpD_fillBar.fillAmount = 0;
                isInvisible = false;
            }
        }

        if (isFast)
        {
            hasTimer += Time.deltaTime;

            UIManager.Instance.haste_fillBar.fillAmount = 1f - (hasTimer / hasTimerDuration);

            if (hasTimer > hasTimerDuration)
            {
                PlayerMovement.Instance.maxSpeed = originalSpeed / 2;

                hasTimer = 0f;
                UIManager.Instance.haste_fillBar.fillAmount = 0;
                isFast = false;
            }
        }

        if (isSlow)
        {
            sloTimer += Time.deltaTime;

            UIManager.Instance.slow_fillBar.fillAmount = 1f - (sloTimer / sloTimerDuration);

            if (sloTimer > sloTimerDuration)
            {
                PlayerMovement.Instance.maxSpeed = originalSpeed * 2;

                sloTimer = 0f;
                UIManager.Instance.slow_fillBar.fillAmount = 0;
                isSlow = false;
            }
        }
    }

    public bool Jump()
    {
        if (PlayerMovement.Instance.isGrounded)
        {
            Rigidbody rb = PlayerMovement.Instance.playerRB;

            rb.linearVelocity = new Vector3(0f, jumpForce, 0f);
            return true;
        }

        return false;
    }

    public void Invisibilty()
    {
        player.GetComponent<Renderer>().enabled = false;

        invTimer = 0f;
        UIManager.Instance.powerUpD_Screen.SetActive(true);
        isInvisible = true;
    }

    public void Haste()
    {
        isSlow = false;
        sloTimer = 0f;
        UIManager.Instance.slow_fillBar.fillAmount = 0;

        hasTimer = 0f;
        PlayerMovement.Instance.maxSpeed = originalSpeed * 2;
        isFast = true;
    }

    public void SlowedDown()
    {
        isFast = false;
        hasTimer = 0f;
        UIManager.Instance.haste_fillBar.fillAmount = 0;

        sloTimer = 0f;
        PlayerMovement.Instance.maxSpeed = originalSpeed / 2;
        isSlow = true;
    }
}
