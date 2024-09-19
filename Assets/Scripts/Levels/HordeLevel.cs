using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeLevel : Level
{
    int _amountOfEnemiesToSpawn;
    float _spawnRate;
    LevelManager _levelManager;
    SpawnerManager _spawnerManager;


    public HordeLevel(int amountOfEnemies, LevelManager levelManager, List<Ship> shipsToSpawn, float spawnRate, SpawnerManager spawnerManager, FactionType factionType)
    {
        _leveltype = LevelType.Horde;
        _factionType = factionType;
        _amountOfEnemiesToSpawn = amountOfEnemies;
        _levelManager = levelManager;
        _spawnerManager = spawnerManager;
        _spawnRate = spawnRate;
        _shipsToSpawn = shipsToSpawn;
    }
    public override void StartLevel()
    {
        _spawnerManager.EnemiesToSpawnLeft = _amountOfEnemiesToSpawn;
        Debug.Log("Starting Horde Level");
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawn, _spawnRate, _amountOfEnemiesToSpawn, 200f, shipList));
        Debug.Log("Spawning Enemies");
    }

    public override void UpdateLevel()
    {
        if (_spawnerManager.EnemiesList.Count <= 0 && _spawnerManager.EnemiesToSpawnLeft <= 0)
        {
            CompleteLevel();
        }
    }

    public override void CompleteLevel()
    {
        Debug.Log("Completing Horde Level");
        _levelManager.CompleteLevel();
        _spawnerManager.EnemiesToSpawnLeft = 0;
        _spawnerManager.EnemiesList.Clear();
    }


}
