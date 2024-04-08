using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InGameUI : NetworkBehaviour
{
    [Header("CANVAS")]
    [SerializeField] private GameObject hud;

    [Header("DATA STUFF")]
    [SerializeField] private TextMeshProUGUI testText;

    private bool isGameLoaded;


    private void Awake()
    {
        isGameLoaded = false;

        hud.SetActive(true);

        //Listen for the event of GameManager finishing loading
        GameManager.Instance.OnGameLoaded += GameManager_OnGameLoaded;
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        
    }

    private void GameManager_OnGameLoaded(object sender, System.EventArgs e)
    {
        isGameLoaded = true;
        SetHudIcons();
    }

    private void SetHudIcons()
    {
        testText.text = HackzMultiplayer.Instance.GetPlayerName();

    }
}
