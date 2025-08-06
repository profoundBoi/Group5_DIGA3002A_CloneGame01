using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<Transform> startingPoints;
    [SerializeField]
    private List<LayerMask> playerLayers;

    private PlayerInputManager playerInputManager;

    [SerializeField]
    private TetherManager tetherManager;

    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }


    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        Transform playerParent = player.transform.parent;
        Transform spawnPoint = startingPoints[players.Count - 1];

        playerParent.position = spawnPoint.position;
        playerParent.rotation = spawnPoint.rotation;

        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);
        playerParent.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        playerParent.GetChild(2).GetComponent<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
        playerParent.GetComponentInChildren<InputHandler>().horizontal = player.actions.FindAction("Look");
        playerParent.GetComponentInChildren<InputHandlerr>().hori = player.actions.FindAction("Lookaim");

        if (players.Count == 1)
        {
            // First player — keep AudioListener enabled
            playerParent.GetComponentInChildren<AudioListener>().enabled = true;
        }
        else
        {
            // All other players — disable AudioListener
            playerParent.GetComponentInChildren<AudioListener>().enabled = false;
        }

    }
}