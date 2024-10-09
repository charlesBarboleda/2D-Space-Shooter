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
        // 10% chance to start a random objective
        if (Random.value < 0.1f)
        {
            _spawnerManager.StartCoroutine(StartRandomObjective());
        }
        _spawnerManager.ResetRound();
        _spawnerManager.EnemiesToSpawnLeft = _amountOfEnemiesToSpawn;
        Debug.Log("Starting Horde Level");
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawn, _spawnRate, _amountOfEnemiesToSpawn, 200f, shipList));
        Debug.Log("Spawning Enemies");
    }

    IEnumerator StartRandomObjective()
    {
        yield return new WaitForSeconds(Random.Range(5, 15));
        ObjectiveBase randomObjective = ObjectiveManager.Instance.GetRandomObjectiveFromPool();
        if (randomObjective != null)
        {
            _levelObjectives.Add(randomObjective);
        }
        ObjectiveManager.Instance.StartObjectivesForLevel(this);
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
        _spawnerManager.StopAllCoroutines();
        Debug.Log("Completing Horde Level");
        _levelManager.CompleteLevel();
        _spawnerManager.EnemiesToSpawnLeft = 0;
        _spawnerManager.EnemiesList.Clear();
    }


}
