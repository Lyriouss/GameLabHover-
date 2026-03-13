using UnityEngine;

public class RemoveFlagTrap : MonoBehaviour
{
    private Rigidbody losingHovercraft; //quale hovercraft perde la bandeira
    private bool isLosing;
    private Collider trapCollider;

    private void Awake()
    {
        //ci gettiamo il collider della trappola 
        trapCollider = GetComponent<Collider>();
    }

    public void FixedUpdate()
    {
        //se qualcuno è caduto in trappola
        if (isLosing && losingHovercraft != null)
        {
            //resettiamo gli stati
            isLosing = false;
            losingHovercraft = null;
        }
    }

    //quando entriamo nel trigger
    public void OnTriggerEnter(Collider other)
    {
        if (PowerUpsManager.Instance.isShielded)
            return;

        //ci gettiamo il rigidbody della navicella che ha beccato la trappola
        Rigidbody hoversRB = other.GetComponent<Rigidbody>();
        losingHovercraft = hoversRB;

        //prendiamo il suo tag per capire se è friend or foe
        string hoverTag = losingHovercraft.gameObject.tag;

        //se è enemy
        if (hoverTag == "Enemy")
        {
            //abbassa di uno il suo punteggio
            GameManager.Instance.RemoveRedFlags();

            isLosing = true;
        }

        //se è il player
        else if (hoverTag == "Player")
        {
            //abbassa di uno il suo punteggio
            GameManager.Instance.RemoveBlueFlags();

            isLosing = true;
        }
    }
}
