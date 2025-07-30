using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public float moveSpeed = 2f;        // Speed of movement
    public float moveRange = 3f;        // Total distance to move from center

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2) - moveRange;
        transform.position = startPosition + Vector3.right * offset;
    }
}
