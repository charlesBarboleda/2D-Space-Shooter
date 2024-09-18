using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] List<Level> _levels = new List<Level>();
    [SerializeField] int _currentLevelIndex;
    SpawnerManager _spawnerManager;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            DontDestroyOnLoad(this);
        }
        else
        {
            Instance = this;
        }
        _spawnerManager = GetComponent<SpawnerManager>();

    }
    void Start()
    {
        _levels.Add(CreateInvasionLevel(
            1f, // Spawn Rate Losing
            SpawnerManager.Instance.shipNamesLate3, // Ships to Spawn Winning
            SpawnerManager.Instance.shipNamesCrimsonFleet, // Ships to Spawn Losing
            5 / 1, // Spawn Amount Ratio
            20)); // Amount of Enemies Losing

        _levels.Add(CreateHordeLevel(10, SpawnerManager.Instance.shipNamesCrimsonFleet, 5f));

        _levels.Add(CreateSoloShooterBossLevel(
            _currentLevelIndex * 350f, // Health
            _currentLevelIndex * 2f, // Bullet Damage
            _currentLevelIndex * 1.5f, // Bullet Speed
            Random.Range(1, 5), // Firerate
            _currentLevelIndex * 1, // Speed
            _currentLevelIndex * 2, // Stop Distance
            _currentLevelIndex * 2.1f, // Attack Range
            Random.Range(5, 15), // Fire Angle
            _currentLevelIndex * 400f, // Currency Drop
            SpawnerManager.Instance.SoloBossSpawnPoints, // Spawn Points
            "LargeShip" // Boss Name Used to Spawn the Boss

        ));


        Debug.Log($"Levels added in Awake. Levels count: {_levels.Count}");
    }

    void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.LevelIn)
        {
            _levels[_currentLevelIndex].UpdateLevel();
        }
    }


    public void StartLevel()
    {
        Debug.Log("Starting Level from LevelManager");
        _levels[_currentLevelIndex].StartLevel();
    }

    public void CompleteLevel()
    {
        _currentLevelIndex++;
        GameManager.Instance.ChangeState(GameManager.GameState.LevelEnd);
    }

    public Level CreateInvasionLevel(float spawnRateLosing, List<Ship> shipsToSpawnWinning, List<Ship> shipsToSpawnLosing, int spawnAmountRatio, int amountOfEnemiesLosing)
    {
        return new InvasionLevel(
            spawnRateLosing,
            shipsToSpawnWinning,
            shipsToSpawnLosing,
            spawnAmountRatio,
            amountOfEnemiesLosing,
            this,
            _spawnerManager);
    }

    public Level CreateHordeLevel(int amountOfEnemies, List<Ship> shipsToSpawn, float spawnRate)
    {
        return new HordeLevel(amountOfEnemies, this, shipsToSpawn, spawnRate, _spawnerManager);
    }

    public Level CreateSoloShooterBossLevel(float health, float bulletDamage, float bulletSpeed, float firerate, float speed, float stopDistance, float attackRange, float fireAngle, float currencyDrop, List<Vector3> spawnPoints, string bossName)
    {
        return new SoloShooterBossLevel(
            health,
            bulletDamage,
            bulletSpeed,
            firerate,
            speed,
            stopDistance,
            attackRange,
            fireAngle,
            currencyDrop,
            spawnPoints,
            bossName,
            this,
            _spawnerManager
        );
    }


    public List<Level> Levels { get => _levels; }
    public int CurrentLevelIndex { get => _currentLevelIndex; set => _currentLevelIndex = value; }

}
