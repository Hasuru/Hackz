using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;

public class FortuneWheelManager_Custom : NetworkBehaviour
{
    [SerializeField] private FortuneWheel fortuneWheel;
    [SerializeField] private Text resultLabel;
    [SerializeField] private Button spinButton;


    void Awake()
    {
            spinButton.onClick.AddListener(() =>
            {
                SpinServerRpc();
            });
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

        if(fortuneWheel.GetLatestResult().CompareTo("1") == 0)
        {
            Loader.LoadNetwork(Loader.Scene.PhishingTopicScene);
        }
        else if (fortuneWheel.GetLatestResult().CompareTo("2") == 0)
        {
            Loader.LoadNetwork(Loader.Scene.PhishingTopicScene);
        }
    }

}
