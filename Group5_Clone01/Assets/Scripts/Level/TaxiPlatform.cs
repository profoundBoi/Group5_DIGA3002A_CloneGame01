using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField]
    private float moveSpeed;
    private Vector3 platformMotion;
    private Transform targetPosition; //Between point A and B
    private Vector3 lastPosition;
    Rigidbody rb;
    private List<Rigidbody> players = new List<Rigidbody>();
    void Start()
    {
        targetPosition = pointB;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

        platformMotion = transform.position - lastPosition;
        lastPosition = transform.position;

        //Target Switching

        if(Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            targetPosition = targetPosition == pointA ? pointB : pointA;
        }

        foreach (Rigidbody player in players)
        {
            player.MovePosition(player.position + platformMotion);
        }

    }

    private void OnCollisionEnter(Collision coli)
    {
        if (coli.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = coli.rigidbody;
            if (playerRb != null && !players.Contains(playerRb))
            {
                players.Add(playerRb);
            }
        }
    }

    private void OnCollisionExit(Collision coli)
    {
        if (coli.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = coli.rigidbody;
            if (playerRb != null && players.Contains(playerRb))
            {
                players.Remove(playerRb);
            }
        }
    }


}
