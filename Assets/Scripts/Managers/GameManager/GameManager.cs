using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public static event Action OnGameOver;
    public static event Action OnNextRound;
    public static event Action OnPowerUpRound;
    public static event Action OnBossRound;
    public static event Action OnPowerUpChosen;
    [SerializeField] private GameObject spawnerManager;
    [SerializeField] private GameObject powerUps;

    public float points;
    public float pointsRequired;
    public int level;
    public float spawnRate;
    public float maxSpawnRate;
    public List<GameObject> enemies = new List<GameObject>();
    public static GameManager Instance { get; private set; }
    public static GameManager GetInstance() { return Instance; }
    private PlayerManager Player;

    void Awake()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerManager>();
        SetSingleton();
        OnNextRound += NextRound;
        OnBossRound += DestroyAllShips;
        OnBossRound += DisableSpawning;
        OnPowerUpRound += ChoosePowerUp;
    }

    void OnDestroy()
    {
        OnNextRound -= NextRound;
        OnBossRound -= DestroyAllShips;
        OnBossRound -= DisableSpawning;
        OnPowerUpRound -= ChoosePowerUp;
    }

    void Start()
    {
        EnableSpawning();
    }

    void Update()
    {
        if (level % 5 == 0 && level != 0)
        {
            OnPowerUpRound?.Invoke();
        }
        if (points >= pointsRequired)
        {
            OnNextRound?.Invoke();
        }

        if ((level - 49) % 50 == 0 && level >= 49)
        {
            OnBossRound?.Invoke();
        }
        else
        {
            EnableSpawning();
        }
        if (Player.playerHealth <= 0)
        {
            OnGameOver?.Invoke();
        }
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
        spawnerManager.SetActive(false);
    }

    private void EnableSpawning()
    {
        spawnerManager.SetActive(true);
    }

    private void ChoosePowerUp()
    {
        powerUps.SetActive(true);
        Time.timeScale = 0;
    }
    public void NextRound()
    {
        spawnRate -= 0.001f;
        if (spawnRate < maxSpawnRate) spawnRate = maxSpawnRate;
        level++;
        points = 0;
        pointsRequired += 5f;

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
