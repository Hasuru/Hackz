using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Q_GameManager : NetworkBehaviour
{
    [Header("Managers")]
    [SerializeField] Q_UIManager _uiManager;
    public Q_UIManager UIManager { get { return _uiManager; } }
    [SerializeField] GamePreferencesManager _prefsManager;
    public GamePreferencesManager PrefsManager { get { return _prefsManager;} }

    [HideInInspector] int _points = 0;
    public int Points { get { return _points; } }
    [HideInInspector] int[] _powerUps = new int[2];
    public int[] PowerUps { get { return _powerUps; } }
    [HideInInspector] CategoryType _category = CategoryType.PHISHING;
    public CategoryType Category { get { return _category; } }

    [HideInInspector] GameState _gameState;
    [HideInInspector] Question[] _questions;
    [HideInInspector] Question _currentQuestion;
    [HideInInspector] int _currentAnswer;
    [HideInInspector] float _timer;
    private bool alreadyUsed = false;
    private int questionCount = 0;
    private int rand = 0;

    [Header("User Interface")]
    [SerializeField] private GameObject loadingUI;

    public void SetPoints(int pts) { _points = pts; }
    public void SetTimePU(int qnt) { _powerUps[0] = qnt; }
    public void SetCutPU(int qnt) { _powerUps[1] = qnt; }
    public void SetCategory(CategoryType type) { _category = type; }

    /// <summary>
    /// Fetches Questions and loads the first Question
    /// </summary>
    public void Start()
    {
        _prefsManager.LoadPrefs();
        FetchQuestions();
        LoadNewQuestion();
        _uiManager.UpdatePowerUp();
    }

    private void Awake()
    {
        loadingUI.SetActive(true);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // When all of the clients have loaded
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += QuestionPhase_OnLoadEventCompleted;
        }
    }

    private void QuestionPhase_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        SetInitialSettingsClientRpc();
    }

    [ClientRpc]
    private void SetInitialSettingsClientRpc()
    {
        loadingUI.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        _prefsManager.SavePrefs(_points, _category.ToString(), _powerUps[0], _powerUps[1]);
    }

    /// <summary>
    /// Checks current Game State and updates game 
    /// </summary>
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
                    _uiManager.BlockButtons();
                    StartCoroutine(ChangeState(GameState.LOCKED, 0));
                }
                break;
            
            case GameState.LOCKED:
                SubmitAnswer();
                break;

            case GameState.SWITCH:
                StartCoroutine(RefreshQuestion());
                break;
            
            case GameState.FINISHED:
                StartCoroutine(EndScene());
                break;
            case GameState.TERMINATED:
                Loader.LoadNetwork(Loader.Scene.TopicWheelScene);
                break;
        }
    }


    /// <summary>
    /// Selects a question from _questions list and locks that position as used. Also updates UI to display the question
    /// </summary>
    public void LoadNewQuestion()
    {
        _gameState = GameState.CHOOSING;
        questionCount++;

        _currentQuestion = _questions[rand++];

        _uiManager.Show(_currentQuestion);
        _currentAnswer = -1;

        _timer = _currentQuestion.QuestionTimer;
        alreadyUsed = false;
    }


    /// <summary>
    /// Fetches questions depending on Category type and updates the question HashSet
    /// </summary>
    public void FetchQuestions()
    {
        // Question Load
        if (String.Compare(HackzMultiplayer.Instance.GetCurrentTopic(), "Phishing") == 0)
            _questions = Resources.LoadAll<Question>("QPhishing");
        else if (String.Compare(HackzMultiplayer.Instance.GetCurrentTopic(), "Password") == 0)
            _questions = Resources.LoadAll<Question>("QPassword");
        else
            return;
    }

    /// <summary>
    /// Change the answer index to the currently selected answer on the UI (-1 if none is selected)
    /// </summary>
    /// <param name="index">Index of the new answer selected</param>
    public void ChangeCurrentAnswer(int index) 
    { 
        if (index >= 0 && index < _currentQuestion.Answers.Length)
            _currentAnswer = index; 
    }

    public void Close() { Application.Quit(); }

    /// <summary>
    /// Adds additional time to the timer
    /// </summary>
    public void AddTime() 
    { 
        if (_powerUps[0] <= 0 || alreadyUsed) return;

        _timer += 20;
        _powerUps[0]--;
        _uiManager.UpdatePowerUp();
        alreadyUsed = true; 
    }


    /// <summary>
    /// Selects one of the wrong answers to be disabled 
    /// </summary>
    public void CutChoice()
    {
        if (_powerUps[1] <= 0) return;

        int index = -1;
        do
        {
            index = UnityEngine.Random.Range(0, _currentQuestion.Answers.Length - 1);
        } while (index == _currentQuestion.CorrectAnswerId);

        _uiManager.BlockButton(index);
        _powerUps[1]--;
        _uiManager.UpdatePowerUp();
    }

    private bool CheckCorrectAnswer() { return _currentAnswer == _currentQuestion.CorrectAnswerId; }


    /// <summary>
    /// Update score, game state and display the result accordingly
    /// </summary>
    public void SubmitAnswer()
    {
        if (CheckCorrectAnswer())
        {
            _points += _currentQuestion.QuestionPoints;
            UIManager.ShowAnswerResultColor(_currentAnswer, new Color(0, 255, 0));
            UIManager.UpdatePoints();
        } 
        else
        {
            UIManager.ShowAnswerResultColor(_currentQuestion.CorrectAnswerId, new Color(0, 255, 0));
            if (_currentAnswer != -1)
                UIManager.ShowAnswerResultColor(_currentAnswer, new Color(255, 0, 0));
        }
        
        StartCoroutine(ChangeState(GameState.SWITCH, 3));
    }

    private IEnumerator EndScene()
    {
        string category;
        if (_category == CategoryType.PASSWORD)
            category = "PHISHING";
        else
            category = "PASSWORD";

        _prefsManager.SavePrefs(_points, category, _powerUps[0], _powerUps[1]);
        _uiManager.DisplayFinalScore();
        _gameState = GameState.TERMINATED;
        yield return new WaitForSeconds(3);
        // SwitchScenes
    } 

    /// <summary>
    /// Loads new Question if the Question count hasn't reached its limit, otherwise changes Game to a FINISHED State
    /// </summary>
    private IEnumerator RefreshQuestion()
    {
        if (questionCount >= 4)
        {
            // game is finished
            _gameState = GameState.FINISHED;
            yield return new WaitForSeconds(0);
        }
        else
        {
            // load new question
            LoadNewQuestion();
            _uiManager.ResetButtonColors(new Color(0, 255, 255), new Color(255, 171, 0));
            _uiManager.UnblockButtons();   
        }

        yield return new WaitForSeconds(0);
    }

    private IEnumerator ChangeState(GameState state, int freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
        _gameState = state;
    }

    public void OnDestroy()
    {
        // NetworkManager has a longer life cycle, so unsub from it
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= QuestionPhase_OnLoadEventCompleted;
    }
}
