using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_ConnectingUI : MonoBehaviour
{
    private void Start()
    {
        HackzMultiplayer.Instance.OnTryingToJoinGame += HackzMultiplayer_OnTryingToJoinGame;
        HackzMultiplayer.Instance.OnFailedToJoinGame += HackzMultiplayer_OnFailedToJoinGame;

        Hide();
    }

    private void HackzMultiplayer_OnTryingToJoinGame(object sender, EventArgs e)
    {
        Show();
    }

    private void HackzMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // Unsubscribe from the events we subbed to in Start()
    // We do this because these UI objects and the HackzMultiplayer have different lifecycles
    //
    private void OnDestroy()
    {
        HackzMultiplayer.Instance.OnTryingToJoinGame -= HackzMultiplayer_OnTryingToJoinGame;
        HackzMultiplayer.Instance.OnFailedToJoinGame -= HackzMultiplayer_OnFailedToJoinGame;
    }
}
