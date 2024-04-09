using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class MP_LobbyUI : MonoBehaviour
{
    public static MP_LobbyUI Instance { get; private set; }

    [SerializeField] private Button readyButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button refreshLobby;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;


    private void Awake()
    {
        Instance = this;

        // Hide the player Template
        playerSingleTemplate.gameObject.SetActive(false);

        readyButton.onClick.AddListener(() =>
        {
            readyButton.gameObject.SetActive(false);
            LobbyReady.Instance.SetPlayerReady();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            HackzGameLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        refreshLobby.onClick.AddListener(() =>
        {
            UpdateLobby(HackzGameLobby.Instance.GetLobby());
        });
    }

    private void Start()
    {
        Lobby lobby = HackzGameLobby.Instance.GetLobby();

        HackzGameLobby.Instance.OnJoinedLobby += Update_OnJoinedLobby;
        HackzGameLobby.Instance.OnJoinedLobbyUpdate += Update_OnJoinedLobby;
        LobbyReady.Instance.OnReadyChanged += Update_OnReadyChanged;
    }

    private void Update_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdateLobby(HackzGameLobby.Instance.GetLobby());
    }

    private void Update_OnJoinedLobby(object sender, HackzGameLobby.LobbyEventArgs e)
    {
        UpdateLobby(HackzGameLobby.Instance.GetLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();

        if (playerSingleTemplate != null && container != null)
        {
            foreach (Player player in lobby.Players)
            {
                if (player != null && HackzMultiplayer.Instance.GetPlayerDataFromPlayerId(player.Id).playerId.ToString().Length > 0)
                {
                    Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
                    playerSingleTransform.gameObject.SetActive(true);
                    MP_LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<MP_LobbyPlayerSingleUI>();

                    // Make the kick button only available to the host/server
                    lobbyPlayerSingleUI.SetKickButtonVisible(
                        NetworkManager.Singleton.IsServer &&
                        player.Id != AuthenticationService.Instance.PlayerId // Don't allow self kick
                    );

                    // Set the ready icon
                    MP_PlayerData playerData = HackzMultiplayer.Instance.GetPlayerDataFromPlayerId(player.Id);
                    lobbyPlayerSingleUI.SetReadyIconVisible(
                        LobbyReady.Instance.IsPlayerReady(playerData.clientId)
                    );

                    lobbyPlayerSingleUI.UpdatePlayer(player);
                }
            }

            lobbyNameText.text = "Lobby Name:  " + lobby.Name;
            lobbyCodeText.text = "Lobby Code:  " + lobby.LobbyCode;
            playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

            Show();
        }
    }

    private void ClearLobby()
    {
        if (container != null)
            foreach (Transform child in container)
            {
                if (child == playerSingleTemplate) continue;
                Destroy(child.gameObject);
            }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}