using UnityEngine;

public class PatrolState : IEnemyState, IEnemyMovement
{
    private EnemyManager em;

    private Vector3 offset = new Vector3(1f, 0f, 1f);
    private float angleCheck;
    private Quaternion targetRotation;

    public PatrolState(EnemyManager state)
    {
        em = state;
    }

    public void Enter()
    {
        em.stayStillTimer = 0f;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (em.stayStillTimer < em.timeStayStill)
            em.stayStillTimer += Time.deltaTime;

        DesReachedCheck();

        em.navMeshA.destination = em.patrolPositions[em.randomDes].position;

        if (em.stayStillTimer >= em.timeStayStill)
        {
            CheckForTarget();
        }
    }

    public void FixedUpdate()
    {
        if (em.stayStillTimer >= em.timeStayStill)
        {
            RotateVehicle();
            RegulateMovement();
        }

        CheckGround();
    }

    public void RotateVehicle()
    {
        targetRotation = Quaternion.LookRotation(em.navMeshA.velocity);
        em.enemyRB.transform.rotation = Quaternion.RotateTowards(em.transform.rotation, targetRotation, em.rotationSpeed * Time.fixedDeltaTime);

        Vector3 currentFacing = em.transform.forward;
        angleCheck = Vector3.Angle(currentFacing, em.lastFacing);
        //Debug.Log(angleCheck);

        if (em.enemyRB.linearVelocity.x >= offset.x || em.enemyRB.linearVelocity.z >= offset.z || em.enemyRB.linearVelocity.x <= -offset.x || em.enemyRB.linearVelocity.z <= -offset.z)
            em.isMoving = true;
        else
            em.isMoving = false;

        if (angleCheck > 0.5f && !em.isRotating && !em.needsToStop && em.isMoving)
        {
            em.isRotating = true;
            em.needsToStop = true;
        }

        em.lastFacing = currentFacing;
    }

    public void RegulateMovement()
    {
        //after detecting a rotation after moving straight, stops the movement of enemy
        if (em.isRotating && em.needsToStop)
        {
            em.navMeshA.speed = 0f;

            float decelerationRate = em.enemyRB.linearVelocity.magnitude / em.decelerationTime;
            em.enemyRB.linearVelocity = Vector3.MoveTowards(em.enemyRB.linearVelocity, Vector3.zero, decelerationRate * Time.fixedDeltaTime);

            //only when completely stopped, heads to the next phase of movement
            if (em.enemyRB.linearVelocity == Vector3.zero)
            {
                em.needsToStop = false;
            }
        }
        //after it stops moving, start rotating the enemy
        else if (em.isRotating && !em.needsToStop)
        {
            em.navMeshA.speed = 0.01f;

            //only when the difference between current rotation angle and last rotation angle is less than 0.5
            if (angleCheck <= 0.5f)
                em.isRotating = false;
        }
        //after rotation is complete, move the enemy
        else if (!em.isRotating && !em.needsToStop)
        {
            em.navMeshA.speed = em.patrolSpeed;
        }

        //moves the enemy rigid body in the direction of nav mesh agent path
        em.enemyRB.linearVelocity = em.navMeshA.velocity;
        em.navMeshA.nextPosition = em.enemyRB.position;
    }

    //always keeps the enemy height from ground to a fixed value (falls with gravity force)
    public void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(em.enemyRB.position, Vector3.down, out hit, em.groundDistance, em.groundLayer))
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
        //when detecting a target of enemy
        Collider[] check = Physics.OverlapSphere(em.transform.position, em.detectionRadius, em.targetMask);

        if (check.Length == 0)
            return;

        foreach (Collider target in check)
        {
            //gets direction and distance from target
            Vector3 rayDirection = (target.transform.position - em.transform.position).normalized;
            float rayDistance = Vector3.Distance(target.transform.position, em.transform.position);

            //and casts a RayCast to see if there are any obstacles in the way (is in line of sight)
            if (!Physics.Raycast(em.transform.position, rayDirection, rayDistance, em.obstacleMask))
            {
                //if no obstacles were detected, changes enemy state and assigns target
                em.target = target.transform;
                em.ChangeState(new TargetState(em));
            }
        }
    }

    //when reaching path destination, gets another destination to travel to
    public void DesReachedCheck()
    {
        float CheckDistance = Vector3.Distance(em.patrolPositions[em.randomDes].position, em.transform.position);

        if (CheckDistance < 3f)
        {
            em.randomDes = Random.Range(0, em.patrolPositions.Count);
        }
    }
}
