using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quizz", menuName = "Questions/New Question")]
public class Question : ScriptableObject
{
    [Header("Question General Information")]
    [SerializeField] CategoryType _questionCategory;
    [SerializeField] string _questionInfo;
    public string QuestionInfo { get { return _questionInfo; } }
    [SerializeField] int _questionTimer;
    public int QuestionTimer { get { return _questionTimer; } }
    [SerializeField] int _questionPoints;
    public int QuestionPoints { get { return _questionPoints; } }


    [Header("Answer General Information")]
    [SerializeField] int _correctAnswerId;
    public int CorrectAnswerId { get { return _correctAnswerId; } }
    [SerializeField] string[] _answers;
    public string[] Answers { get { return _answers;} }
}
