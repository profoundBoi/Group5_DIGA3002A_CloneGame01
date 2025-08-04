using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRange = 3f;

    private Vector3 startPosition;
    private Vector3 previousPosition;

    void Start()
    {
        startPosition = transform.position;
        previousPosition = startPosition;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2) - moveRange;
        Vector3 newPosition = startPosition + Vector3.right * offset;

        Vector3 direction = newPosition - previousPosition;
        if (direction != Vector3.zero)
        {
            transform.forward = direction.normalized;
        }

        transform.position = newPosition;
        previousPosition = newPosition;
    }
}
