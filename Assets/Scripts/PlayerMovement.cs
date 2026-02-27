using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRB;
    private float moveForward;

    [Header("Player Movement Parameters")]
    [SerializeField] public float accelerationForce = 10f; //la forza limite dell'accelerazione 
    [SerializeField] public float maxSpeed = 25f; //velocità massima di movimento del Player
    [SerializeField] private float rotationSpeed = 20f; //la velocità con cui il player ruota a sx/dx
    [SerializeField] private float groundDistance = 3f; // distanza fra player e ground

    public LayerMask groundLayer;
    public float rayLenght = 10f;


    void Start()
    {
        playerRB = GetComponent<Rigidbody>();

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
            playerRB.AddForce(transform.forward * moveForward * accelerationForce, ForceMode.Acceleration);

            if (playerRB.linearVelocity.magnitude > maxSpeed)
            {
                playerRB.linearVelocity = playerRB.linearVelocity.normalized * maxSpeed;
            }
        }

        //per farlo frenare:
        
        float decelerationTime = 1f; // tempo che ci mette per fermarsi dopo aver smesso di ricevere l'input di accelerazione
        float decelerationRate = playerRB.linearVelocity.magnitude / decelerationTime; //e il suo rate

        playerRB.linearVelocity = Vector3.MoveTowards(playerRB.linearVelocity, Vector3.zero, decelerationRate * Time.fixedDeltaTime);//applicato al fixed DeltaTime

    }

    //per ruotare il player a destra/sinistra
    void Rotate()
    {
        float rotation = rotationSpeed * Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            playerRB.MoveRotation(playerRB.rotation * Quaternion.Euler(0f, -rotation, 0f));
        }

        else if (Input.GetKey(KeyCode.D))
        {
            playerRB.MoveRotation(playerRB.rotation * Quaternion.Euler(0f, rotation, 0f));
        }

    }

    //per tenere il player sempre a un tot di distanza dal ground, sia questo ground o scale
    void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerRB.position, Vector3.down, out hit, groundDistance, groundLayer))
        {
            //tocca disattivare la gravità sennò rimbalza all'infinito lol
            playerRB.useGravity = false;

            float difference = groundDistance - hit.distance;

            playerRB.position += new Vector3(0, difference, 0);
        }

        else
        {
            //la attivo solo quando cadiamo
            playerRB.useGravity = true;
        }

    }

    //per colorare il raycast in fase di develop
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * groundDistance);
    }
}
