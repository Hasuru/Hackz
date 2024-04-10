using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Threading;

[Serializable()]
public struct UIManagerParameter
{
    [Header("Answers Options")]
    [SerializeField] float _margins;
    public float Margins { get { return _margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color _correctBGColor;
    public Color CorrectBGColor { get {return _correctBGColor; } }
    [SerializeField] Color _incorrectBGColor;
    public Color IncorrectBGColor { get {return _incorrectBGColor; } }
    [SerializeField] Color _finishBGColor;
    public Color FinishBGColor { get {return _finishBGColor; } }
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

    [SerializeField] Animator _resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return _resolutionScreenAnimator; } }

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

    private int resStateParamHash = 0;

    private IEnumerator IE_DisplayTimedResolution;

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
    }

    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
    }

    void Start()
    {
        resStateParamHash = Animator.StringToHash("ScreenState");
    }

    void UpdateQuestionUI(Question question)
    {
        uiElements.QuestionInfoText.text = question.Info;
        CreateAnswers(question);
    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResolutionUI(type, score);
        uiElements.ResolutionScreenAnimator.SetInteger(resStateParamHash, 2);
        uiElements.MainCanvas.blocksRaycasts = false;

        if (type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }

            IE_DisplayTimedResolution = DisplayTimedResolution();
            StartCoroutine(IE_DisplayTimedResolution);
        }
    }

    IEnumerator DisplayTimedResolution()
    {
        yield return new WaitForSeconds(Q_Utility.ResolutionDelayTime);
        uiElements.ResolutionScreenAnimator.SetInteger(resStateParamHash, 1);
        uiElements.MainCanvas.blocksRaycasts = true;
    }

    void UpdateResolutionUI(ResolutionScreenType type, int score)
    {
        int highScore = PlayerPrefs.GetInt(Q_Utility.SavePrefKey);

        switch (type)
        {
            case ResolutionScreenType.Correct:
                uiElements.ResolutionBG.color = parameters.CorrectBGColor;
                uiElements.ResolutionStateInfo.text = "CORRECT";
                uiElements.ScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uiElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uiElements.ResolutionStateInfo.text = "INCORRECT";
                uiElements.ScoreText.text = "Better luck next time!";
                break;
            case ResolutionScreenType.Finish:
                uiElements.ResolutionBG.color = parameters.FinishBGColor;
                uiElements.ResolutionStateInfo.text = "FINAL SCORE";
                
                StartCoroutine(CalculateScore());
                uiElements.FinishUIElements.gameObject.SetActive(true);
                uiElements.HighScoreText.gameObject.SetActive(true);
                uiElements.HighScoreText.text = ((highScore > events.StartupHighScore) ? "<color=yellow>new</color>" : string.Empty) 
                                                + "Highscore: " + highScore;
                break;
        }
    }

    IEnumerator CalculateScore()
    {
        int scoreValue = 0;
        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;
            uiElements.ScoreText.text = scoreValue.ToString();
            yield return null;
        }
    }

    void ScoreUpdated(int timeLeft)
    {
        uiElements.ScoreText.text = "Score: " + timeLeft.ToString();
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
