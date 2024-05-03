using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePreferencesManager : MonoBehaviour
{
    const string scoreKey = "score";
    const string categoryKey = "category";
    const string timePUKey = "timePowerUp";
    const string cutPUKey = "cutPowerUp";

    [SerializeField] Q_GameManager _gameManager;

    public void SavePrefs(int score, string category, int timePU, int cutPU)
    {
        Debug.Log("score:" + score + " / " + "category:" + category + " / " + "score:" + timePU + " / " + "score:" + cutPU);
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.SetString(categoryKey, category);
        PlayerPrefs.SetInt(timePUKey, timePU);
        PlayerPrefs.SetInt(cutPUKey, cutPU);
        PlayerPrefs.Save();
        Debug.Log("saved");
    }

    public void LoadPrefs()
    {
        var points = PlayerPrefs.GetInt(scoreKey, 0);
        _gameManager.SetPoints(points);

        var timepowerup = PlayerPrefs.GetInt(timePUKey, 0);
        _gameManager.SetTimePU(timepowerup);

        var cutpowerup = PlayerPrefs.GetInt(cutPUKey, 0);
        _gameManager.SetCutPU(cutpowerup);

        var category = PlayerPrefs.GetString(categoryKey, "PASSWORD");
        if (category.Equals("PASSWORD"))
            _gameManager.SetCategory(CategoryType.PASSWORD);
        else
            _gameManager.SetCategory(CategoryType.PHISHING);
        Debug.Log("loaded\nscore:" + points + "/ time:" + timepowerup + "/ cut:" + cutpowerup + " category:" + category);
    }
}
