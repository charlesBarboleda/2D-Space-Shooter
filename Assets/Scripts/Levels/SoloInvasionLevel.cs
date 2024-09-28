using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloInvasionLevel : Level
{
    int _amountOfEnemiesDefending;
    int _spawnAmountRatio;
    List<Ship> _shipsToSpawnInvading;
    Dictionary<string, GameObject> _totalInvaders = new Dictionary<string, GameObject>();
    float _spawnRateDefending;
    LevelManager _levelManager;
    SpawnerManager _spawnerManager;
    bool _invasionLost = false;
    bool _invasionWon = false;




    public SoloInvasionLevel(FactionType factionType, float spawnRateDefending, List<Ship> shipsToSpawnInvading, List<Ship> shipsToSpawnDefending, int spawnAmountRatio, int amountOfEnemiesDefending, LevelManager levelManager, SpawnerManager spawnerManager)
    {
        _factionType = factionType;
        _leveltype = LevelType.Invasion;
        _spawnRateDefending = spawnRateDefending;
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
        // Start the spawning of the defending enemies
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawn, _spawnRateDefending, _amountOfEnemiesDefending, 200f, shipList));
        // Start the proper objectives
        // ObjectivesManager.Instance.StartObjectives();

        // Start the spawning of the winning enemies after a delay
        _spawnerManager.StartCoroutine(DelayedSpawn());
    }

    public override void UpdateLevel()
    {
        if (_spawnerManager.EnemiesList.Count <= 0 && _spawnerManager.EnemiesToSpawnLeft <= 0)
        {
            CompleteLevel();
        }
        // Check if the invasion has lost
        else if (InvasionLost())
        {
            if (!_invasionLost)
            {
                _invasionLost = true;
                Debug.Log("The invasion has lost");
            }

        }
        // Check if the invasion has won
        else if (InvasionWon() && !_invasionWon)
        {
            _invasionWon = true;
            EventManager.FactionInvasionWonEvent(_factionType);
            Debug.Log("The invasion has won");
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
        yield return new WaitForSeconds(Random.Range(20f, 30f));
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawnInvading, _spawnRateDefending / 2, (int)Mathf.Round(_amountOfEnemiesDefending / 2), 250f, _totalInvaders));
        UIManager.Instance.MidScreenWarningText($"An invasion is occuring!", 3.5f);

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

    public Dictionary<string, GameObject> TotalInvaders { get => _totalInvaders; }


}
