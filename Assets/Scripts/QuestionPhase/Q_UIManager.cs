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
        _questionCategory.text = _currentQuestion.QuestionCategory.ToString();
        _questionText.text = _currentQuestion.QuestionInfo;
        for (int i = 0; i < _answersText.Length; i++)
            _answersText[i].text = _currentQuestion.Answers[i];
    }

    public void UpdateTimer(int timer) 
    { 
        _timer.text = timer.ToString();
        
        if (timer > 5 && timer <= 10)
            _timer.color = Color.yellow;
        else if (timer <= 5)
            _timer.color = Color.red;
        else
            _timer.color = Color.white; 
    }

    public void ShowAnswerResultColor(int i, Color color)
    {
        ColorBlock cb = _answerButtons[i].colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.selectedColor = color;
        _answerButtons[i].colors = cb;
    }

    public void ResetButtonColors(Color normalColor, Color highlightedColor)
    {
        foreach (Button b in _answerButtons)
        {
            ColorBlock cb = b.colors;
            cb.normalColor = normalColor;
            cb.highlightedColor = highlightedColor;
            cb.selectedColor = highlightedColor;
            b.colors = cb;
        }
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
