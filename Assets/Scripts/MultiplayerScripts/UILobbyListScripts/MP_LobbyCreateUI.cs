using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MP_LobbyCreateUI : MonoBehaviour
{
    public static MP_LobbyCreateUI Instance { get; private set; }


    [SerializeField] private Button closeButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Button lobbyNameButton;
    [SerializeField] private Button publicPrivateButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI publicPrivateText;

    [SerializeField] private MP_LobbyListUI lobbyListUI;


    private string lobbyName;
    private bool isPrivate;


    private void Awake()
    {
        Instance = this;

        createButton.onClick.AddListener(() => {
            HackzGameLobby.Instance.CreateLobby(
                lobbyName,
                isPrivate
            );
        });
        
        lobbyNameButton.onClick.AddListener(() => {
            UI_InputWindow.Show_Static("Lobby", lobbyName, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ .,-", 20,
            () => {
                // Cancel
            },
            (string lobbyName) => {
                this.lobbyName = lobbyName;
                UpdateText();
            });
        });

        publicPrivateButton.onClick.AddListener(() => {
            isPrivate = !isPrivate;
            UpdateText();
        });

        closeButton.onClick.AddListener(() => {
            lobbyListUI.Show();
            Hide();
        });
    }

    private void Start()
    {
        Hide();
    }


    private void UpdateText()
    {
        lobbyNameText.text = lobbyName;
        publicPrivateText.text = isPrivate ? "Private" : "Public";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        lobbyName = "My Lobby";
        isPrivate = false;

        UpdateText();
    }

}