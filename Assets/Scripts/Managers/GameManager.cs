using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] GameObject _spawnerManager;


    [Header("Round Settings")]
    float _maxSpawnRate;
    float _spawnRate;
    int _enemiesToSpawnTotal;
    int _enemiesToSpawnLeft;
    int _level;

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
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        _level = 1;
        _spawnRate = 0.5f;
        _maxSpawnRate = 0.1f;
        _enemiesToSpawnTotal = 20;
        _roundCountdown = 10f;
        _isCountdown = true;
    }

    void Update()
    {
        // On-going round
        if (_isRound)
        {
            if (_enemies.Count == 0 && !_isRoundOver && _canTriggerNextRound && _enemiesToSpawnLeft == 1)
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
                RoundStart();
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
        EventManager.OnRoundStart += RoundStart;
        EventManager.OnNextRound += NextRound;
    }

    void OnDestroy()
    {
        EventManager.OnNextRound -= NextRound;
        EventManager.OnRoundStart -= RoundStart;

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
        _enemiesToSpawnLeft--;
    }

    public float GetRoundCountdown()
    {
        return _roundCountdown;
    }

    public int Level()
    {
        return _level;
    }

    public bool IsRound()
    {
        return _isRound;
    }

    public bool IsObjectiveRound()
    {
        return _isObjectiveRound;
    }

    public bool IsCountdown()
    {
        return _isCountdown;
    }



    private void DisableSpawning()
    {
        _spawnerManager.SetActive(false);
    }

    private void EnableSpawning()
    {
        _spawnerManager.SetActive(true);
    }

    public void RoundStart()
    {
        _isRound = true;
        _isRoundOver = false;
        _roundCountdown = 10f;
        EnableSpawning();
        _enemiesToSpawnLeft = _enemiesToSpawnTotal;
        ObjectivesManager.Instance.StartObjectives();
    }
    public void NextRound()
    {
        ObjectivesManager.Instance.RemoveAllObjectives();
        ObjectivesUIManager.Instance.ClearObjectivesUI();


        if (UnityEngine.Random.value <= 0.33f) _isObjectiveRound = true;
        else _isObjectiveRound = false;

        if (_isObjectiveRound && _level >= 10)
        {
            // Set the objectives for the round based on the _level of the game
            if (_level >= 5 && _level < 40) ObjectivesManager.Instance.SetActiveObjectives(ObjectivesManager.Instance.earlyObjectives, 1);
            else if (_level >= 40 && _level < 70) ObjectivesManager.Instance.SetActiveObjectives(ObjectivesManager.Instance.midObjectives, UnityEngine.Random.Range(1, 3));
            else ObjectivesManager.Instance.SetActiveObjectives(ObjectivesManager.Instance.lateObjectives, UnityEngine.Random.Range(1, 4));

        }
        DisableSpawning();
        _spawnRate -= 0.005f;
        _enemiesToSpawnTotal += 10;
        if (_spawnRate <= _maxSpawnRate) _spawnRate = _maxSpawnRate;
        IncreaseLevel();
    }

    public void DestroyAllShips()
    {
        List<GameObject> enemiesCopy = new List<GameObject>(_enemies);
        foreach (GameObject enemy in enemiesCopy)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().Destroy();
            }
        }
        _enemies.Clear();
    }



}
