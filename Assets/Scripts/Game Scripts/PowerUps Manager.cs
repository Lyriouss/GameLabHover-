using UnityEngine;

enum PowerUps
{
    Jump,
    TempWall,
    Invisibility,
    Shield,
    Haste,
    SlowedDown
}

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
    private float originalAcceleration;
    private float originalSpeed;

    //Slowed Down Power Up stats
    private float slowTimer = 0f;
    public float slowTimerDuration = 10f;
    private bool isSlow = false;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpCollect;
    [SerializeField] private AudioSource wallCollect;
    [SerializeField] private AudioSource cloakCollect;
    [SerializeField] private AudioSource jumpActivate;
    [SerializeField] private AudioSource wallActivate;
    [SerializeField] private AudioSource cloakActivate;
    [SerializeField] private AudioSource shieldActivate;
    [SerializeField] private AudioSource hasteActivate;
    [SerializeField] private AudioSource slowActivate;
    [SerializeField] private AudioSource cloakEnd;
    [SerializeField] private AudioSource shieldEnd;
    [SerializeField] private AudioSource hasteEnd;
    [SerializeField] private AudioSource slowEnd;

    private void OnEnable()
    {
        JumpBubble.collectPowerup += CollectPowerup;
        WallBubble.collectPowerup += CollectPowerup;
        InvisibiltyBubble.collectPowerup += CollectPowerup;
    }

    private void OnDisable()
    {
        JumpBubble.collectPowerup -= CollectPowerup;
        WallBubble.collectPowerup -= CollectPowerup;
        InvisibiltyBubble.collectPowerup -= CollectPowerup;
    }

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
        originalAcceleration = PlayerMovement.Instance.accelerationForce;
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
                cloakEnd.Play();

                //quando arriva allo scadere del timer, torna visibile e si disattiva lo screen blu della UI
                int layerChange = LayerMask.NameToLayer("Player");
                player.layer = layerChange;

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
                shieldEnd.Play();

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
                hasteEnd.Play();

                //quando arriva allo scadere del timer, torna alla velocità originale e si resetta tutto
                PlayerMovement.Instance.accelerationForce = originalAcceleration;
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
                slowEnd.Play();

                //quando arriva allo scadere del timer, torna alla velocità originale e si resetta tutto
                PlayerMovement.Instance.accelerationForce = originalAcceleration;
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
            jumpActivate.Play();

            //se il player sta "accelerando" in avanti, la jumpforce è molto più alta per farlo saltare ad ugual altezza di quando invece è fermo
            if (PlayerMovement.Instance.moveForward > 0f)
            {
                PlayerMovement.Instance.playerRB.AddForce(Vector3.up * jumpForce * 10, ForceMode.Impulse);
            }
            else
            {
                PlayerMovement.Instance.playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            return true;
        }

        return false;
    }

    public void TempWall()
    {
        wallActivate.Play();

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
        cloakActivate.Play();

        //il player diventa "invisibile" perdendo temporaneamente la mesh (DA CONTROLLARE APPENA LUCA HA I NEMICI)
        int layerChange = LayerMask.NameToLayer("Default");
        player.layer = layerChange;

        //setto il timer a 0 e attivo il blue screen dell'invisibilità
        invTimer = 0f;
        UIManager.Instance.powerUpD_Screen.SetActive(true);
        isInvisible = true;
    }
    public void Shield()
    {
        shieldActivate.Play();

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
        hasteActivate.Play();

        //se avevo preso un power up della lentezza, beh non più
        isSlow = false;
        slowTimer = 0f;
        UIManager.Instance.slow_fillBar.fillAmount = 0;

        //duplico la velocità originale per farlo andare veloce
        hasteTimer = 0f;
        PlayerMovement.Instance.accelerationForce = originalAcceleration + 5f;
        PlayerMovement.Instance.maxSpeed = originalSpeed + 10f;
        isFast = true;
    }


    public void SlowedDown()
    {
        if (!isShielded)
        {
            slowActivate.Play();

            //se avevo preso un power up della velocità, beh non più
            isFast = false;
            hasteTimer = 0f;
            UIManager.Instance.haste_fillBar.fillAmount = 0;

            //dimezzo la velocità originale per farlo andare veloce
            slowTimer = 0f;
            PlayerMovement.Instance.accelerationForce = originalAcceleration - 5f;
            PlayerMovement.Instance.maxSpeed = originalSpeed - 10f;
            isSlow = true;
        }
    }

    //pe ril metodo Random, abbiamo creato una lista enum dei vari powerup
    //in PowerUpTypes li abbbiamo assegnati ai metodi
    //e in RandomPW() abbiamo randomizzaro il Power Up che ti becchi
    public void RandomPowerUp()
    {
        PowerUps randomPW = (PowerUps)Random.Range(0, System.Enum.GetValues(typeof(PowerUps)).Length);
        PowerUpTypes(randomPW);
    }

    public void CollectPowerup(int index)
    {
        PowerUps collectPU = (PowerUps)index;
        PowerUpTypes(collectPU);
    }
    
    void PowerUpTypes(PowerUps PU)
    {
        switch (PU)
        {
            case PowerUps.Jump:
                jumpCollect.Play();
                UIManager.Instance.powerUpA_quantity += 1;
                UIManager.Instance.powerUpA_TXT.text = UIManager.Instance.powerUpA_quantity.ToString();
                break;

            case PowerUps.TempWall:
                wallCollect.Play();
                UIManager.Instance.powerUpS_quantity += 1;
                UIManager.Instance.powerUpS_TXT.text = UIManager.Instance.powerUpS_quantity.ToString();
                break;

            case PowerUps.Invisibility:
                cloakCollect.Play();
                UIManager.Instance.powerUpD_quantity += 1;
                UIManager.Instance.powerUpD_TXT.text = UIManager.Instance.powerUpD_quantity.ToString();
                break;

            case PowerUps.Shield:
                Shield();
                break;

            case PowerUps.Haste:
                Haste();
                break;

            case PowerUps.SlowedDown:
                SlowedDown();
                break;
        }
    }
}
