using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
  [SerializeField]
  string question;

  [SerializeField]
  List<string> answers;

  [SerializeField]
  TMP_Text questionText, timerText;

  [SerializeField]
  List<TMP_Text> answersText;

  private int timer = 10;
  private float timeLeft = 1.0f;

  private void FixedUpdate()
  {
    UpdateClock();
  }

  bool UpdateClock()
  {
    if (timer == 0)
      return false;

    timeLeft -= Time.deltaTime;  
    if (timeLeft <= 0) 
    {
      timer--;
      timerText.text = timer.ToString();
      timeLeft = 1.0f;
    }

    return true;
  }
}
