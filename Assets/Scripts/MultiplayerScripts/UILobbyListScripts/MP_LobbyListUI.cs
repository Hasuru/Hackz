using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class MP_LobbyListUI : MonoBehaviour
{
    public static MP_LobbyListUI Instance { get; private set; }



    [SerializeField] private Transform lobbySingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinLobbyButton;
    [SerializeField] private Button joinByCodeButton;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField playerUsernameField;

    [SerializeField] private MP_LobbyCreateUI lobbyCreateUI;


    private void Awake()
    {
        Instance = this;

        // Hide the template for the lobbies
        lobbySingleTemplate.gameObject.SetActive(false);

        // Button listeners
        mainMenuButton.onClick.AddListener(() => {
            HackzGameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        refreshButton.onClick.AddListener(() => {
            HackzGameLobby.Instance.HandleLobbyListRefresh();
        });

        createLobbyButton.onClick.AddListener(() => {
            lobbyCreateUI.Show();
            Hide();
        });

        quickJoinLobbyButton.onClick.AddListener(() => {
            HackzGameLobby.Instance.QuickJoinLobby();
        });

        joinByCodeButton.onClick.AddListener(() => {
            HackzGameLobby.Instance.JoinLobbyByCode(joinCodeInputField.text);
        });
    }

    private void Start()
    {
        // Update the username of the player
        playerUsernameField.text = HackzMultiplayer.Instance.GetPlayerName();
        playerUsernameField.onValueChanged.AddListener((string newText) => {
            HackzMultiplayer.Instance.SetPlayerName(newText);
        });

        HackzGameLobby.Instance.OnLobbyListChanged += HackzGameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void HackzGameLobby_OnLobbyListChanged(object sender, HackzGameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in container)
        {
            if (child == lobbySingleTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbySingleTransform = Instantiate(lobbySingleTemplate, container);
            lobbySingleTransform.gameObject.SetActive(true);

            MP_LobbyListSingleUI lobbyListSingleUI = lobbySingleTransform.GetComponent<MP_LobbyListSingleUI>();
            lobbyListSingleUI.SetLobby(lobby);
        }
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        HackzGameLobby.Instance.OnLobbyListChanged -= HackzGameLobby_OnLobbyListChanged;
    }
}