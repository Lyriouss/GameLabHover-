using UnityEngine;

enum MoveState 
{
    Idle,
    Running,
    Fast,
    Slow
}

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    MoveState moveState;
    private string changeSound;
    public string currentSound;

    public Rigidbody playerRB;
    public float moveForward;

    [Header("Player Movement Parameters")]
    [SerializeField] public float accelerationForce = 15f; //la forza limite dell'accelerazione 
    [SerializeField] public float maxSpeed = 7f; //velocità massima di movimento del Player
    [SerializeField] public float bounceBackForce = 5f; //la forza con cui si viene respinti dopo aver collidato con un muro 
    [SerializeField] public float rotationSpeed = 50f; //la velocità con cui il player ruota a sx/dx
    [SerializeField] public float groundDistance = 2f; // distanza fra player e ground
    public bool isGrounded = true; //mi dice se è a terra

    public LayerMask groundLayer;
    public float rayLenght = 10f;

    [Header("Audio")]
    public AudioSource engineIdle;
    public AudioSource engineRun;
    public AudioSource engineHaste;
    public AudioSource engineSlow;
    [SerializeField] private AudioSource jumpEnd;
    [SerializeField] private AudioSource collisionWall;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        playerRB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        changeSound = null;
        currentSound = null;
    }

    public void FixedUpdate()
    {
        Accelerate();
        Rotate();
        CheckGround();
    }

    //per l'accelerazione avanti/indietro
    void Accelerate()
    {
        moveForward = Input.GetAxis("Vertical");

        if (moveForward != 0)
        {
            if (PowerUpsManager.Instance.isFast)
            {
                engineIdle.loop = false;
                engineRun.loop = false;
                engineHaste.loop = true;
                engineSlow.loop = false;

                moveState = MoveState.Fast;
                changeSound = "Fast";
            }
            else if (PowerUpsManager.Instance.isSlow)
            {
                engineIdle.loop = false;
                engineRun.loop = false;
                engineHaste.loop = false;
                engineSlow.loop = true;

                moveState = MoveState.Slow;
                changeSound = "Slow";
            }
            else
            {
                engineIdle.loop = false;
                engineRun.loop = true;
                engineHaste.loop = false;
                engineSlow.loop = false;

                moveState = MoveState.Running;
                changeSound = "Running";
            }

            playerRB.AddForce(transform.forward * moveForward * accelerationForce, ForceMode.Acceleration);

            if (playerRB.linearVelocity.magnitude > maxSpeed)
            {
                playerRB.linearVelocity = playerRB.linearVelocity.normalized * maxSpeed;
            }
        }
        else
        {
            engineIdle.loop = true;
            engineRun.loop = false;
            engineHaste.loop = false;
            engineSlow.loop = false;

            moveState = MoveState.Idle;
            changeSound = "Idle";
        }

        if (changeSound != currentSound)
        {
            switch (moveState)
            {
                case MoveState.Idle:
                    engineIdle.Play();
                    currentSound = "Idle";

                    engineRun.Stop();
                    engineHaste.Stop();
                    engineSlow.Stop();
                    break;

                case MoveState.Running:
                    engineRun.Play();
                    currentSound = "Running";

                    engineIdle.Stop();
                    engineHaste.Stop();
                    engineSlow.Stop();
                    break;

                case MoveState.Fast:
                    engineHaste.Play();
                    currentSound = "Fast";

                    engineRun.Stop();
                    engineIdle.Stop();
                    engineSlow.Stop();
                    break;

                case MoveState.Slow:
                    engineSlow.Play();
                    currentSound = "Slow";

                    engineRun.Stop();
                    engineHaste.Stop();
                    engineIdle.Stop();
                    break;
            }
        }

        //per farlo frenare:
        float decelerationTime = 1.5f; // tempo che ci mette per fermarsi dopo aver smesso di ricevere l'input di accelerazione
        float decelerationRate = playerRB.linearVelocity.magnitude / decelerationTime; //e il suo rate

        playerRB.linearVelocity = Vector3.MoveTowards(playerRB.linearVelocity, Vector3.zero, decelerationRate * Time.fixedDeltaTime);//applicato al fixed DeltaTime

    }

    //per ruotare il player a destra/sinistra
    void Rotate()
    {
        float rotation = rotationSpeed * Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRB.MoveRotation(playerRB.rotation * Quaternion.Euler(0f, -rotation, 0f));
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRB.MoveRotation(playerRB.rotation * Quaternion.Euler(0f, rotation, 0f));
        }
    }


    //per tenere il player sempre a un tot di distanza dal ground, sia questo ground o scale
    void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerRB.position, Vector3.down, out hit, groundDistance + 0.25f, groundLayer))
        {
            if (!isGrounded)
            {
                isGrounded = true;
                jumpEnd.Play();
            }

            //tocca disattivare la gravità sennò rimbalza all'infinito lol
            playerRB.useGravity = false;

            float difference = groundDistance - hit.distance;

            playerRB.position += new Vector3(0, difference, 0);
        }

        else
        {
            isGrounded = false;
            //la attivo solo quando cadiamo
            playerRB.useGravity = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy"))
        {
            collisionWall.Play();
            //playerRB.angularVelocity = Vector3.zero;
            playerRB.linearVelocity = Vector3.zero;
            playerRB.AddForce(-transform.forward * bounceBackForce, ForceMode.Impulse);
        }
    }

    //per colorare il raycast in fase di develop
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * groundDistance);
    }
}
