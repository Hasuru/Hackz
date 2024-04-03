using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InstanceCleanUp : MonoBehaviour
{
    // Cleanup Network created objects that are not destroyed during after leaving multiplayer
    // in this case, NetworkManager, HackzMultiplayer and HackzGameLobby
    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (HackzMultiplayer.Instance != null)
        {
            Destroy(HackzMultiplayer.Instance.gameObject);
        }
        if (HackzGameLobby.Instance != null)
        {
            Destroy(HackzGameLobby.Instance.gameObject);
        }
    }
}
