using UnityEngine;
using System.Collections.Generic;

public class GameRespawnManager : MonoBehaviour
{
    public static GameRespawnManager Instance;

    private Vector3 currentRespawnPoint;
    private List<GameObject> players = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterPlayer(GameObject player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);

            
            if (players.Count == 1)
                currentRespawnPoint = player.transform.position;
        }
    }

    public void SetRespawnPoint(Vector3 lastPosition)
    {
        currentRespawnPoint = lastPosition;
        Debug.Log("Shared checkpoint updated to: " + lastPosition);
    }

    /*public void RespawnAllPlayers()
    {
        foreach (var player in players)
        {
            player.transform.position = currentRespawnPoint; 
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;

          
        }

        Debug.Log("All players respawned.");
    }
    */
    public void RespawnAllPlayers()
    {
        float spacing = 2f; // How far apart each player should be
        int playerCount = players.Count;

        for (int i = 0; i < playerCount; i++)
        {
            GameObject player = players[i];
            if (player != null)
            {
                // Center players around the respawn point
                float offset = (i - (playerCount - 1) / 2f) * spacing;
                Vector3 spawnPosition = currentRespawnPoint + new Vector3(offset, 0f, 0f);

                player.transform.position = spawnPosition;

                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.velocity = Vector3.zero;
            }
        }

        Debug.Log("All players respawned side by side.");
    }



}
