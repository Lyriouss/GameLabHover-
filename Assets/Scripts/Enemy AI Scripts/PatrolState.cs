using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PatrolState : IEnemyState, IEnemyMovement
{
    private EnemyManager em;

    private float elapsedTime;

    public PatrolState(EnemyManager state)
    {
        em = state;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1f)
            GeneratePath();

        CheckForTarget();
    }

    public void FixedUpdate()
    {
        CheckGround();
    }

    //Only switches states when detecting it's target and can be seen
    public void CheckForTarget()
    {
        Collider[] check = Physics.OverlapSphere(em.transform.position, em.detectionRadius, em.targetMask);

        if (check.Length == 0)
            return;

        Vector3 checkDirection = (check[0].transform.position - em.transform.position).normalized;
        float checkDistance = Vector3.Distance(check[0].transform.position, em.transform.position);

        if (!Physics.Raycast(em.transform.position, checkDirection, checkDistance, em.obstacleMask))
        {
            em.ChangeState(new TargetState(em));
        }
    }

    public void GeneratePath()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(em.transform.position, em.target.position, NavMesh.AllAreas, path);

        em.path = path;

        for (int i = 0; i < em.path.corners.Length; i++)
        {
            Debug.DrawLine(em.path.corners[i], em.path.corners[i + 1], Color.red);
            Debug.Log(em.path.corners[i + 1]);
        }
    }

    public void CalibrateVehicle()
    {
        //float rotation = em.rotationSpeed * Time.fixedDeltaTime;
        //float direction = 

        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
            //em.enemyRB.MoveRotation(em.enemyRB.rotation * Quaternion.Euler(0f, -rotation, 0f));
        //}

        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
            //em.enemyRB.MoveRotation(em.enemyRB.rotation * Quaternion.Euler(0f, rotation, 0f));
        //}
    }

    public void Accelerate()
    {
        em.enemyRB.AddForce(em.transform.forward * em.accelerationForce, ForceMode.Acceleration);

        if (em.enemyRB.linearVelocity.magnitude > em.patrolSpeed)
        {
            em.enemyRB.linearVelocity = em.enemyRB.linearVelocity.normalized * em.patrolSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TravelPoint"))
        {

        }
    }

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
}
