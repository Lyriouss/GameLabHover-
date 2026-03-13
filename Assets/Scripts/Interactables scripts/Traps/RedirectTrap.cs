using UnityEngine;

public class RedirectTrap : MonoBehaviour
{
    [SerializeField] AudioSource pushTrap; 
    //Stop trap stats
    private Rigidbody redirectedHovercraft; //gli hovercraft da spingere
    public float pushForce = 10f; //la forza con cui vengono spinti
    private bool isRedirected;
    private Collider trapCollider;

    private void Awake()
    {
        //ci gettiamo il collider della trappola 
        trapCollider = GetComponent<Collider>();
    }

    public void FixedUpdate()
    {
        //se qualcuno è caduto in trappola
        if (isRedirected && redirectedHovercraft != null)
        {
            //lo spingiamo nella direzione in cui l'abbiamo impostato
            redirectedHovercraft.AddForce(transform.forward * pushForce, ForceMode.Impulse);

            //resettiamo gli stati
            isRedirected = false;
            redirectedHovercraft = null;
        }
    }

    //quando entriamo nel trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PowerUpsManager.Instance.isShielded)
                return;

            pushTrap.Play();

            //ci gettiamo il rigidbody della navicella che ha beccato la trappola
            Rigidbody hoversRB = other.GetComponent<Rigidbody>();
            redirectedHovercraft = hoversRB;

            redirectedHovercraft.angularVelocity = Vector3.zero;
            redirectedHovercraft.linearVelocity = Vector3.zero;

            //centriamo il player mantenendo la sua altezza
            Vector3 trapCenter = new Vector3(transform.position.x, redirectedHovercraft.position.y, transform.position.z);
            redirectedHovercraft.MovePosition(trapCenter);

            //gli diamo la stessa direzione in cui "punta" la trappola
            redirectedHovercraft.MoveRotation(Quaternion.Euler(0f, transform.eulerAngles.y, 0f));

            isRedirected = true;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            //ci gettiamo il rigidbody della navicella che ha beccato la trappola
            Rigidbody hoversRB = other.GetComponent<Rigidbody>();
            redirectedHovercraft = hoversRB;

            redirectedHovercraft.angularVelocity = Vector3.zero;
            redirectedHovercraft.linearVelocity = Vector3.zero;

            //centriamo il player mantenendo la sua altezza
            Vector3 trapCenter = new Vector3(transform.position.x, redirectedHovercraft.position.y, transform.position.z);
            redirectedHovercraft.MovePosition(trapCenter);

            //gli diamo la stessa direzione in cui "punta" la trappola
            redirectedHovercraft.MoveRotation(Quaternion.Euler(0f, transform.eulerAngles.y, 0f));

            isRedirected = true;
        }
    }
}
