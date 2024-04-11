using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Q_GameManager : MonoBehaviour
{
  Question[] _questions = null;
  public Question[] Questions { get { return _questions; } }

  [SerializeField] Q_GameEvents events = null;

  private AnswerData PickedAnswer = null;
  public void SetPickedAnswer(AnswerData ans) { PickedAnswer = ans; }

  private List<int> FinishedQuestions = new List<int>();
  private int currentQuestion = 0;

  private IEnumerator IE_WaitForNextRound = null;

  private bool isFinished
  {
    get
    {
      return (FinishedQuestions.Count < 4) ? false : true;
    }
  }

  void Start()
  {
    LoadQuestions();

    Debug.Log(Questions.Length);

    Display();
  }

  public void EraseAnswer()
  {
    PickedAnswer = null;
  }

  void Display()
  {
    EraseAnswer();

    Question question = GetRandomQuestion();

    if (events.UpdateQuestionUI != null)
    {
      events.UpdateQuestionUI(question);
    }
    else
    {
      Debug.Log("Ups! Something went wrong while trying to display the question UI!");
    }
  }

  public void AcceptAnswer()
  {
    bool isCorrect = CheckAnswer();
    FinishedQuestions.Add(currentQuestion);

    UpdateScore((isCorrect) ? Questions[currentQuestion].Score : 0);

    Q_UIManager.ResolutionScreenType type = 
                (isFinished) ? Q_UIManager.ResolutionScreenType.Finish 
                : ((isCorrect) ? Q_UIManager.ResolutionScreenType.Correct : Q_UIManager.ResolutionScreenType.Incorrect);

    events.DisplayResolutionScreen?.Invoke(type, Questions[currentQuestion].Score);

    if (IE_WaitForNextRound != null)
      StopCoroutine(IE_WaitForNextRound);
    
    IE_WaitForNextRound = WaitForNextRound();
    StartCoroutine(IE_WaitForNextRound);
  }

  IEnumerator WaitForNextRound()
  {
    yield return new WaitForSeconds(Q_Utility.ResolutionDelayTime);
    Display();
  }

  Question GetRandomQuestion()
  {
    int randomIndex = GetRandomQuestionIndex();
    currentQuestion = randomIndex;

    return Questions[currentQuestion];
  }

  int GetRandomQuestionIndex()
  {
    int random = 0;
    if (FinishedQuestions.Count < 4)
    {
      do
      {
        random = UnityEngine.Random.Range(0, Questions.Length);
      } while (FinishedQuestions.Contains(random) || random == currentQuestion);
    }
    return random;
  }

  void LoadQuestions()
  {
    Object[] objs = Resources.LoadAll("Questions", typeof(Question));
    _questions = new Question[objs.Length];
    for (int i = 0; i < objs.Length; i++)
    {
      _questions[i] = (Question)objs[i];
    }
  }

  bool CheckAnswer()
  {
    int correct = Questions[currentQuestion].GetCorrectAnswer();
    int picked = PickedAnswer.AnswerIndex;

    // if smth goes wrong and both are -1, function would return true instead of returning false
    if (correct == -1 || picked == -1)
      return false;

    return correct == picked;
  }

  private void UpdateScore(int score)
  {
    events.CurrentFinalScore += score;

    events.ScoreUpdated?.Invoke();
  }
}