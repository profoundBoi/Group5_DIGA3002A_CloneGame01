using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    private Collider checkpointColi;
    private HashSet<GameObject> playersInside = new HashSet<GameObject>();
    private int totalPlayers = 2; 

    void Awake()
    {
        checkpointColi = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider coli)
    {
        if (coli.CompareTag("Player"))
        {
            playersInside.Add(coli.gameObject);
            GameRespawnManager.Instance.SetRespawnPoint(coli.transform.position);
        }
    }

    void OnTriggerExit(Collider coli)
    {
        if (coli.CompareTag("Player"))
        {
            playersInside.Remove(coli.gameObject);

                if (playersInside.Count == 0)
                {
                   checkpointColi.enabled = false;
                   Debug.Log("Checkpoint disabled after both players exited.");
                }
        }
    }
}
