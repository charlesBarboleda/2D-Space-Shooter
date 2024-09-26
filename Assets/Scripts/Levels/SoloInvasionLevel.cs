using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloInvasionLevel : Level
{
    int _amountOfEnemiesDefending;
    int _spawnAmountRatio;
    List<Ship> _shipsToSpawnInvading;
    Dictionary<string, GameObject> _totalInvaders = new Dictionary<string, GameObject>();
    float _spawnRateLosing;
    LevelManager _levelManager;
    SpawnerManager _spawnerManager;




    public SoloInvasionLevel(FactionType factionType, float spawnRateLosing, List<Ship> shipsToSpawnInvading, List<Ship> shipsToSpawnDefending, int spawnAmountRatio, int amountOfEnemiesDefending, LevelManager levelManager, SpawnerManager spawnerManager)
    {
        _factionType = factionType;
        _leveltype = LevelType.Invasion;
        _spawnRateLosing = spawnRateLosing;
        _shipsToSpawn = shipsToSpawnDefending;
        _shipsToSpawnInvading = shipsToSpawnInvading;
        _spawnAmountRatio = spawnAmountRatio;
        _amountOfEnemiesDefending = amountOfEnemiesDefending;
        _levelManager = levelManager;
        _spawnerManager = spawnerManager;
    }
    public override void StartLevel()
    {
        Debug.Log("Starting Invasion Level");

        EventManager.OnEnemyDestroyed += RegisterInvaderKill;
        // Calculate the amount of enemies to spawn
        _spawnerManager.EnemiesToSpawnLeft = _amountOfEnemiesDefending + (_amountOfEnemiesDefending * _spawnAmountRatio);
        // Start the spawning of the losing enemies
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawn, _spawnRateLosing, _amountOfEnemiesDefending, 200f, shipList));
        // Start the spawning of the winning enemies after a delay
        _spawnerManager.StartCoroutine(DelayedSpawn());
    }

    public override void UpdateLevel()
    {
        Debug.Log("Enemies to spawn: " + _spawnerManager.EnemiesToSpawnLeft);
        Debug.Log("Enemies spawned: " + _spawnerManager.EnemiesList.Count);
        Debug.Log("Total invaders: " + _totalInvaders.Count);
        if (_spawnerManager.EnemiesList.Count <= 0 && _spawnerManager.EnemiesToSpawnLeft <= 0)
        {
            CompleteLevel();
        }
        // Check if the invasion has lost
        if (InvasionLost())
        {
            Debug.Log("The Invasion has lost");
            // Add UI event
        }
        // Check if the invasion has won
        else if (InvasionWon())
        {
            EventManager.FactionInvasionWonEvent(_factionType);
            Debug.Log("The Invasion has won");
        }

    }
    public override void CompleteLevel()
    {
        Debug.Log("Completing Invasion Level");
        EventManager.OnEnemyDestroyed -= RegisterInvaderKill;
        _levelManager.CompleteLevel();
    }

    public bool InvasionLost()
    {
        if (_totalInvaders.Count <= 0 && _spawnerManager.EnemiesToSpawnLeft <= 0)
        {
            return true;
        }
        return false;
    }

    public bool InvasionWon()
    {
        if (_totalInvaders.Count > 0 && _spawnerManager.EnemiesToSpawnLeft <= 0 && _spawnerManager.EnemiesList.Count - _totalInvaders.Count <= 0)
        {
            return true;
        }
        return false;
    }



    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(Random.Range(10f, 20f));
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawnInvading, _spawnRateLosing / 2, _amountOfEnemiesDefending * _spawnAmountRatio, 300f, _totalInvaders));
    }

    public void RegisterInvaderKill(string invaderID, GameObject invader)
    {
        // Check if the destroyed invader exists in the dictionary
        if (_totalInvaders.ContainsKey(invaderID))
        {
            _totalInvaders.Remove(invaderID);
            Debug.Log($"Invader {invaderID} destroyed. Remaining invaders: {_totalInvaders.Count}");
        }
    }


}
