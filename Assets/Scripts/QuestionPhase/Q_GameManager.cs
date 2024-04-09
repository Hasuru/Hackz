using System.Collections.Generic;
using UnityEngine;

public class Q_GameManager : MonoBehaviour
{
  Question[] _questions = null;
  public Question[] Questions { get { return _questions; } }

  [SerializeField] Q_GameEvents events = null;

  private List<AnswerData> PickedAnswers = new List<AnswerData>();
  private List<int> FinishedQuestions = new List<int>();
  private int currentQuestion = 0;

  void Start()
  {
    LoadQuestions();

    Debug.Log(Questions.Length);

    Display();
  }

  public void EraseAnswers()
  {
    PickedAnswers = new List<AnswerData>();
  }

  void Display()
  {
    EraseAnswers();

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
}