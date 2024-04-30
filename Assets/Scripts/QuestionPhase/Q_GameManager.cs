using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Q_GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] Q_UIManager _uiManager;
    public Q_UIManager UIManager { get { return _uiManager; } }

    [HideInInspector] Question[] _questions;
    [HideInInspector] GameState _gameState;
    [HideInInspector] Question _currentQuestion;
    [HideInInspector] float _timer;

    public void Start()
    {
        // initial load of information
        _questions = Resources.LoadAll<Question>("QPhishing");
        foreach (Question q in _questions)
            Debug.Log(q.QuestionInfo);

        _gameState = GameState.CHOOSING;
        _currentQuestion = _questions[Random.Range(0, _questions.Length - 1)];
        _timer = _currentQuestion.QuestionTimer;
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
                    _gameState = GameState.LOCKED;
                }

                break;
            
            case GameState.LOCKED:
                Debug.Log("time's up");

                break;

            case GameState.SWITCH:
                break;
            
            case GameState.FINISHED:
                break;
        }
    }
}
