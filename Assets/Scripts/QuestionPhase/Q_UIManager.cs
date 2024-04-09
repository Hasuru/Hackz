using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[Serializable()]
public struct UIManagerParameter
{
    [Header("Answers Options")]
    [SerializeField] float _margins;
    public float Margins { get { return _margins; } }
}

[Serializable()]
public struct UIElements
{
    [SerializeField] RectTransform _answersContentArea;
    public RectTransform AnswersContentArea { get { return _answersContentArea; } }

    [SerializeField] TextMeshProUGUI _questionInfoText;
    public TextMeshProUGUI QuestionInfoText { get { return _questionInfoText; } }

    [SerializeField] TextMeshProUGUI _scoreText;
    public TextMeshProUGUI ScoreText { get { return _scoreText; } }

    [Space]

    [SerializeField] Image _resolutionBG;
    public Image ResolutionBG { get { return _resolutionBG; } }

    [SerializeField] TextMeshProUGUI _resolutionStateInfo;
    public TextMeshProUGUI ResolutionStateInfo { get { return _resolutionStateInfo; } }

    [SerializeField] TextMeshProUGUI _resolutionScoreInfo;
    public TextMeshProUGUI ResolutionScoreInfo { get { return _resolutionStateInfo; } }

    [Space]

    [SerializeField] TextMeshProUGUI _highScoreText;
    public TextMeshProUGUI HighScoreText { get { return _highScoreText; } }

    [SerializeField] CanvasGroup _mainCanvas;
    public CanvasGroup MainCanvas { get { return _mainCanvas; } }

    [SerializeField] RectTransform _finishUIElements;
    public RectTransform FinishUIElements { get { return _finishUIElements; } }
}

public class Q_UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField] Q_GameEvents events;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] AnswerData answerPrefab;

    [SerializeField] UIElements uiElements;

    [SerializeField] UIManagerParameter parameters;

    AnswerData currentAnswer;

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
    }

    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
    }

    void UpdateQuestionUI(Question question)
    {
        uiElements.QuestionInfoText.text = question.Info;
        CreateAnswers(question);
    }

    void CreateAnswers(Question question)
    {
        EraseAnswers();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uiElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            //uiElements.AnswersContentArea.sizeDelta = new Vector2(uiElements.AnswersContentArea.sizeDelta.x, offset);

            currentAnswer = newAnswer;
        }
    }

    void EraseAnswers()
    {
        Destroy(currentAnswer);
    }
}
