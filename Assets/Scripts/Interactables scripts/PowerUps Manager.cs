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

    //Shield Power Up stats
    private float shieldTimer = 0f;
    public float shieldTimerDuration = 5f;
    private bool isShielded = false;

    //Haste Power Up stats
    private float hasteTimer = 0f;
    public float hasteTimerDuration = 5f;
    private bool isFast = false;
    private float originalSpeed;

    //Slowed Down Power Up stats
    private float slowTimer = 0f;
    public float slowTimerDuration = 5f;
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
    public void FixedUpdate()
    {
        if (isInvisible)
        {
            invTimer += Time.fixedDeltaTime;

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

        if (isShielded)
        {
            shieldTimer += Time.fixedDeltaTime;
            UIManager.Instance.shield_fillBar.fillAmount = 1f - (shieldTimer / shieldTimerDuration);
        }

        if (isFast)
        {
            hasteTimer += Time.fixedDeltaTime;

            UIManager.Instance.haste_fillBar.fillAmount = 1f - (hasteTimer / hasteTimerDuration);

            if (hasteTimer > hasteTimerDuration)
            {
                PlayerMovement.Instance.maxSpeed = originalSpeed / 2;

                hasteTimer = 0f;
                UIManager.Instance.haste_fillBar.fillAmount = 0;
                isFast = false;
            }
        }

        if (isSlow)
        {
            slowTimer += Time.fixedDeltaTime;

            UIManager.Instance.slow_fillBar.fillAmount = 1f - (slowTimer / slowTimerDuration);

            if (slowTimer > slowTimerDuration)
            {
                PlayerMovement.Instance.maxSpeed = originalSpeed * 2;

                slowTimer = 0f;
                UIManager.Instance.slow_fillBar.fillAmount = 0;
                isSlow = false;
            }
        }
    }

    public bool Jump()
    {
        if (PlayerMovement.Instance.isGrounded)
        {
            if (PlayerMovement.Instance.moveForward > 0f)
                PlayerMovement.Instance.playerRB.AddForce(Vector3.up * jumpForce * 10, ForceMode.Impulse);

            else
                PlayerMovement.Instance.playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

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
        slowTimer = 0f;
        UIManager.Instance.slow_fillBar.fillAmount = 0;

        hasteTimer = 0f;
        PlayerMovement.Instance.maxSpeed = originalSpeed * 2;
        isFast = true;
    }

    public void Shield()
    {
        shieldTimer = 0f;
        isShielded = true;

        if (isSlow)
        {
            isSlow = false;
            slowTimer = 0f;
            PlayerMovement.Instance.maxSpeed = originalSpeed;
            UIManager.Instance.slow_fillBar.fillAmount = 0;
        }
    }

    public void SlowedDown()
    {
        if (!isShielded)
        {
            isFast = false;
            hasteTimer = 0f;
            UIManager.Instance.haste_fillBar.fillAmount = 0;

            slowTimer = 0f;
            PlayerMovement.Instance.maxSpeed = originalSpeed / 2;
            isSlow = true;
        }
    }
}
