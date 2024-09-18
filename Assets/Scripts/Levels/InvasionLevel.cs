using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvasionLevel : Level
{
    int _amountOfEnemiesLosing;
    int _spawnAmountRatio;
    List<Ship> _shipsToSpawnWinning;
    float _spawnRateLosing;
    LevelManager _levelManager;
    SpawnerManager _spawnerManager;


    public InvasionLevel(float spawnRateLosing, List<Ship> shipsToSpawnWinning, List<Ship> shipsToSpawnLosing, int spawnAmountRatio, int amountOfEnemiesLosing, LevelManager levelManager, SpawnerManager spawnerManager)
    {
        _leveltype = LevelType.Invasion;
        _spawnRateLosing = spawnRateLosing;
        _shipsToSpawn = shipsToSpawnLosing;
        _shipsToSpawnWinning = shipsToSpawnWinning;
        _spawnAmountRatio = spawnAmountRatio;
        _amountOfEnemiesLosing = amountOfEnemiesLosing;
        _levelManager = levelManager;
        _spawnerManager = spawnerManager;
    }
    public override void StartLevel()
    {
        Debug.Log("Starting Invasion Level");

        // Start the spawning of the losing enemies
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawn, _spawnRateLosing, _amountOfEnemiesLosing, 200f));
        // Start the spawning of the winning enemies after a delay
        _spawnerManager.StartCoroutine(DelayedSpawn());
    }

    public override void UpdateLevel()
    {
        Debug.Log("Updating Invasion Level");
        if (_spawnerManager.EnemiesList.Count == 0)
        {
            CompleteLevel();
        }
    }

    public override void CompleteLevel()
    {
        Debug.Log("Completing Invasion Level");
        _levelManager.CompleteLevel();
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(Random.Range(10f, 20f));
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawnWinning, _spawnRateLosing / 2, _amountOfEnemiesLosing * _spawnAmountRatio, 300f));
    }


}
