using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Services.Authentication;

public class MP_LobbyPlayerSingleUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject readyIcon;
    [SerializeField] private Button kickButton;

    private MP_PlayerData playerData;

    private void Awake()
    {
        kickButton.onClick.AddListener(() =>
        {
            // Kick player from from Netcode and Lobby
            HackzGameLobby.Instance.KickPlayer(playerData.playerId.ToString());
            HackzMultiplayer.Instance.KickPlayer(playerData.clientId);
        });
    }


    public void UpdatePlayer(Player player)
    {
        this.playerData = HackzMultiplayer.Instance.GetPlayerDataFromPlayerId(player.Id);

        playerNameText.text = playerData.playerUsername.ToString();
    }

    public void SetKickButtonVisible(bool visible)
    {
        kickButton.gameObject.SetActive(visible);
    }

    public void SetReadyIconVisible(bool visible)
    {
        readyIcon.gameObject.SetActive(visible);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
    }
}