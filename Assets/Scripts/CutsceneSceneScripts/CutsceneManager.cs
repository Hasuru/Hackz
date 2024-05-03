using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : NetworkBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    [Header("Camera Animator")]
    [SerializeField] private Animator animator;

    [Header("User Interface")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject loadingUI;

    private bool isGameLoaded;
    private int dialogueCount;

    private void Awake()
    {
        Instance = this;

        dialogueCount = 0;
        isGameLoaded = false;

        loadingUI.SetActive(true);
        dialogueUI.SetActive(false);

        // Destroy the Lobby instance, as it is no longer needed I guess
        if (HackzGameLobby.Instance != null)
        {
            Destroy(HackzGameLobby.Instance.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGameLoaded && IsServer)
        {
            ChangeDialogueTextClientRpc();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // When all of the clients have loaded
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += CutsceneManager_OnLoadEventCompleted;
        }

    }

    private void CutsceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        SetInitialSettingsClientRpc();
    }

    [ClientRpc]
    private void SetInitialSettingsClientRpc()
    {
        StartCoroutine(PositionInitialCamera());
    }


    IEnumerator PositionInitialCamera()
    {
        loadingUI.SetActive(false);

        animator.SetTrigger("Cutscene");

        // Get the duration of the animation
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float animationDuration = 0;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "CutsceneCamPos1")
            {
                animationDuration = clip.length;
                break;
            }
        }
       
        // Wait for the duration of the animation
        yield return new WaitForSeconds(animationDuration);

        StartDialogue();
    }

    private void StartDialogue()
    {
        dialogueText.text = "Hellow! I'm your host for today's Show, mister Purrsky, and welcome to Hackz, the Quiz Show!";

        dialogueUI.SetActive(true);
        isGameLoaded = true;
    }

    [ClientRpc]
    private void ChangeDialogueTextClientRpc()
    {
        switch (dialogueCount)
        {
            case 0: 
                dialogueText.text = "Shall we start? I'll be counting the points you acquire along the game, and you'll be able to see them at any time in your screen.";
                dialogueCount++;
                break;
            case 1:
                dialogueText.text = "Well then, let's do this! Good Luck!";
                dialogueCount++;
                break;
            case 2:
                dialogueUI.SetActive(false);
                dialogueCount++;
                StartCoroutine(PositionTransitionCamera());
                break;
            default:
                break;
        }
    }

    IEnumerator PositionTransitionCamera()
    {
        animator.SetTrigger("Cutscene2");

        // Get the duration of the animation
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float animationDuration = 0;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "CutsceneCamPos2")
            {
                animationDuration = clip.length;
                break;
            }
        }

        // Wait for the duration of the animation
        yield return new WaitForSeconds(animationDuration + 0.5f);

        Loader.LoadNetwork(Loader.Scene.TopicWheelScene);
    }

    public void OnDestroy()
    {
        // NetworkManager has a longer life cycle, so unsub from it
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= CutsceneManager_OnLoadEventCompleted;
    }
}
