using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FortuneWheelManager_Custom : NetworkBehaviour
{
    public static FortuneWheelManager_Custom Instance { get; private set; }

    [Header("Fortune Wheel Stuff")]
    [SerializeField] private FortuneWheel fortuneWheel;
    [SerializeField] private Text resultLabel;
    [SerializeField] private Button spinButton;

    [Header("User Interface")]
    //[SerializeField] private GameObject loadingUI;

    private bool isSceneLoaded;

    void Awake()
    {
        Instance = this;

        isSceneLoaded = false;

        //loadingUI.SetActive(true);

        spinButton.onClick.AddListener(() =>
        {
                SpinServerRpc();
        });
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
        //loadingUI.SetActive(false);
        isSceneLoaded = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpinServerRpc()
    {
        if (IsServer) {
            StartCoroutine(SpinCoroutine());
        }
    }
    IEnumerator SpinCoroutine()
    {
        yield return StartCoroutine(fortuneWheel.StartFortune());

        if(resultLabel == null) yield break;
        resultLabel.text = fortuneWheel.GetLatestResult();

        print(fortuneWheel.GetLatestResult());
        yield return new WaitForSeconds(5);

        if(fortuneWheel.GetLatestResult().CompareTo("Password") == 0)
        {
            HackzMultiplayer.Instance.ChangeTopic("Password");
            Loader.LoadNetwork(Loader.Scene.DecryptorPassword);
        }
        else if (fortuneWheel.GetLatestResult().CompareTo("Phishing") == 0)
        {
            HackzMultiplayer.Instance.ChangeTopic("Phishing");
            Loader.LoadNetwork(Loader.Scene.PhishingTopicScene);
        }
    }

    public void OnDestroy()
    {
        // NetworkManager has a longer life cycle, so unsub from it
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= TopicScene_OnLoadEventCompleted;
    }
}
