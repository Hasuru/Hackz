using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Q_UIManager : MonoBehaviour
{
    [Header("Object Area")]
    [SerializeField] Question _currentQuestion;
    public Question CurrentQuestion { get { return _currentQuestion; } } 

    [Header("Text Area")]
    [SerializeField] TextMeshProUGUI _questionText;
    public TextMeshProUGUI QuestionText { get { return _questionText; } }

    [SerializeField] TextMeshProUGUI[] _answersText;
    public TextMeshProUGUI[] AnswersText { get { return _answersText; } }

    public void Start()
    {
        // update text info of the boxes
        _questionText.text = CurrentQuestion.QuestionInfo;
        for (int i = 0; i < _answersText.Length; i++)
            _answersText[i].text = CurrentQuestion.Answers[i];
    }
}
