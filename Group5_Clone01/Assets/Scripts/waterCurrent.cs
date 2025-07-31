using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterCurrent : MonoBehaviour
{
    public Vector3 currentDirection = Vector3.back; 
    public float pushStrength = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(currentDirection.normalized * pushStrength, ForceMode.Force);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, currentDirection.normalized * 3f);
    }
}
