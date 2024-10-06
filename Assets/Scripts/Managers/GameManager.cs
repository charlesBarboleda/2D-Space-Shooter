using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("Game State Machine")]
    GameState _currentState;
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
        _levelManager.CurrentLevelIndex = 1;
        if (_levelManager != null)
        {

            Debug.Log($"Levels Count: {_levelManager.Levels.Count}");

            StartCoroutine(GameStartCoroutine());
        }
        else
        {
            Debug.LogError("Level Manager not found");
        }
        StartCoroutine(DisableShipCollision());
    }

    IEnumerator DisableShipCollision()
    {
        yield return new WaitForSeconds(1f);
        Collider2D[] ships = Physics2D.OverlapCircleAll(new Vector2(0, 0), 300f);
        foreach (var ship1 in ships)
        {
            if (!ship1.CompareTag("Player")) continue;
            foreach (var ship2 in ships)
            {
                if (ship1 != ship2)
                {
                    Physics2D.IgnoreCollision(ship1.GetComponent<Collider2D>(), ship2.GetComponent<Collider2D>());
                }
            }
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
        ObjectiveManager.Instance.ClearActiveObjectives();
        Debug.Log("Countdown Started");
        _isInputActive = true;
        _roundCountdown = 20f;
        UIManager.Instance.countdownText.gameObject.SetActive(true);

        while (_roundCountdown > 0)
        {
            _roundCountdown -= Time.deltaTime;
            yield return null;
        }
        UIManager.Instance.countdownText.gameObject.SetActive(false);
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
        _spawnerManager.SpawnCometsOverTime(_cometsPerRound, _cometSpawnRate);
        Debug.Log("Level Start from Game Manager");
        _levelManager.StartLevel();
    }

    void LevelComplete()
    {
        Debug.Log("Level Complete from Game Manager");
        AudioManager.Instance.PlaySound(_audioSource, _nextRoundAudio);
        ChangeState(GameState.Countdown);
    }

    void GameEnd()
    {
        Debug.Log("Game Over");
        ChangeState(GameState.GameOver);
        _isInputActive = false;
        GameOverSound();
        UpdateHighScore();
    }



    void UpdateHighScore()
    {
        if (_levelManager.CurrentLevelIndex + 1 > PlayerPrefs.GetFloat("HighScore"))
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
        AudioManager.Instance.PlaySound(_audioSource, _gameOverAudio);
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
