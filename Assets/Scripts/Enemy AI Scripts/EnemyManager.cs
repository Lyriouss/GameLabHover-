using UnityEngine;
using UnityEngine.AI;

public interface IEnemyMovement
{
    void CheckForTarget();
    void CalibrateVehicle();
    void Accelerate();
    void CheckGround();
}

public class EnemyManager : MonoBehaviour, IEnemyStateMachine
{
    public Rigidbody enemyRB;

    [Header("Movement")]
    public float accelerationForce = 10f;
    public float patrolSpeed = 15f;
    public float targetSpeed = 25f;
    public float rotationSpeed = 20f;

    [Header("Target Detection")]
    public LayerMask obstacleMask;
    public LayerMask targetMask;
    public float detectionRadius = 20f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundDistance = 2f;

    public Transform target;
    public NavMeshPath path;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
