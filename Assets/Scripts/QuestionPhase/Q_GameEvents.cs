using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Quizz/GameEvent")]
public class Q_GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallBack(Question question);
    public UpdateQuestionUICallBack UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallBack(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallBack UpdateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallBack(Q_UIManager.ResolutionScreenType type, int score);
    public DisplayResolutionScreenCallBack DisplayResolutionScreen;

    public delegate void ScoreUpdatedCallBack();
    public ScoreUpdatedCallBack ScoreUpdated;

    [HideInInspector]
    public int CurrentFinalScore;
    [HideInInspector]
    public int StartupHighScore;
}
