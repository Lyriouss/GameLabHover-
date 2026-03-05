using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public static PowerUpsManager Instance;

    public GameObject player;

    //Jump Power Up stats
    public int jumpForce = 25;

    //Wall Power Up stats
    [SerializeField] public GameObject tempWallPrefab;
    private float wallTimer = 0f;
    public float wallTimerDuration = 15f;
    public float wallDistance = 3f;
    private bool wallCreated = false;

    //Invisibilty Power Up stats
    private float invTimer = 0f;
    public float invTimerDuration = 10f;
    private bool isInvisible = false;

    //Shield Power Up stats
    private float shieldTimer = 0f;
    public float shieldTimerDuration = 15f;
    private bool isShielded = false;

    //Haste Power Up stats
    private float hasteTimer = 0f;
    public float hasteTimerDuration = 10f;
    private bool isFast = false;
    private float originalSpeed;

    //Slowed Down Power Up stats
    private float slowTimer = 0f;
    public float slowTimerDuration = 10f;
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
        //settiamo la velocità originale del player pre-power ups
        originalSpeed = PlayerMovement.Instance.maxSpeed;
    }
    public void FixedUpdate()
    {
        if (isInvisible)
        {
            //se è invisibile, parte il timer (anche della UI)
            invTimer += Time.fixedDeltaTime;

            UIManager.Instance.powerUpD_fillBar.fillAmount = 1f - (invTimer / invTimerDuration);

            if (invTimer > invTimerDuration)
            {
                //quando arriva allo scadere del timer, torna visibile e si disattiva lo screen blu della UI
                player.GetComponent<Renderer>().enabled = true;

                invTimer = 0f;
                UIManager.Instance.powerUpD_Screen.SetActive(false);
                UIManager.Instance.powerUpD_fillBar.fillAmount = 0;
                isInvisible = false;
            }
        }

        if (wallCreated)
        {
            //se si crea un muro parte il timer per il suo despawn (anche della UI)
            wallTimer += Time.fixedDeltaTime;
            UIManager.Instance.powerUpS_fillBar.fillAmount = 1f - (wallTimer / wallTimerDuration);


            if (wallTimer > wallTimerDuration)
            {
                //quando arriva allo scadere del timer, si resetta tutto
                wallTimer = 0f;
                UIManager.Instance.powerUpS_fillBar.fillAmount = 0;
                wallCreated = false;
            }
        }

        if (isShielded)
        {
            //se c'è lo shield attivo, parte il timer per la sua durata (anche della UI)
            shieldTimer += Time.fixedDeltaTime;
            UIManager.Instance.shield_fillBar.fillAmount = 1f - (shieldTimer / shieldTimerDuration);

            if (shieldTimer > shieldTimerDuration)
            {
                //quando arriva allo scadere del timer, si resetta tutto
                shieldTimer = 0f;
                UIManager.Instance.shield_fillBar.fillAmount = 0;
                isShielded = false;
            }
        }

        if (isFast)
        {
            //se ha preso il power up della velocità, parte il timer (anche della UI)
            hasteTimer += Time.fixedDeltaTime;

            UIManager.Instance.haste_fillBar.fillAmount = 1f - (hasteTimer / hasteTimerDuration);

            if (hasteTimer > hasteTimerDuration)
            {
                //quando arriva allo scadere del timer, torna alla velocità originale e si resetta tutto
                PlayerMovement.Instance.maxSpeed = originalSpeed;

                hasteTimer = 0f;
                UIManager.Instance.haste_fillBar.fillAmount = 0;
                isFast = false;
            }
        }

        if (isSlow)
        {
            //se ha preso il power up della lentezza, parte il timer (anche della UI)
            slowTimer += Time.fixedDeltaTime;

            UIManager.Instance.slow_fillBar.fillAmount = 1f - (slowTimer / slowTimerDuration);

            if (slowTimer > slowTimerDuration)
            {
                //quando arriva allo scadere del timer, torna alla velocità originale e si resetta tutto

                PlayerMovement.Instance.maxSpeed = originalSpeed;

                slowTimer = 0f;
                UIManager.Instance.slow_fillBar.fillAmount = 0;
                isSlow = false;
            }
        }
    }

    //questo è un bool perchè ci serve nel UI Manager (Se stiamo saltando, aggiorna la UI del boost a -1 solo se abbiamo toccato terra e possiamo saltare di nuovo)
    public bool Jump()
    {
        //si attiva il salto solo se il player risulta a terra dallo script di movimento
        if (PlayerMovement.Instance.isGrounded)
        {
            //se il player sta "accelerando" in avanti, la jumpforce è molto più alta per farlo saltare ad ugual altezza di quando invece è fermo
            if (PlayerMovement.Instance.moveForward > 0f)
                PlayerMovement.Instance.playerRB.AddForce(Vector3.up * jumpForce * 10, ForceMode.Impulse);

            else
                PlayerMovement.Instance.playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            return true;
        }

        return false;
    }

    public void tempWall()
    {
        //settiamo le impostazioni per spawnare il wallPrefab dietro di noi, e ruotato di 90° sull'asse y rispetto al player
        Quaternion wallRot = PlayerMovement.Instance.playerRB.rotation * Quaternion.Euler(0, 90, 0);
        Vector3 wallPos = PlayerMovement.Instance.playerRB.transform.position - PlayerMovement.Instance.playerRB.transform.forward * wallDistance;
        
        //e lo creiamo, settando il timer a 0
        GameObject newTempWall = Instantiate(tempWallPrefab, wallPos, wallRot);

        wallTimer = 0f;
        wallCreated = true;

        //lo distruggiamo solo dopo che ha hittato la wallTimerDuration
        Destroy(newTempWall, wallTimerDuration);
    }

    public void Invisibilty()
    {
        //il player diventa "invisibile" perdendo temporaneamente la mesh (DA CONTROLLARE APPENA LUCA HA I NEMICI)
        player.GetComponent<Renderer>().enabled = false;

        //setto il timer a 0 e attivo il blue screen dell'invisibilità
        invTimer = 0f;
        UIManager.Instance.powerUpD_Screen.SetActive(true);
        isInvisible = true;
    }
    public void Shield()
    {
        //setto il timer
        shieldTimer = 0f;
        isShielded = true;

        //se ha l'effetto negativo della lentezza, lo cancella
        if (isSlow)
        {
            isSlow = false;
            slowTimer = 0f;
            PlayerMovement.Instance.maxSpeed = originalSpeed;
            UIManager.Instance.slow_fillBar.fillAmount = 0;
        }
    }

    public void Haste()
    {
        //se avevo preso un power up della lentezza, beh non più
        isSlow = false;
        slowTimer = 0f;
        UIManager.Instance.slow_fillBar.fillAmount = 0;

        //duplico la velocità originale per farlo andare veloce
        hasteTimer = 0f;
        PlayerMovement.Instance.maxSpeed = originalSpeed * 2;
        isFast = true;
    }


    public void SlowedDown()
    {
        if (!isShielded)
        {
            //se avevo preso un power up della velocità, beh non più
            isFast = false;
            hasteTimer = 0f;
            UIManager.Instance.haste_fillBar.fillAmount = 0;

            //dimezzo la velocità originale per farlo andare veloce
            slowTimer = 0f;
            PlayerMovement.Instance.maxSpeed = originalSpeed / 2;
            isSlow = true;
        }
    }
}
