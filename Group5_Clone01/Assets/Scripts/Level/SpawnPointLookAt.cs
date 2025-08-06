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
            // Flip 180 degrees around the Y axis
            spawnPoint.Rotate(0f, 180f, 0f);

            // OR: Uncomment this line instead if you want to flip based on current forward
            // spawnPoint.rotation = Quaternion.LookRotation(-spawnPoint.forward);

            Debug.Log("Spawn point flipped!");
        }
        else
        {
            Debug.LogWarning("No spawnPoint assigned!");
        }
    }
}
