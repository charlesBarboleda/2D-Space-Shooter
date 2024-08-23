using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] GameObject _spawnerManager;
    PlayerManager player;

    [Header("Round Settings")]
    bool isRoundOver;
    bool canTriggerNextRound = true;
    public float maxSpawnRate;
    public float spawnRate;
    public int enemiesToSpawn;
    public int level;
    public float roundCountdown;
    public bool isCountdown;
    public bool isRound;
    public List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
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
        if (player.playerHealth <= 0)
        {
            EventManager.GameOverEvent();
        }
        Debug.Log("Enemies Count: " + enemies.Count);
        Debug.Log("Enemies to Spawn Count: " + enemiesToSpawn);

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
        return player;
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
