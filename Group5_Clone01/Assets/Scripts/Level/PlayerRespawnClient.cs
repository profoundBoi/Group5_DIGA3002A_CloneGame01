using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnClient : MonoBehaviour
{
    void Start()
    {
        GameRespawnManager.Instance.RegisterPlayer(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillZone"))
        {
            GameRespawnManager.Instance.RespawnAllPlayers();
        }
    }
}


