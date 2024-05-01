using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Q_UIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button[] _answerButtons;
    public Button[] AnswerButtons { get { return _answerButtons; } }

    [Header("Object Area")]
    [SerializeField] Q_GameManager _gameManager;
    public Q_GameManager GameManager { get { return _gameManager; } }
    [HideInInspector] Question _currentQuestion;
    public Question CurrentQuestion { get { return _currentQuestion; } set {} } 

    [SerializeField] TextMeshProUGUI _timer;
    public TextMeshProUGUI Timer { get { return _timer; } }
    [SerializeField] TextMeshProUGUI _questionCategory;
    public TextMeshProUGUI QuestionCategory { get { return _questionCategory; } } 

    [Header("Text Area")]
    [SerializeField] TextMeshProUGUI _questionText;
    public TextMeshProUGUI QuestionText { get { return _questionText; } }
    [SerializeField] TextMeshProUGUI _pointsText;
    public TextMeshProUGUI PointsText { get { return _pointsText; } }
    [SerializeField] TextMeshProUGUI[] _answersText;
    public TextMeshProUGUI[] AnswersText { get { return _answersText; } }

    public void Show(Question q)
    {
        _currentQuestion = q;
        UpdateQuestionText();
        UpdatePoints();
    }

    public void UpdatePoints() { PointsText.text = "Points: "  + _gameManager.Points.ToString(); }

    public void UpdateQuestionText()
    {
        _questionCategory.text = CurrentQuestion.QuestionCategory.ToString();
        _questionText.text = CurrentQuestion.QuestionInfo;
        for (int i = 0; i < _answersText.Length; i++)
            _answersText[i].text = CurrentQuestion.Answers[i];
    }

    public void UpdateTimer(int timer) { _timer.text = timer.ToString(); }

    public void ShowAnswerResultColor(int i, Color color)
    {
        ColorBlock cb = _answerButtons[i].colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.selectedColor = color;
        _answerButtons[i].colors = cb;
    }

    public void BlockButtons()
    {
        foreach(Button b in _answerButtons)
            b.enabled= false;
    }

    public void UnblockButtons()
    {
        foreach(Button b in _answerButtons)
            b.enabled = true;
    }
}
