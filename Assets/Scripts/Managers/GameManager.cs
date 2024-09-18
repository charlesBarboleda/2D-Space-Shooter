using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("Game State Machine")]
    GameState _currentState;
    int _currentLevelIndex = 0;
    bool _isInputActive = true;
    public enum GameState
    {
        Countdown,
        Start,
        Paused,
        LevelIn,
        LevelEnd,
        GameOver,
    }

    [Header("Audio")]
    AudioSource _audioSource;
    [SerializeField] AudioClip _nextRoundAudio;
    [SerializeField] AudioClip _gameOverAudio;

    [Header("Managers")]
    SpawnerManager _spawnerManager;
    LevelManager _levelManager;


    [Header("Round Settings")]
    [SerializeField] int _cometsPerRound = 1;
    [SerializeField] float _cometSpawnRate = 30f;


    [Header("Round States")]
    float _roundCountdown;


    void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            DontDestroyOnLoad(this);
        }
        else
        {
            Instance = this;
        }
        _levelManager = GetComponent<LevelManager>();
        _audioSource = GetComponent<AudioSource>();
        _spawnerManager = GetComponent<SpawnerManager>();
    }
    void Start()
    {
        if (_levelManager != null)
        {

            Debug.Log($"Levels Count: {_levelManager.Levels.Count}");

            StartCoroutine(GameStartCoroutine());
        }
        else
        {
            Debug.LogError("Level Manager not found");
        }

    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (_levelManager.Levels.Count > 0)
        {
            Debug.Log("Game Started... Counting down");
            ChangeState(GameState.Countdown);
        }
        else
        {
            Debug.LogError("No levels found");
        }
    }

    public void ChangeState(GameState state)
    {
        Debug.Log($"Changing State to {state}");
        _currentState = state;
        switch (_currentState)
        {
            case GameState.Countdown:
                StartCoroutine(StartCountdown());
                break;
            case GameState.Start:
                StartGame();
                break;
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.LevelIn:
                LevelStart();
                break;
            case GameState.LevelEnd:
                LevelComplete();
                break;
            case GameState.GameOver:
                GameEnd();
                break;
        }
    }

    IEnumerator StartCountdown()
    {
        Debug.Log("Countdown Started");
        _isInputActive = true;
        _roundCountdown = 10f;

        while (_roundCountdown > 0)
        {
            _roundCountdown -= Time.deltaTime;
            yield return null;
        }

        ChangeState(GameState.Start);

    }

    void StartGame()
    {
        Debug.Log("Game Started");
        ChangeState(GameState.LevelIn);
    }

    void PauseGame()
    {
        Debug.Log("Game Paused");
        ChangeState(GameState.Paused);
        _isInputActive = false;
    }

    void UnPauseGame()
    {
        Debug.Log("Game Unpaused");
        _isInputActive = true;
    }

    void LevelStart()
    {
        Debug.Log("Level Start from Game Manager");

        _levelManager.StartLevel();
    }

    void LevelComplete()
    {
        Debug.Log("Level End");
        _levelManager.CompleteLevel();
        ChangeState(GameState.LevelEnd);
        _levelManager.CurrentLevelIndex++;
    }

    void GameEnd()
    {
        Debug.Log("Game Over");
        ChangeState(GameState.GameOver);
        _isInputActive = false;
        _audioSource.PlayOneShot(_gameOverAudio);
        UpdateHighScore();
    }



    void UpdateHighScore()
    {
        if (_levelManager.CurrentLevelIndex > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", _levelManager.CurrentLevelIndex + 1);
        }
    }



    void OnEnable()
    {
        EventManager.OnGameOver += GameOverSound;
        EventManager.OnGameOver += UpdateHighScore;

    }

    void OnDisable()
    {
        EventManager.OnGameOver -= UpdateHighScore;
        EventManager.OnGameOver -= GameOverSound;
    }





    private void GameOverSound()
    {
        _audioSource.PlayOneShot(_gameOverAudio);
    }

    public void DestroyAllShips()
    {
        List<GameObject> enemiesCopy = new List<GameObject>(SpawnerManager.Instance.EnemiesList);
        foreach (GameObject enemy in enemiesCopy)
        {
            if (enemy != null)
            {
                enemy.GetComponent<IDamageable>().Die();
            }
        }
        SpawnerManager.Instance.EnemiesList.Clear();
    }

    public int CometsPerRound { get => _cometsPerRound; set => _cometsPerRound = value; }
    public float CometSpawnRate { get => _cometSpawnRate; set => _cometSpawnRate = value; }
    public float RoundCountdown { get => _roundCountdown; }
    public bool IsInputActive { get => _isInputActive; }
    public GameState CurrentGameState { get => _currentState; }

}
