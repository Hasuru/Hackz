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

    public event EventHandler OnGameLoaded;

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
        loadingUI.SetActive(false);
        StartCoroutine(PositionInitialCamera());
    }

    IEnumerator PositionInitialCamera()
    {
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
        dialogueText.text = "First Text";

        dialogueUI.SetActive(true);
        isGameLoaded = true;
    }

    [ClientRpc]
    private void ChangeDialogueTextClientRpc()
    {
        switch (dialogueCount)
        {
            case 0: 
                dialogueText.text = "Second Text";
                dialogueCount++;
                break;
            case 1:
                dialogueText.text = "Third Text";
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

        Loader.Load(Loader.Scene.TopicWheelScene);
    }
}
