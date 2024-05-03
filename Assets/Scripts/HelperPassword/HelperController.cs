using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperController : MonoBehaviour
{
    [SerializeField] GameObject decryptorPage;
    [SerializeField] GameObject helperPage;
    [SerializeField] Image currentProtocol;
    [SerializeField] Sprite[] protocols;

    private int protocolIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            UpdateProtocol(1);
        else if (Input.GetKeyDown(KeyCode.A))
            UpdateProtocol(-1);
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            helperPage.SetActive(false);
            decryptorPage.SetActive(true);
        }
            
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
