using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameShowSceneManager : NetworkBehaviour
{
    public static GameShowSceneManager Instance { get; private set; }

    [Header("VISUAL STUFF")]
    [SerializeField] private GameObject loadingUI;

    private Dictionary<ulong, GameObject> playerObjects;

    public event EventHandler OnGameLoaded;


    private void Awake()
    {
        Instance = this;

        loadingUI.SetActive(true);
        playerObjects = new Dictionary<ulong, GameObject>();


        if (HackzGameLobby.Instance != null)
        {
            Destroy(HackzGameLobby.Instance.gameObject);
        }
    }


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // When all of the clients have loaded
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }

    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        Debug.Log("Loaded");
        SetInitialSettingsClientRpc();
    }

    [ClientRpc]
    private void SetInitialSettingsClientRpc()
    {
        loadingUI.SetActive(false);
       
        OnGameLoaded?.Invoke(this, EventArgs.Empty);
    }
}
