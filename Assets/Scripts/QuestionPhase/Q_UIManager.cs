using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Q_UIManager : MonoBehaviour
{
    [Header("Object Area")]
    [SerializeField] Q_GameManager _gameManager;
    public Q_GameManager GameManager { get { return _gameManager; } }
    [SerializeField] Question _currentQuestion;
    public Question CurrentQuestion { get { return _currentQuestion; } } 

    [SerializeField] TextMeshProUGUI _questionCategory;
    public TextMeshProUGUI QuestionCategory { get { return _questionCategory; } } 

    [Header("Text Area")]
    [SerializeField] TextMeshProUGUI _questionText;
    public TextMeshProUGUI QuestionText { get { return _questionText; } }

    [SerializeField] TextMeshProUGUI[] _answersText;
    public TextMeshProUGUI[] AnswersText { get { return _answersText; } }

    [SerializeField] TextMeshProUGUI _timer;
    public TextMeshProUGUI Timer { get { return _timer; } }

    public void Start()
    {
        // update text info of the boxes
        _questionCategory.text = CurrentQuestion.QuestionCategory.ToString();
        _questionText.text = CurrentQuestion.QuestionInfo;
        for (int i = 0; i < _answersText.Length; i++)
            _answersText[i].text = CurrentQuestion.Answers[i];
    }

    public void UpdateTimer(int timer) { _timer.text = "Time: " + timer.ToString(); }
}
