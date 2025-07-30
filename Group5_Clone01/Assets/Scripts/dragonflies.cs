using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonflies : MonoBehaviour
{
    public float speed = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }


}
