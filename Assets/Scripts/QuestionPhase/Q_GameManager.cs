using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/*
Color _red = new Color(255, 0, 0);
Color _green = new Color(0, 255, 0);
Color _cyan = new Color(0, 255, 255);
Color _orange = new Color(255, 171, 0);
Color _darkGrey = new Color(46, 46, 46);
*/

public class Q_GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] Q_UIManager _uiManager;
    public Q_UIManager UIManager { get { return _uiManager; } }

    [Header("Persistence")]
    [SerializeField] int _points;
    public int Points { get { return _points; } }

    [HideInInspector] GameState _gameState;
    [HideInInspector] Question[] _questions;
    [HideInInspector] Question _currentQuestion;
    [HideInInspector] int _currentAnswer;
    [HideInInspector] float _timer;

    private bool isRunning = true;
    private bool isReseting = false;
    private int questionCount = 0;

    public void Start()
    {
        // Question Load
        _questions = Resources.LoadAll<Question>("QPhishing");
        LoadNewQuestion();
    }

    public void Update()
    {
        switch (_gameState)
        {
            case GameState.CHOOSING:
                if (_timer > 0)
                {
                    _uiManager.UpdateTimer((int)_timer);
                    _timer -= 1 * Time.deltaTime;
                }
                else
                {
                    SubmitAnswer();
                    _uiManager.BlockButtons();
                }

                break;
            
            case GameState.LOCKED:
                if (isRunning)
                {
                    _gameState = GameState.SWITCH;
                    isReseting = true;
                }
                else
                {
                    StartCoroutine(Freeze(2));
                }
                break;

            case GameState.SWITCH:
                if (isReseting)
                {
                    StartCoroutine(RefreshQuestion());
                    isReseting = false;
                }
                break;
            
            case GameState.FINISHED:
                break;
        }
    }

    public void LoadNewQuestion()
    {
        _gameState = GameState.CHOOSING;

        _currentQuestion = _questions[Random.Range(0, _questions.Length - 1)];
        _uiManager.Show(_currentQuestion);
        _currentAnswer = -1;

        _timer = _currentQuestion.QuestionTimer;
    }

    public void ChangeCurrentAnswer(int index) 
    { 
        if (index >= 0 && index < _currentQuestion.Answers.Length)
            _currentAnswer = index; 
    }

    public void SubmitAnswer()
    {
        isRunning = false;
        _gameState = GameState.LOCKED;
        if (CheckCorrectAnswer())
        {
            _points += _currentQuestion.QuestionPoints;
            UIManager.ShowAnswerResultColor(_currentAnswer, new Color(0, 255, 0));
            UIManager.UpdatePoints();
            // correct pop up screen
        } 
        else
        {
            UIManager.ShowAnswerResultColor(_currentQuestion.CorrectAnswerId, new Color(0, 255, 0));
            if (_currentAnswer != -1)
                UIManager.ShowAnswerResultColor(_currentAnswer, new Color(255, 0, 0));
            // wrong pop up screen
        }
    }

    private bool CheckCorrectAnswer() { return _currentAnswer == _currentQuestion.CorrectAnswerId; }

    private IEnumerator Freeze(int sec)
    {
        yield return new WaitForSeconds(sec);
        isRunning = true;
    }

    private IEnumerator RefreshQuestion()
    {
        if (questionCount < 4)
        {
            LoadNewQuestion();
            _uiManager.ResetButtonColors(new Color(0, 255, 255), new Color(255, 171, 0));
            _uiManager.UnblockButtons();
            questionCount++;
        }
        else
        {
            _gameState = GameState.FINISHED;   
        }
        
        yield return new WaitForSeconds(0);
    }
}
