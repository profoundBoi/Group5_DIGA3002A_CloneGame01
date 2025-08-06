using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointLookAt : MonoBehaviour
{
    [Header("Transform to Flip")]
    public Transform spawnPoint;

    void Start()
    {
        if (spawnPoint != null)
        {
            
            spawnPoint.Rotate(0f, 180f, 0f);

           

            Debug.Log("Spawn point flipped!");
        }
        else
        {
            Debug.LogWarning("No spawnPoint assigned!");
        }
    }
}
