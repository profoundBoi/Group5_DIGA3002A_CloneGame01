using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    private Collider checkpointColi;

    void Awake()
    {
        checkpointColi = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider coli)
    {
        if (coli.CompareTag("Player"))
        {

            GameRespawnManager.Instance.SetRespawnPoint(coli.transform.position);
        }
    }

    void OnTriggerExit(Collider coli)
    {
        if (coli.CompareTag("Player"))
        {

           checkpointColi.enabled = false;
            Debug.Log("Checkpoint disabled after both players exited.");
                
        }
    }
}
