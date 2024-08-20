using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _spawnerManager;
    [SerializeField] private GameObject _powerUps;
    public float roundCountdown;
    public bool isCountdown;
    public bool isRound;
    bool isRoundOver;
    bool canTriggerNextRound = true;
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

    }

    IEnumerator NextRoundCooldown()
    {
        canTriggerNextRound = false;
        yield return new WaitForSeconds(0.1f);  // Short delay to prevent double trigger
        canTriggerNextRound = true;
    }

    void Update()
    {
        if (Player.playerHealth <= 0)
        {
            EventManager.GameOverEvent();
        }


        if (enemies.Count == 0 && !isRoundOver && canTriggerNextRound)
        {
            isRoundOver = true;
            isCountdown = true;
            EventManager.NextRoundEvent();
            StartCoroutine(NextRoundCooldown());  // Start cooldown to prevent double calls
        }

        if (isCountdown)
        {
            roundCountdown -= Time.deltaTime;
            if (roundCountdown <= 0)
            {
                isCountdown = false;
                isRoundOver = false;
                isRound = true;
                RoundStart();
            }
        }
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

    void Start()
    {
        level = 0;
        spawnRate = 0.5f;
        maxSpawnRate = 0.1f;
        enemiesToSpawn = 10;
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
        isRound = true;
        isRoundOver = false;
        roundCountdown = 5f;
        EnableSpawning();
    }
    public void NextRound()
    {



        DisableSpawning();
        spawnRate -= 0.01f;
        enemiesToSpawn += 10;
        if (spawnRate < maxSpawnRate) spawnRate = maxSpawnRate;
        IncreaseLevel();

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
