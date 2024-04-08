using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Quizz/GameEvent")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallBack(Question question);
    public UpdateQuestionUICallBack updateQuestionUI;

    public delegate void UpdateQuestionAnswerCallBack(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallBack UpdateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallBack(UIManager.ResolutionScreenType type);
    public DisplayResolutionScreenCallBack DisplayResolutionScreen;

    public delegate void ScoreUpdatedCallBack();
    public ScoreUpdatedCallBack ScoreUpdated;

    private int CurrentFinalScore;
}
