using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperController : MonoBehaviour
{
    [SerializeField] Sprite[] protocols;
    [SerializeField] Image currentProtocol;

    private int protocolIndex = 0;

    public void UpdateProtocol()
    {
        protocolIndex++;
        if (protocolIndex >= protocols.Length)
            protocolIndex = 0;
        
        currentProtocol.sprite = protocols[protocolIndex];
    }
}
