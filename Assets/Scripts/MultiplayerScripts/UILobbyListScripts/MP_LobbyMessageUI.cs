using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MP_LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        HackzMultiplayer.Instance.OnFailedToJoinGame += HackzMultiplayer_OnFailedToJoinGame;
        HackzGameLobby.Instance.OnCreateLobbyStarted += HackzGameLobby_OnCreateLobbyStarted;
        HackzGameLobby.Instance.OnCreateLobbyFailed += HackzGameLobby_OnCreateLobbyFailed;
        HackzGameLobby.Instance.OnJoinStarted += HackzGameLobby_OnJoinStarted;
        HackzGameLobby.Instance.OnJoinFailed += HackzGameLobby_OnJoinFailed;
        HackzGameLobby.Instance.OnQuickJoinFailed += HackzGameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void HackzGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("No available lobby to Quick Join found!");
    }

    private void HackzGameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to join lobby!");
    }

    private void HackzGameLobby_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Trying to join lobby..."); ;
    }

    private void HackzGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create Lobby!");
    }

    private void HackzGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating Lobby...");
    }

    private void HackzMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        // Because connection failed, we show the user the reason for disconnecting through this UI messageText
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // Unsubscribe from the events
    // We do this because these UI objects and the HackzMultiplayer have different lifecycles
    //
    private void OnDestroy()
    {
        HackzMultiplayer.Instance.OnFailedToJoinGame -= HackzMultiplayer_OnFailedToJoinGame;
        HackzGameLobby.Instance.OnCreateLobbyStarted -= HackzGameLobby_OnCreateLobbyStarted;
        HackzGameLobby.Instance.OnCreateLobbyFailed -= HackzGameLobby_OnCreateLobbyFailed;
        HackzGameLobby.Instance.OnJoinStarted -= HackzGameLobby_OnJoinStarted;
        HackzGameLobby.Instance.OnJoinFailed -= HackzGameLobby_OnJoinFailed;
        HackzGameLobby.Instance.OnQuickJoinFailed -= HackzGameLobby_OnQuickJoinFailed;
    }
}
