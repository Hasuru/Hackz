using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Loader
{
    public enum Scene
    {
        MainMenuScene, //Main menu of the game
        LoadingScene, //Scene for when the game is loading
        LobbyListScene, //Scene for lobby creation and selection
        LobbyScene, //Scene for the pre-game lobby
        TopicWheelScene, //Scene for the Topic Wheel
        PhishingTopicScene
    }


    private static Scene targetScene;



    public static void Load(string scene)
    {
        if (scene != null && scene != "")
        {
            Scene targetScene = (Scene)Enum.Parse(typeof(Scene), scene);
            Load(targetScene);
        }
    }

    public static void Load(Scene targetScene)
    {
        // Load the scene we want to access
        Loader.targetScene = targetScene;

        // While the target Scene is Loading, show LoadingScene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
