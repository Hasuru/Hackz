using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This Class serves for handling everything Lobby related, from Relay to lobby disconnects and joins
 * 
 */
public class HackzGameLobby : NetworkBehaviour
{
    public static HackzGameLobby Instance { get; private set; }

    private const string KEY_RELAY_CODE = "RelayJoinCode";

    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float listLobbiesTimer;
    private float lobbyPollTimer = 1;

    // Events for before joining lobby
    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;
    public event EventHandler OnJoinStarted;
    public event EventHandler OnQuickJoinFailed;
    public event EventHandler OnJoinFailed;

    // Events for after joining lobby
    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

    // Event for keeping up with the lobby list
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }




    private void Awake()
    {
        Instance = this;

        // We will need data from here in other scenes, so dont destroy this object
        DontDestroyOnLoad(gameObject);

        // Automatically initialize player into the unity services
        InitializeUnityAuthentication();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        //HandlePeriodicListLobbies(); // Disabled Auto Refresh for testing with multiple builds
        HandleLobbyPolling();
    }


    public async void InitializeUnityAuthentication()
    {
        // We only want to initialize once, otherwise error, even though we create and destroy the object every time we enter the lobby
        // So if not already initialized, then initialize
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            // Assign an id for each player even on different builds/same PC
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0, 10000).ToString());

            await UnityServices.InitializeAsync(initializationOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }



    /**
     * Method for sending heartbeat ping to the lobby service, so that the lobby doesnt become inactive
     */
    private async void HandleLobbyHeartbeat()
    {
        // We only want to run this in the host
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                Debug.Log("Heartbeat");
                // Ping for the lobby
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    // For Updating the lobby UIpreriodically with the players every lobbyPoolTimer
    private async void HandleLobbyPolling()
    {
        if (joinedLobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0f)
            {
                float lobbyPollTimerMax = 3f;
                lobbyPollTimer = lobbyPollTimerMax;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                if (!IsPlayerInLobby())
                {
                    joinedLobby = null;
                }
            }
        }
    }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // This player is in this lobby
                    return true;
                }
            }
        }
        return false;
    }

    public void HandleLobbyListRefresh()
    {
        if (joinedLobby == null && UnityServices.State == ServicesInitializationState.Initialized
            && AuthenticationService.Instance.IsSignedIn
            && SceneManager.GetActiveScene().name == Loader.Scene.LobbyListScene.ToString())
        {
            ListLobbies();
        }
    }


    private void HandlePeriodicListLobbies()
    {
        if (joinedLobby == null && UnityServices.State == ServicesInitializationState.Initialized
            && AuthenticationService.Instance.IsSignedIn
            && SceneManager.GetActiveScene().name == Loader.Scene.LobbyListScene.ToString())
        {
            listLobbiesTimer -= Time.deltaTime;
            if (listLobbiesTimer <= 0f)
            {
                float listLobbiesTimerMax = 15f;
                listLobbiesTimer = listLobbiesTimerMax;
                ListLobbies();
            }
        }
    }

    /**
     * Method for returning true if THIS is the host
     */
    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }



    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                // We'll list only lobbies with available slots (more than 0 slots)
                Count = 6,
                Filters = new List<QueryFilter> {
                  new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
             }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
            {
                lobbyList = queryResponse.Results
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /**
     *  Method for creating the realy in order to make p2p communication in this multiplayer setting
     * 
     */
    private async Task<string> RelayAllocation()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(HackzMultiplayer.MAX_PLAYER_AMOUNT - 1); // -1 which is the host
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            return await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);


        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }

    // An allocation created via a join code.
    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
            return await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }


    /**
     * Method for creating and hosting a game Lobby
     */
    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            // Keep created lobby as the joined Lobby
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, HackzMultiplayer.MAX_PLAYER_AMOUNT, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
            });

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            // Relay for p2p multiplayer
            string relayJoinCode = await RelayAllocation();

            // Other players need the code created by Relay to join session
            // Because of this, we keep it stored in the lobby data
            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
                }
            });

            // Start Hosting and go to LobbyScene, where the main lobby will be
            HackzMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.LobbyScene);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    /**
     * Method for quickly joining any available lobby
     */
    public async void QuickJoinLobby()
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            // Join a lobby with Relay
            string relayJoinCode = joinedLobby.Data[KEY_RELAY_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            // Enter the game lobby as a client
            HackzMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnQuickJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    /**
     * Method for joining a lobby via code
     */
    public async void JoinLobbyByCode(string lobbyCode)
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            // Join a lobby with Relay
            string relayJoinCode = joinedLobby.Data[KEY_RELAY_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            // Enter the game lobby as a client
            HackzMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    /**
     * Method for joining a lobby by clicking on it
     */
    public async void JoinLobbyById(string lobbyID)
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID);

            // Join a lobby with Relay
            string relayJoinCode = joinedLobby.Data[KEY_RELAY_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            // Enter the game lobby as a client
            HackzMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }


    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public override void OnDestroy()
    {
        LeaveLobby();

        // Always invoke the base 
        base.OnDestroy();
    }

    void OnApplicationQuit()
    {
        LeaveLobby();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
