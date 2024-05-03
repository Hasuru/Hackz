using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FortuneWheelSceneManager : NetworkBehaviour
{
    public static FortuneWheelSceneManager Instance { get; private set; }

    [Header("User Interface")]
    [SerializeField] private GameObject loadingUI;

    private bool isSceneLoaded;


    private void Awake()
    {
        Instance = this;

        isSceneLoaded = false;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // When all of the clients have loaded
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += TopicScene_OnLoadEventCompleted;
        }

    }

    private void TopicScene_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        SetInitialSettingsClientRpc();
    }

    [ClientRpc]
    private void SetInitialSettingsClientRpc()
    {
        loadingUI.SetActive(false);
        isSceneLoaded = true;
    }
}
