using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperController : MonoBehaviour
{
    [SerializeField] Sprite[] protocols;
    [SerializeField] Image currentProtocol;

    private int protocolIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            UpdateProtocol(1);
        else if (Input.GetKeyDown(KeyCode.A))
            UpdateProtocol(-1);
    }

    public void UpdateProtocol(int direction)
    {
        protocolIndex += direction;

        if (protocolIndex >= protocols.Length)
            protocolIndex = 0;
        if (protocolIndex < 0)
            protocolIndex = protocols.Length - 1;
        
        currentProtocol.sprite = protocols[protocolIndex];
    }
}
