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
    public int enemiesToSpawnTotal;
    public int enemiesToSpawnLeft;
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

    void Update()
    {
        if (player.playerHealth <= 0)
        {
            EventManager.GameOverEvent();
        }
        Debug.Log("Enemies Count: " + enemies.Count);
        Debug.Log("Enemies to Spawn Left: " + enemiesToSpawnLeft);
        Debug.Log("isRoundOver " + isRoundOver);
        Debug.Log("isCountdown " + isCountdown);
        Debug.Log("isRound " + isRound);
        Debug.Log("canTriggerNextRound " + canTriggerNextRound);

        if (isRound)
        {
            if (enemies.Count == 0 && !isRoundOver && canTriggerNextRound && enemiesToSpawnLeft == 1)
            {
                Debug.Log("Round Over");
                isRoundOver = true;
                isCountdown = true;
                Debug.Log("isCountdown set to true");
                StartCoroutine(NextRoundCooldown());  // Start cooldown to prevent double calls
                EventManager.NextRoundEvent();
            }
        }

        if (isCountdown)
        {
            Debug.Log("Countdown Round");
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
    IEnumerator NextRoundCooldown()
    {
        Debug.Log("Starting NextRoundCooldown");
        canTriggerNextRound = false;
        yield return new WaitForSeconds(0.1f);  // Short delay to prevent double trigger
        canTriggerNextRound = true;
        Debug.Log("Ending NextRoundCooldown");
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
        level = 1;
        spawnRate = 1f;
        maxSpawnRate = 0.3f;
        enemiesToSpawnTotal = 5;
        roundCountdown = 3f;
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
        enemiesToSpawnLeft = enemiesToSpawnTotal;
    }
    public void NextRound()
    {
        DisableSpawning();
        spawnRate -= 0.01f;
        enemiesToSpawnTotal += 5;
        if (spawnRate <= maxSpawnRate) spawnRate = maxSpawnRate;
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
