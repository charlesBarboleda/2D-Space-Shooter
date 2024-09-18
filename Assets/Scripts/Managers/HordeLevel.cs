using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeLevel : Level
{
    int _amountOfEnemiesToSpawn;
    float _spawnRate;
    LevelManager _levelManager;
    SpawnerManager _spawnerManager;


    public HordeLevel(int amountOfEnemies, LevelManager levelManager, List<Ship> shipsToSpawn, float spawnRate, SpawnerManager spawnerManager)
    {
        _leveltype = LevelType.Horde;
        _amountOfEnemiesToSpawn = amountOfEnemies;
        _levelManager = levelManager;
        _spawnerManager = spawnerManager;
        _spawnRate = spawnRate;
        _shipsToSpawn = shipsToSpawn;
    }
    public override void StartLevel()
    {
        Debug.Log("Starting Horde Level");
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawn, 1f, _amountOfEnemiesToSpawn));

    }

    public override void UpdateLevel()
    {
        Debug.Log("Updating Horde Level");
        if (_spawnerManager.EnemiesList.Count == 0)
        {
            CompleteLevel();
        }
    }

    public override void CompleteLevel()
    {
        Debug.Log("Completing Horde Level");
        _levelManager.CompleteLevel();
    }


}
