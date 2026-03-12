using UnityEngine;

public class TargetState : IEnemyState, IEnemyMovement
{
    private EnemyManager em;

    //private Quaternion targetRotation;
    Vector3 rayDirection;
    float rayDistance;

    public TargetState(EnemyManager state)
    {
        em = state;
    }

    public void Enter()
    {
        if (em.target.CompareTag("Player"))
            em.targetPing.Play();
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (em.target != null)
        {
            CheckForTarget();

            em.navMeshA.destination = em.target.position;
        }
        else
        {
            em.ChangeState(new PatrolState(em));
        }
    }

    public void FixedUpdate()
    {
        if (em.target != null)
        {
            RotateVehicle();
            RegulateMovement();
        }
        else
        {
            em.ChangeState(new PatrolState(em));
        }

        CheckGround();
    }

    public void RotateVehicle()
    {
        Quaternion targetRotation = Quaternion.LookRotation(em.target.position - em.transform.position);
        em.enemyRB.transform.rotation = Quaternion.RotateTowards(em.transform.rotation, targetRotation, em.rotationSpeed * Time.fixedDeltaTime);
    }

    public void RegulateMovement()
    {
        if (Physics.Raycast(em.enemyRB.position, em.transform.forward, em.detectionRadius, em.targetMask)) 
        {
            em.navMeshA.speed = em.targetSpeed;
        }
        else
        {
            em.navMeshA.speed -= Time.fixedDeltaTime;}

        //moves the enemy rigid body in the direction of nav mesh agent path
        em.enemyRB.linearVelocity = em.navMeshA.velocity;
        em.navMeshA.nextPosition = em.enemyRB.position;
    }

    //always keeps the enemy height from ground to a fixed value (falls with gravity force)
    public void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(em.enemyRB.position, Vector3.down, out hit, em.groundDistance + 0.2f, em.groundLayer))
        {
            em.enemyRB.useGravity = false;

            float difference = em.groundDistance - hit.distance;

            em.enemyRB.position += new Vector3(0, difference, 0);
        }

        else
        {
            em.enemyRB.useGravity = true;
        }
    }

    public void CheckForTarget()
    {
        rayDirection = (em.target.transform.position - em.transform.position).normalized;

        RaycastHit hit;

        //gets the distance from an obstacle if there is one present in between target and enemy
        if (Physics.Raycast(em.transform.position, rayDirection, out hit, em.detectionRadius, em.obstacleMask))
        {
            if (hit.collider != null)
            {
                rayDistance = Vector3.Distance(em.transform.position, hit.collider.transform.position);
            }
        }
        else
        {
            rayDistance = em.detectionRadius;
        }

        //checks if the enemy can the see the target
        if (!Physics.Raycast(em.transform.position, rayDirection, rayDistance, em.targetMask))
        {
            em.ChangeState(new PatrolState(em));
        }
    }
}