using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] List<Level> _levels = new List<Level>();
    [SerializeField] int _currentLevelIndex = 1;
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
        _currentLevelIndex = 1;

    }
    void Start()
    {
        _levels.Add(GenerateNextLevel());
        Debug.Log($"Levels added in Awake. Levels count: {_levels.Count}");
    }


    Level GenerateNextLevel()
    {
        // In 2-4 levels, create an invasion level
        if (_currentLevelIndex % Random.Range(2, 5) == 0)
            return CreateSoloInvasionLevel();

        // In 3-6 levels, create a solo shooter boss level or a solo carrier boss level
        else if (_currentLevelIndex % Random.Range(5, 11) == 0)
        {
            if (Random.value > 0.5f) CreateSoloShooterBossLevel(_spawnerManager.GetShooterBossName());
            else CreateSoloSpawnerBossLevel(_spawnerManager.GetSpawnerBossName());
        }
        // In 5-10 levels, create a Multi Phase Boss level if the defending faction is the Thrax Armada
        else if (_currentLevelIndex % Random.Range(5, 11) == 0 && InvasionManager.Instance.DefendingFaction == FactionType.ThraxArmada)
        {
            return CreateMultiPhaseBossLevel("ThraxBoss2Phase1", "ThraxBoss2Phase2");
        }

        // Otherwise, create a horde level
        return CreateHordeLevel();
    }



    void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.LevelIn)
        {
            _levels[_currentLevelIndex - 1].UpdateLevel();
        }
    }


    public void StartLevel()
    {
        Debug.Log("Starting Level from LevelManager");
        _levels[_currentLevelIndex - 1].StartLevel();
    }


    public void CompleteLevel()
    {
        _currentLevelIndex++;
        Background.Instance.PlayOriginalBackgroundMusic();
        SpawnerManager.Instance.EnemiesToSpawnLeft = 0;
        _levels.Add(GenerateNextLevel());
        GameManager.Instance.ChangeState(GameManager.GameState.LevelEnd);
    }



    public Level CreateSoloInvasionLevel()
    {
        float spawnRateDefending = 0.5f;
        List<Ship> shipsToSpawnInvading = _spawnerManager.DetermineSoloInvadingShips();
        Debug.Log($"Ships to spawn invading: {shipsToSpawnInvading.Count}");
        List<Ship> shipsToSpawnDefending = _spawnerManager.DetermineDefendingShips();
        Debug.Log($"Ships to spawn defending: {shipsToSpawnDefending.Count}");
        int spawnAmountRatio = 1 / 2;
        int amountOfEnemiesLosing = _currentLevelIndex * 10;
        FactionType factionType = InvasionManager.Instance.InvadingFactions[0];

        return new SoloInvasionLevel(
            factionType,
            spawnRateDefending,
            shipsToSpawnInvading,
            shipsToSpawnDefending,
            spawnAmountRatio,
            amountOfEnemiesLosing,
            this,
            _spawnerManager
            );
    }

    public Level CreateSoloSpawnerBossLevel(string bossName)
    {
        float health = _currentLevelIndex * 500f;
        // Every 20 levels, add 1 extra ship to spawn
        int shipsPerSpawn = Mathf.RoundToInt(_currentLevelIndex / 20) + 1;
        float speed = _currentLevelIndex * 1f;
        float spawnRate = Mathf.Max(5f - (_currentLevelIndex * 0.1f), 0.1f);
        float stopDistance = _currentLevelIndex * 2.5f;
        float attackRange = _currentLevelIndex * 2.6f;
        float currencyDrop = _currentLevelIndex * 600f;
        List<Vector3> spawnPoints = SpawnerManager.Instance.SoloBossSpawnPoints;
        return new SoloSpawnerBossLevel(
            health,
            speed,
            spawnRate,
            stopDistance,
            shipsPerSpawn,
            attackRange,
            currencyDrop,
            spawnPoints,
            bossName,
            this,
            _spawnerManager
        );
    }

    public Level CreateHordeLevel()
    {
        int amountOfEnemies = _currentLevelIndex * 10;
        List<Ship> shipsToSpawn = _spawnerManager.DetermineDefendingShips();
        float spawnRate = 0.5f;
        FactionType factionType = FactionType.Syndicates;

        return new HordeLevel(
            amountOfEnemies,
            this,
            shipsToSpawn,
            spawnRate,
            _spawnerManager,
            factionType
        );
    }

    public Level CreateMultiPhaseBossLevel(string bossName, string bossNamePhase2)
    {
        float health = Mathf.Max(_currentLevelIndex * 2000f, 20000f);
        float bulletDamage = Mathf.Max(_currentLevelIndex * 5f, 50f);
        float bulletSpeed = Mathf.Min(Mathf.Max(_currentLevelIndex * 1f, 30f), 40f);
        float firerate = Random.Range(1, 3);
        float speed = Mathf.Max(_currentLevelIndex * 0.5f, 20f);
        float stopDistance = Mathf.Min(Mathf.Max(_currentLevelIndex * 2f, 90f), 120f);
        float attackRange = Mathf.Min(Mathf.Max(_currentLevelIndex * 4, 150f), 180f);
        float fireAngle = Random.Range(15, 21);
        float currencyDrop = _currentLevelIndex * 1000f;
        List<Vector3> spawnPoints = SpawnerManager.Instance.SoloBossSpawnPoints;
        // Choose a random formation type
        FormationType formationType = (FormationType)Random.Range(0, 7);
        Debug.Log($"Formation type: {formationType}");
        // Number of ships in formation is based on the current level index
        int numberOfShipsInFormation = Mathf.Min(_currentLevelIndex * 5, 50);
        float formationRadius = Mathf.Min(Mathf.Max(_currentLevelIndex * 2, 25f), 50f);
        List<string> formationShipName = new List<string>(_spawnerManager.GetFormationShipNames());

        return new MultiPhaseBossLevel(
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
            _spawnerManager,
            formationType,
            numberOfShipsInFormation,
            formationRadius,
            formationShipName,
            bossNamePhase2
        );
    }


    public Level CreateSoloShooterBossLevel(string bossName)
    {
        float health = _currentLevelIndex * 350f;
        float bulletDamage = _currentLevelIndex * 2f;
        float bulletSpeed = _currentLevelIndex * 1.5f;
        float firerate = Random.Range(1, 5);
        float speed = _currentLevelIndex * 0.5f;
        float stopDistance = _currentLevelIndex * 2f;
        float attackRange = _currentLevelIndex * 2.1f;
        float fireAngle = Random.Range(5, 15);
        float currencyDrop = _currentLevelIndex * 400f;
        List<Vector3> spawnPoints = SpawnerManager.Instance.SoloBossSpawnPoints;
        // Choose a random formation type
        FormationType formationType = (FormationType)Random.Range(0, 7);
        Debug.Log($"Formation type: {formationType}");
        // Number of ships in formation is based on the current level index
        int numberOfShipsInFormation = Mathf.Min(_currentLevelIndex * 5, 25);
        float formationRadius = Mathf.Min(Mathf.Max(_currentLevelIndex * 2, 25f), 50f);
        List<string> formationShipName = new List<string>(_spawnerManager.GetFormationShipNames());

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
            _spawnerManager,
            formationType,
            numberOfShipsInFormation,
            formationRadius,
            formationShipName
        );
    }



    public List<Level> Levels { get => _levels; }
    public int CurrentLevelIndex { get => _currentLevelIndex; set => _currentLevelIndex = value; }

}
