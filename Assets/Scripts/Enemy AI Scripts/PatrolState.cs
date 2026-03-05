using System.Collections;
using UnityEngine;

public class PatrolState : IEnemyState, IEnemyMovement
{
    private EnemyManager em;

    private int randomDes;
    private float distanceCorner;
    private float waitForAction = 0f;
    private int cornerDes = 0;
    private Quaternion targetRotation;

    public PatrolState(EnemyManager state)
    {
        em = state;
    }

    public void Enter()
    {
        waitForAction = 0f;
        cornerDes = 0;
        em.navMeshA.isStopped = true;
        em.isCalibrated = false;

        GetTargetDes();
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (waitForAction > 3f)
            return;
        waitForAction += Time.deltaTime;
    }

    public void FixedUpdate()
    {
        if (!em.isCalibrated && waitForAction > 3f)
            CalibrateVehicle();
        if (em.isCalibrated && waitForAction > 3f)
            MoveVehicle();
        CheckForTarget();
        CheckGround();
        DebugPath();
        Debug.Log(em.navMeshA.path.corners.Length);
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

    public void GetTargetDes()
    {
        randomDes = Random.Range(0, em.patrolPositions.Count);

        em.navMeshA.destination = em.patrolPositions[randomDes].position;

        TargetReachedCheck();

        //NavMeshPath path = new NavMeshPath();
        //NavMesh.CalculatePath(em.transform.position, em.target.position, NavMesh.AllAreas, path);

        //em.path = path;

        //foreach (var corner in em.path.corners)
        //{
        //    Debug.Log(corner);
        //}
    }

    public void DebugPath()
    {
        for (int i = 0; i < em.navMeshA.path.corners.Length; i++)
        {
            if (i < em.navMeshA.path.corners.Length - 1)
            {
                Debug.DrawLine(em.navMeshA.path.corners[i], em.navMeshA.path.corners[i + 1], Color.red);
            }
        }
    }

    public void CalibrateVehicle()
    {
        if (cornerDes >= em.navMeshA.path.corners.Length - 1)
            return;

        targetRotation = Quaternion.LookRotation(em.navMeshA.path.corners[cornerDes + 1] - em.transform.position);

        //Debug.Log(targetRotation);
        float rotate = em.rotationSpeed * Time.fixedDeltaTime;
        em.enemyRB.transform.rotation = Quaternion.RotateTowards(em.transform.rotation, targetRotation, rotate);

        Debug.Log(Quaternion.Angle(em.transform.rotation, targetRotation));
        if (Quaternion.Angle(em.transform.rotation, targetRotation) < 6f)
        {
            Debug.Log("Calibrated");
            em.enemyRB.transform.rotation = targetRotation;
            em.navMeshA.speed = 10f;
            em.isCalibrated = true;
            em.navMeshA.isStopped = false;
        }
    }

    public void MoveVehicle()
    {
        if (cornerDes >= em.navMeshA.path.corners.Length - 1)
            return;

        distanceCorner = Vector3.Distance(em.transform.position, em.navMeshA.path.corners[cornerDes + 1]);
        Debug.Log(em.navMeshA.path.corners[cornerDes + 1]);
        
        if (distanceCorner <= 5f)
        {
            em.navMeshA.speed = 3f;
        }
        else if (distanceCorner <= 15f)
        {
            em.navMeshA.speed = 8f;
        }
        
        //TargetReachedCheck();

        if (distanceCorner <= 2f)
        {
            cornerDes++;
            em.isCalibrated = false;
            em.navMeshA.isStopped = true;
        }
    }

    public void TargetReachedCheck()
    {
        float distanceTarget = Vector3.Distance(em.transform.position, em.patrolPositions[randomDes].position);
        if (distanceTarget < 2f)
        {
            waitForAction = 0f;
            cornerDes = 1;
            em.navMeshA.isStopped = true;
            em.isCalibrated = false;

            GetTargetDes();
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
