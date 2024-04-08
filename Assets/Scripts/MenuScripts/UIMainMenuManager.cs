using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using SlimUI.ModernMenu;

public class UIMainMenuManager : MonoBehaviour
{
    private Animator CameraObject;

    [Header("MENUS")]
    [Tooltip("The Menu for when the MainMenu")]
    public GameObject mainMenu;
    [Tooltip("The Menu for when the PLAY button is clicked")]
    public GameObject playMenu;
    [Tooltip("The Menu for when the EXIT button is clicked")]
    public GameObject exitMenu;
    [Tooltip("Optional 4th Menu")]
    public GameObject extrasMenu;

    public enum Theme { custom1, custom2, custom3 };
    [Header("THEME SETTINGS")]
    public Theme theme;
    private int themeIndex;
    public ThemedUIData themeController;

    [Header("PANELS")]
    [Tooltip("The UI Panel that holds the CONTROLS window tab")]
    public GameObject PanelControls;
    [Tooltip("The UI Panel that holds the GAME window tab")]
    public GameObject PanelGame;


    // highlights in settings screen
    [Header("SETTINGS SCREEN")]
    [Tooltip("Highlight Image for when GAME Tab is selected in Settings")]
    public GameObject lineGame;
    [Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
    public GameObject lineControls;

    /*
    [Header("LOADING SCREEN")]
    [Tooltip("If this is true, the loaded scene won't load until receiving user input")]
    public bool waitForInput = true;
    public GameObject loadingMenu;
    [Tooltip("The loading bar Slider UI element in the Loading Screen")]
    public Slider loadingBar;
    public TMP_Text loadPromptText;
    public KeyCode userPromptKey;
    */

    [Header("SFX")]
    [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
    public AudioSource hoverSound;
    [Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
    public AudioSource sliderSound;
    [Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
    public AudioSource swooshSound;

    void Start()
    {
        CameraObject = transform.GetComponent<Animator>();

        playMenu.SetActive(false);
        exitMenu.SetActive(false);
        if (extrasMenu) extrasMenu.SetActive(false);
        mainMenu.SetActive(true);

        SetThemeColors();
    }

    void SetThemeColors()
    {
        switch (theme)
        {
            case Theme.custom1:
                themeController.currentColor = themeController.custom1.graphic1;
                themeController.textColor = themeController.custom1.text1;
                themeIndex = 0;
                break;
            case Theme.custom2:
                themeController.currentColor = themeController.custom2.graphic2;
                themeController.textColor = themeController.custom2.text2;
                themeIndex = 1;
                break;
            case Theme.custom3:
                themeController.currentColor = themeController.custom3.graphic3;
                themeController.textColor = themeController.custom3.text3;
                themeIndex = 2;
                break;
            default:
                Debug.Log("Invalid theme selected.");
                break;
        }
    }

    public void PlayMenu()
    {
        exitMenu.SetActive(false);
        if (extrasMenu) extrasMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void ResetMainMenu()
    {
        playMenu.SetActive(false);
        if (extrasMenu) extrasMenu.SetActive(false);
        exitMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadMultiplayerLobbyScene()
    {
        Loader.Load(Loader.Scene.LobbyListScene);
    }

    public void DisablePlayMenu()
    {
        playMenu.SetActive(false);
    }

    public void Position2()
    {
        DisablePlayMenu();
        CameraObject.SetFloat("Animate", 1);
    }

    public void Position1()
    {
        CameraObject.SetFloat("Animate", 0);
    }

    void DisablePanels()
    {
        PanelControls.SetActive(false);
        PanelGame.SetActive(false);

        lineGame.SetActive(false);
        lineControls.SetActive(false);
    }

    public void GamePanel()
    {
        DisablePanels();
        PanelGame.SetActive(true);
        lineGame.SetActive(true);
    }

    public void ControlsPanel()
    {
        DisablePanels();
        PanelControls.SetActive(true);
        lineControls.SetActive(true);
    }

    // Are You Sure - Quit Panel Pop Up
    public void AreYouSurePanel()
    {
        exitMenu.SetActive(true);
        if (extrasMenu) extrasMenu.SetActive(false);
        DisablePlayMenu();
    }

    public void ExtrasMenu()
    {
        playMenu.SetActive(false);
        if (extrasMenu) extrasMenu.SetActive(true);
        exitMenu.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
    }


    /*
    // Load Bar synching animation
    IEnumerator LoadAsynchronously(string sceneName)
    { // scene name is just the name of the current scene being loaded
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        mainCanvas.SetActive(false);
        loadingMenu.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .95f);
            loadingBar.value = progress;

            if (operation.progress >= 0.9f && waitForInput)
            {
                loadPromptText.text = "Press " + userPromptKey.ToString().ToUpper() + " to continue";
                loadingBar.value = 1;

                if (Input.GetKeyDown(userPromptKey))
                {
                    operation.allowSceneActivation = true;
                }
            }
            else if (operation.progress >= 0.9f && !waitForInput)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }*/


    public void PlayHover()
    {
        hoverSound.Play();
    }

    public void PlaySFXHover()
    {
        sliderSound.Play();
    }

    public void PlaySwoosh()
    {
        swooshSound.Play();
    }
}
