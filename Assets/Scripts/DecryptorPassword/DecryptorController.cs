using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DecryptorController : NetworkBehaviour
{
    [HideInInspector] Password password;
    [SerializeField] TMP_InputField passwordText;
    [SerializeField] Image[] protocols = new Image[3];

    [Header("Related UI")]
    [SerializeField] private GameObject serverUI;
    [SerializeField] private GameObject loadingUI;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Object[] objs = Resources.LoadAll<Password>("Passwords/");
        password = (Password)objs[(int)UnityEngine.Random.Range(0, objs.Length-1)];
        passwordText.text = password.InitialPassword;

        for (int i = 0; i < protocols.Length; i++)
        {
            Color tempColor = password.Protocols[i];
            tempColor.a = 1f;
            protocols[i].color = tempColor;
        }
    }

    private void Awake()
    {
        loadingUI.SetActive(true);
        serverUI.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // When all of the clients have loaded
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += DecryptorController_OnLoadEventCompleted;
        }

    }

    private void DecryptorController_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        loadingUI.SetActive(false);
        serverUI.SetActive(true);
    }

    public void SubmitPassword()
    {
        if (VerifyCurrentPassword())
            Debug.Log("Correct Password");
        else
            Debug.Log("Wrong Password");
    }

    public void ScanPassword() { passwordText.text = password.ScannedPassword; }
    public void ResetPassword() { passwordText.text = password.InitialPassword; }
    bool VerifyCurrentPassword() { return passwordText.text.Equals(password.FinalPassword); }
    public void InvertPassword()
    {
        string curPassword = passwordText.text;
        char[] charArray = curPassword.ToCharArray();
        Array.Reverse(charArray);
        passwordText.text = new string(charArray);
    }

    public void UpdatePassword(string s) 
    {
        passwordText.text = s;
    }

    public void OnDestroy()
    {
        // NetworkManager has a longer life cycle, so unsub from it
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= DecryptorController_OnLoadEventCompleted;
    }
}
