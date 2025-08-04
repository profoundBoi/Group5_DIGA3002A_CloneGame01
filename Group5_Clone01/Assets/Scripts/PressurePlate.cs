using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform platformToRaise;  
    public float raiseHeight = 5f;   
    public float raiseSpeed = 2f;
    public float holdTime = 2f;  

    private Vector3 initialPlatformPosition;
    private Vector3 raisedPosition;
    private bool playerOnPlate = false;
    private float timer = 0f;

    void Start()
    {
        if (platformToRaise == null)
        {
            Debug.LogError("Platform to raise is not assigned!");
            return;
        }

        initialPlatformPosition = platformToRaise.position;
        raisedPosition = initialPlatformPosition + Vector3.up * raiseHeight;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlate = true;
            timer = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlate = false;
            timer = 0f;
        }
    }

    void Update()
    {
        if (platformToRaise == null) return;

        if (playerOnPlate)
        {
            platformToRaise.position = Vector3.MoveTowards(platformToRaise.position, raisedPosition, raiseSpeed * Time.deltaTime);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= holdTime)
            {
                platformToRaise.position = Vector3.MoveTowards(platformToRaise.position, initialPlatformPosition, raiseSpeed * Time.deltaTime);
            }
        }
    }
}
