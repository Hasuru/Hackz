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
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.SetString(categoryKey, category);
        PlayerPrefs.SetInt(timePUKey, timePU);
        PlayerPrefs.SetInt(cutPUKey, cutPU);
        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        _gameManager.SetPoints(PlayerPrefs.GetInt(scoreKey, 0));
        _gameManager.PowerUps[0] = PlayerPrefs.GetInt(timePUKey, 0);
        _gameManager.PowerUps[1] = PlayerPrefs.GetInt(cutPUKey, 0);

        var category = PlayerPrefs.GetString(categoryKey, "PASSWORD");
        if (category.Equals("PASSWORD"))
            _gameManager.SetCategory(CategoryType.PASSWORD);
        else
            _gameManager.SetCategory(CategoryType.PHISHING);
    }
}
