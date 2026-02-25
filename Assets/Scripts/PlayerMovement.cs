using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRB;
    private float moveHorizontal;
    private float moveForward;

    [Header("Player Movement Parameters")]
    [SerializeField] public float accelerationForce = 10f; //la forza limite dell'accelerazione 
    [SerializeField] public float maxSpeed = 25f; //velocità massima di movimento del Player
    [SerializeField] private float rotationSpeed = 20f; //la velocità con cui il player ruota a sx/dx


    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveForward = Input.GetAxis("Vertical");
    }

    public void FixedUpdate()
    {
        Accelerate();
        Rotate();
    }

    //per l'accelerazione avanti/indietro
    void Accelerate()
    {
        if (moveForward != 0)
        {
            playerRB.AddForce(transform.forward * moveForward * accelerationForce, ForceMode.Acceleration);

            if (playerRB.linearVelocity.magnitude > maxSpeed)
            {
                playerRB.linearVelocity = playerRB.linearVelocity.normalized * maxSpeed;
            }
        }

    }
    //per ruotare il player a destra/sinistra
    void Rotate()
    {
        float rotation = moveHorizontal * rotationSpeed * Time.fixedDeltaTime;

        if (moveHorizontal != 0)
        {
            playerRB.MoveRotation(playerRB.rotation * Quaternion.Euler(0f, rotation, 0f));
        }

    }
}
