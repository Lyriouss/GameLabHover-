using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemyMovement
{
    void RotateVehicle();
    void RegulateMovement();
    void CheckForTarget();
    void CheckGround();
}

public class EnemyManager : MonoBehaviour, IEnemyStateMachine
{
    public Rigidbody enemyRB;
    public NavMeshAgent navMeshA;

    [Header("Movement")]
    public float decelerationTime = 1f;
    public float patrolSpeed = 8f;
    public float targetSpeed = 10f;
    public float rotationSpeed = 50f;
    public List<Transform> patrolPositions;

    [Header("Target Detection")]
    public LayerMask obstacleMask;
    public LayerMask targetMask;
    public float detectionRadius = 20f;
    public float timeBeforeChange = 3f;
    [HideInInspector] public Transform target;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundDistance = 2f;

    [Header("Time to stay still after Collision")]
    public float timeStayStill = 3f;

    [Header("In Script Values")]
    public Vector3 lastFacing;
    public bool isMoving;
    public bool isRotating;
    public bool needsToStop;
    public int randomDes;
    public float stayStillTimer;

    private EnemyStateMachine esm;

    private void Start()
    {
        navMeshA.updatePosition = false;
        navMeshA.updateRotation = false;

        randomDes = Random.Range(0, patrolPositions.Count);

        esm = new EnemyStateMachine();
        esm.ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        esm.Update();
    }

    private void FixedUpdate()
    {
        esm.FixedUpdate();
    }

    public void ChangeState(IEnemyState newState)
    {
        esm.ChangeState(newState);
    }

    private void OnCollisionEnter(Collision other)
    {
        stayStillTimer = 0f;
        enemyRB.angularVelocity = Vector3.zero;
        enemyRB.linearVelocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawRay(transform.position, transform.forward * detectionRadius);
    }
}
