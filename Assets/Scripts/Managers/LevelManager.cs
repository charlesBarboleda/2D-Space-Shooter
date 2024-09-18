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
        _levels.Add(CreateHordeLevel(10, SpawnerManager.Instance.shipNamesCrimsonFleet, 5f));
        _levels.Add(CreateHordeLevel(20, SpawnerManager.Instance.shipNamesCrimsonFleet, 5f));
        _levels.Add(CreateHordeLevel(30, SpawnerManager.Instance.shipNamesCrimsonFleet, 5f));
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

    }

    public Level CreateHordeLevel(int amountOfEnemies, List<Ship> shipsToSpawn, float spawnRate)
    {
        return new HordeLevel(amountOfEnemies, this, shipsToSpawn, spawnRate, _spawnerManager);
    }


    public List<Level> Levels { get => _levels; }
    public int CurrentLevelIndex { get => _currentLevelIndex; set => _currentLevelIndex = value; }

}
