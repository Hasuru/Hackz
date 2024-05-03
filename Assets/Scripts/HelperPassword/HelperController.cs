using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelperController : NetworkBehaviour
{
    [SerializeField] GameObject decryptorPage;
    [SerializeField] GameObject helperPage;
    [SerializeField] Image currentProtocol;
    [SerializeField] Sprite[] protocols;

    [Header("Related UI")]
    [SerializeField] private GameObject clientUI;
    [SerializeField] private GameObject loadingUI;

    private int protocolIndex = 0;

    private void Awake()
    {
        loadingUI.SetActive(true);
        clientUI.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // When all of the clients have loaded
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += DecryptorHelper_OnLoadEventCompleted;
        }

    }

    private void DecryptorHelper_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        SetInitialSettingsClientRpc();
    }

    [ClientRpc]
    private void SetInitialSettingsClientRpc()
    {
        if (!IsServer)
        {
            loadingUI.SetActive(false);
            clientUI.SetActive(true);
        }
    }

    void Update()
    {
        if (!IsServer)
        {
            if (Input.GetKeyDown(KeyCode.D))
                UpdateProtocol(1);
            else if (Input.GetKeyDown(KeyCode.A))
                UpdateProtocol(-1);
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                helperPage.SetActive(false);
                decryptorPage.SetActive(true);
            }
        }   
    }

    public void UpdateProtocol(int direction)
    {
        protocolIndex += direction;

        if (protocolIndex >= protocols.Length)
            protocolIndex = 0;
        if (protocolIndex < 0)
            protocolIndex = protocols.Length - 1;
        
        currentProtocol.sprite = protocols[protocolIndex];
    }

    public void OnDestroy()
    {
        // NetworkManager has a longer life cycle, so unsub from it
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= DecryptorHelper_OnLoadEventCompleted;
    }
}
