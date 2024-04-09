using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Answer
{
  [SerializeField] private string _info;
  public string Info { get { return _info; } }

  [SerializeField] private bool _isCorrect;
  public bool IsCorrect { get { return _isCorrect; } }
}
[CreateAssetMenu(fileName = "Question", menuName ="Quizz/Question")]
public class Question : ScriptableObject
{
  [SerializeField] private string _info = string.Empty;
  public string Info { get { return _info; } }

  [SerializeField] private int _timer = 0;
  public int Timer { get { return _timer; } }

  [SerializeField] Answer[] _answers;
  public Answer[] Answers { get { return _answers; } }

  [SerializeField] private int _score = 10;
  public int Score { get { return _score; } }

  public int GetCorrectAnswer()
  {
    for (int i = 0; i < Answers.Length; i++)
      if (Answers[i].IsCorrect)
        return i;
    return -1;
  }
}


