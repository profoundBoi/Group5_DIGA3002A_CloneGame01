using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 5f;
    public float patrolSpeed = 2f;

    [Header("Chase Settings")]
    public float chaseSpeed = 5f;
    public float detectionRange = 8f;
    public float viewAngle = 60f;
    public float loseSightTime = 3f;

    private Vector3 patrolCenter;
    private Vector3 targetPatrolPoint;
    private bool chasingPlayer = false;
    private float timeSinceLostPlayer = 0f;

    private Transform player;

    void Start()
    {
        patrolCenter = transform.position;
        PickNewPatrolPoint();
    }

    void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                return; 
            }
        }

        if (chasingPlayer)
        {
            if (CanSeePlayer())
            {
                ChasePlayer();
                timeSinceLostPlayer = 0f;
            }
            else
            {
                timeSinceLostPlayer += Time.deltaTime;

                if (timeSinceLostPlayer >= loseSightTime)
                {
                    chasingPlayer = false;
                    PickNewPatrolPoint();
                }
                else
                {
                    ChasePlayer(); 
                }
            }
        }
        else
        {
            Patrol();

            if (CanSeePlayer())
            {
                chasingPlayer = true;
            }
        }
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPatrolPoint, patrolSpeed * Time.deltaTime);
        Vector3 direction = targetPatrolPoint - transform.position;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        if (Vector3.Distance(transform.position, targetPatrolPoint) < 0.5f)
        {
            PickNewPatrolPoint();
        }
    }

    void PickNewPatrolPoint()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        targetPatrolPoint = patrolCenter + new Vector3(randomOffset.x, Random.Range(-1f, 1f), randomOffset.y);
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
            return false;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > viewAngle / 2f)
            return false;

       
        Ray ray = new Ray(transform.position, directionToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        Vector3 direction = player.position - transform.position;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); 
        }
    }

   
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftBoundary * detectionRange);
        Gizmos.DrawRay(transform.position, rightBoundary * detectionRange);
    }

}
