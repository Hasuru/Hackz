using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public struct Answer
{
  [SerializeField] private string _info;
  public string Info { get { return _info; } }

  [SerializeField] private bool _isCorrect;
  public bool IsCorrect { get { return _isCorrect; } }
}

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/new Question")]
public class Question : ScriptableObject 
{
  [SerializeField] private String _info = String.Empty;
  public String Info { get { return _info; } }
  [SerializeField] Answer[] _answers = null;
  public Answer[] Answers { get { return _answers; } }

  //Parameters

  [SerializeField] private bool _useTimer = false;
  public bool UseTimer { get { return _useTimer; } }

  [SerializeField] private int _timer = 0;
  public int Timer { get { return _timer; } }

  [SerializeField] private int _addScore = 10;
  public int AddScore { get { return _addScore; } }


  public int GetCorrectAnswer()
  {
    for (int i = 0; i < Answers.Length; i++)
    {
      if (Answers[i].IsCorrect)
      {
        return i;
      }
    }
    return -1;
  }
}