using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject spawnerManager;
    [SerializeField] private GameObject powerUps;

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
            EventManager.GameOver();
        }
    }



    void OnDestroy()
    {
        EventManager.OnNextRound -= NextRound;
    }

    void Start()
    {
        EnableSpawning();
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
        spawnerManager.SetActive(false);
    }

    private void EnableSpawning()
    {
        spawnerManager.SetActive(true);
    }

    public void RoundStart()
    {

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
