using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    AudioSource _audioSource;
    [SerializeField] AudioClip _nextRoundAudio;
    [SerializeField] AudioClip _gameOverAudio;

    [Header("Managers")]
    [SerializeField] GameObject _spawnerManager;


    [Header("Round Settings")]
    [SerializeField] int _cometsPerRound = 1;
    [SerializeField] float _cometSpawnRate = 30f;
    [SerializeField] float _maxSpawnRate;
    [SerializeField] float _spawnRate;
    [SerializeField] int _enemiesToSpawnTotal;
    [SerializeField] int _enemiesToSpawnLeft;
    [SerializeField] int _level;

    [Header("Round States")]
    float _roundCountdown;
    bool _isRoundOver;
    bool _isCountdown;
    bool _isRound;
    bool _isObjectiveRound;
    bool _canTriggerNextRound = true;
    List<GameObject> _enemies = new List<GameObject>();

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
    }
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _level = 1;
        _spawnRate = 0.5f;
        _maxSpawnRate = 0.1f;
        _enemiesToSpawnTotal = 20;
        _roundCountdown = 10f;
        _isCountdown = true;
    }

    void UpdateHighScore()
    {
        if (_level > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", _level);
        }
    }

    void LateUpdate()
    {
        _enemies.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
    }

    void Update()
    {
        // On-going round
        if (_isRound)
        {
            Debug.Log("Enemies Count: " + _enemies.Count);
            Debug.Log("Enemies To Spawn Left: " + _enemiesToSpawnLeft);
            if (_enemies.Count <= 0 && !_isRoundOver && _canTriggerNextRound && _enemiesToSpawnLeft <= 0)
            {

                _isRoundOver = true;
                _isCountdown = true;

                StartCoroutine(NextRoundCooldown());  // Start cooldown to prevent double calls
                EventManager.NextRoundEvent();
            }
        }

        // Preperation stage before next round
        if (_isCountdown)
        {

            _roundCountdown -= Time.deltaTime;
            if (_roundCountdown <= 0)
            {
                _isCountdown = false;
                _isRoundOver = false;
                _isRound = true;
                EventManager.RoundStartEvent();
            }
        }

    }
    IEnumerator NextRoundCooldown()
    {

        _canTriggerNextRound = false;
        yield return new WaitForSeconds(0.1f);  // Short delay to prevent double trigger
        _canTriggerNextRound = true;

    }



    void OnEnable()
    {
        EventManager.OnEnemyDestroyed += RemoveEnemy;
        EventManager.OnRoundStart += RoundStart;
        EventManager.OnNextRound += NextRound;
        EventManager.OnGameOver += GameOverSound;
        EventManager.OnGameOver += UpdateHighScore;

    }

    void OnDisable()
    {
        EventManager.OnEnemyDestroyed -= RemoveEnemy;
        EventManager.OnNextRound -= NextRound;
        EventManager.OnRoundStart -= RoundStart;
        EventManager.OnGameOver -= UpdateHighScore;
        EventManager.OnGameOver -= GameOverSound;


    }


    public void IncreaseLevel()
    {
        _level++;
    }

    public float GetSpawnRate()
    {
        return _spawnRate;
    }

    public List<GameObject> GetEnemies()
    {
        return _enemies;
    }
    public int GetEnemiesToSpawnLeft()
    {
        return _enemiesToSpawnLeft;
    }

    public int GetEnemiesToSpawnTotal()
    {
        return _enemiesToSpawnTotal;
    }
    public void SetEnemiesToSpawnLeft(int value)
    {
        _enemiesToSpawnLeft = value;
    }
    public void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
    }




    private void DisableSpawning()
    {
        _spawnerManager.SetActive(false);
    }

    private void EnableSpawning()
    {
        _spawnerManager.SetActive(true);
    }

    private void RoundStart()
    {
        _isRound = true;
        _isRoundOver = false;
        _roundCountdown = 10f;
        EnableSpawning();
        _enemiesToSpawnLeft = _enemiesToSpawnTotal;
        ObjectivesManager.Instance.SetObjectives("Early", 7);

    }
    private void NextRound()
    {
        ObjectivesManager.Instance.RemoveAllObjectives();
        _isObjectiveRound = false;

        _audioSource.PlayOneShot(_nextRoundAudio);

        if (UnityEngine.Random.value <= 0.99f) _isObjectiveRound = true;
        else _isObjectiveRound = false;
        Debug.Log("Objective Round: " + _isObjectiveRound);
        if (_isObjectiveRound)
        {

            if (_level >= 1 && _level < 20) ObjectivesManager.Instance.SetObjectives("Early", 1);
            else if (_level >= 20 && _level < 30) ObjectivesManager.Instance.SetObjectives("Early", 2);
            else if (_level >= 30 && _level < 40) ObjectivesManager.Instance.SetObjectives("Mid", 1);
            else if (_level >= 40 && _level < 50) ObjectivesManager.Instance.SetObjectives("Mid", 2);
            else if (_level >= 50 && _level < 60) ObjectivesManager.Instance.SetObjectives("Late", 2);
            else if (_level >= 60 && _level < 70) ObjectivesManager.Instance.SetObjectives("Late", 3);
            else if (_level >= 70) ObjectivesManager.Instance.SetObjectives("Late", 4);


        }
        if (_level % UnityEngine.Random.Range(20, 50) == 0)
        {
            _cometsPerRound = 1000;
            _cometSpawnRate = 1f;
        }
        else
        {
            _cometsPerRound = 3;
            _cometSpawnRate = UnityEngine.Random.Range(10f, 60f);
        }

        DisableSpawning();
        _spawnRate -= 0.005f;
        _enemiesToSpawnTotal += 10;
        _spawnRate = Mathf.Max(_spawnRate, _maxSpawnRate);
        IncreaseLevel();
    }
    private void GameOverSound()
    {
        _audioSource.PlayOneShot(_gameOverAudio);
    }

    public void DestroyAllShips()
    {
        List<GameObject> enemiesCopy = new List<GameObject>(_enemies);
        foreach (GameObject enemy in enemiesCopy)
        {
            if (enemy != null)
            {
                enemy.GetComponent<IDamageable>().Die();
            }
        }
        _enemies.Clear();
    }

    public int CometsPerRound { get => _cometsPerRound; set => _cometsPerRound = value; }
    public float CometSpawnRate { get => _cometSpawnRate; set => _cometSpawnRate = value; }
    public float RoundCountdown { get => _roundCountdown; set => _roundCountdown = value; }
    public int Level { get => _level; set => _level = value; }

    public bool IsRound { get => _isRound; set => _isRound = value; }

    public bool IsObjectiveRound { get => _isObjectiveRound; set => _isObjectiveRound = value; }
    public bool IsCountdown { get => _isCountdown; set => _isCountdown = value; }

}
