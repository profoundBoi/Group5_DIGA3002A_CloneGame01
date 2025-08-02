using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackScript : MonoBehaviour
{
    public float KnockbackForce = 5f;

    private void OnCollisionEnter(Collision coli)
    {
        if (coli.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = coli.rigidbody;
            Vector3 hitDirection = (coli.transform.position - transform.position).normalized;
            playerRb.AddForce(hitDirection * KnockbackForce, ForceMode.Impulse);


        }
    }
}
