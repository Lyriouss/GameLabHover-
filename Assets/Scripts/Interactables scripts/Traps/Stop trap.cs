using UnityEngine;

public class Stoptrap : MonoBehaviour
{
    //Stop trap stats
    private Rigidbody trappedHovercraft; //gli hovercraft da intrappolare
    private float stopTimer = 0f;
    public float StopTimerDuration = 5f;
    private bool isStopped = false;
    private Collider trapCollider; //mi serve il collider da disattivare quando il soggetto è intrappolato

    private void Awake()
    {
        //ci gettiamo il collider della trappola
        trapCollider = GetComponent<Collider>();
    }

    public void FixedUpdate()
    {
        //se qualcuno è caduto in trappola
        if (isStopped && trappedHovercraft != null)
        {
            //gli freeziamo la posizione 
            trappedHovercraft.constraints = RigidbodyConstraints.FreezePosition;

            //parte il timer
            stopTimer += Time.fixedDeltaTime;

            //quando il timer arriva allo scadere del tempo
            if (stopTimer >= StopTimerDuration)
            {
                // sblocchiamo posizione ma manteniamo rotazione congelata
                trappedHovercraft.constraints = RigidbodyConstraints.FreezeRotation;

                //cambio gli stati
                isStopped = false;
                trappedHovercraft = null;
                trapCollider.enabled = true;
            }
        }
    }

    //quando entriamo nel trigger
    public void OnTriggerEnter(Collider other)
    {
        //se la trappola è già attiva, non far nulla
        if (isStopped) return;

        //ci gettiamo il rigidbody della navicella che ha beccato la trappola
        Rigidbody HoversRB = other.GetComponent<Rigidbody>();
        trappedHovercraft = HoversRB;

        //centriamo il player mantenendo la sua altezza
        Vector3 trapCenter = new Vector3(transform.position.x, trappedHovercraft.position.y, transform.position.z);
        trappedHovercraft.MovePosition(trapCenter);

        //reset timer
        stopTimer = 0f;
        isStopped = true;

        //disattiviamo il collider per evitare altri ingressi
        trapCollider.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        //riattiviamo il collider quando l'hover esce dalla trappola
        if (!isStopped)
        {
            trapCollider.enabled = true;
        }
    }

}
