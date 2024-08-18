using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _spawnerManager;
    [SerializeField] private GameObject _powerUps;
    public float roundCountdown;
    public bool isCountdown;
    public bool isRound;

    public int enemiesToSpawn;
    public int level;
    public float spawnRate;
    public float maxSpawnRate;
    public List<GameObject> enemies = new List<GameObject>();
    public static GameManager Instance { get; private set; }
    private PlayerManager Player;

    void Awake()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerManager>();
        SetSingleton();
        EventManager.OnNextRound += NextRound;

    }

    void Update()
    {
        if (Player.playerHealth <= 0)
        {
            EventManager.GameOverEvent();
        }
        if (isCountdown)
        {
            roundCountdown -= Time.deltaTime;
            if (roundCountdown <= 0)
            {
                isCountdown = false;
                isRound = true;
                RoundStart();
            }
        }

    }



    void OnDestroy()
    {
        EventManager.OnNextRound -= NextRound;
    }

    void Start()
    {
        level = 1;
        spawnRate = 0.5f;
        maxSpawnRate = 0.1f;
        roundCountdown = 5f;
        isCountdown = true;

    }

    public void IncreaseLevel()
    {
        level++;
    }

    public PlayerManager GetPlayer()
    {
        return Player;
    }


    void SetSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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
        roundCountdown = 5f;
        EnableSpawning();
    }
    public void NextRound()
    {

        DisableSpawning();
        spawnRate -= 0.01f;
        if (spawnRate < maxSpawnRate) spawnRate = maxSpawnRate;
        level++;

    }

    public void DestroyAllShips()
    {
        List<GameObject> enemiesCopy = new List<GameObject>(enemies);
        foreach (GameObject enemy in enemiesCopy)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().Destroy();
            }
        }
        enemies.Clear();
    }



}
