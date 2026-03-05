using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemyMovement
{
    void CheckForTarget();
    void CalibrateVehicle();
    void MoveVehicle();
    void CheckGround();
}

public class EnemyManager : MonoBehaviour, IEnemyStateMachine
{
    public Rigidbody enemyRB;
    public NavMeshAgent navMeshA;

    [Header("Movement")]
    public float accelerationForce = 10f;
    public float patrolSpeed = 15f;
    public float targetSpeed = 25f;
    public float rotationSpeed = 20f;
    public List<Transform> patrolPositions;

    [Header("Target Detection")]
    public LayerMask obstacleMask;
    public LayerMask targetMask;
    public float detectionRadius = 20f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundDistance = 2f;

    [HideInInspector] public Transform target;
    //[HideInInspector] public NavMeshPath path;
    //[HideInInspector] public int cornerDes = 1;
    //[HideInInspector] public Transform cornerDes;
    public bool isCalibrated = false;

    private EnemyStateMachine esm;

    private void Start()
    {
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

    IEnumerator WaitForNavigation()
    {
        yield return new WaitForSeconds(3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        esm.ChangeState(new PatrolState(this));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawRay(transform.position, transform.forward * 20f);
    }
}
