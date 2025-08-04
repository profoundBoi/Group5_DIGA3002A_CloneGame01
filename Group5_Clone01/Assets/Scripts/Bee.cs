using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public float patrolRadius = 5f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 8f;
    public float viewAngle = 60f;

    private Vector3 patrolCenter;
    private Vector3 targetPatrolPoint;
    private bool chasingPlayer = false;
    private Transform player;

    void Start()
    {
        patrolCenter = transform.position;
        PickNewPatrolPoint();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        if (!chasingPlayer)
        {
            Patrol();

            if (CanSeePlayer())
            {
                chasingPlayer = true;
            }
        }
        else
        {
            ChasePlayer();
        }
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPatrolPoint, patrolSpeed * Time.deltaTime);

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

       
        return true;
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        transform.LookAt(player);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            Destroy(gameObject);
        }
    }
}
