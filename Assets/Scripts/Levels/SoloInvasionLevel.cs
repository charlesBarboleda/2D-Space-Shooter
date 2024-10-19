using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloInvasionLevel : Level
{
    int _amountOfEnemiesDefending;
    int _spawnAmountRatio;
    List<Ship> _shipsToSpawnInvading;
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

        EventManager.OnEnemyDestroyed += RegisterInvaderKill;
        _spawnerManager.ResetRound();
        _spawnerManager.EnemiesToSpawnLeft = _amountOfEnemiesDefending + (_amountOfEnemiesDefending * _spawnAmountRatio);
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawn, _spawnRateDefending, _amountOfEnemiesDefending, 300f, _spawnerManager.DefendingShipsList));
        _spawnerManager.StartCoroutine(DelayedSpawn());
    }



    public override void UpdateLevel()
    {
        if (_spawnerManager.EnemiesList.Count <= 0 && _spawnerManager.EnemiesToSpawnLeft <= 0)
        {
            CompleteLevel();
        }
        else if (InvasionLost())
        {
            if (!_invasionLost)
            {
                _invasionLost = true;
                Debug.Log("The invasion has lost");
            }
        }
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

    private bool InvasionLost()
    {
        return _spawnerManager.SpecialEnemiesList.Count <= 0 && _spawnerManager.EnemiesToSpawnLeft <= 0;
    }

    private bool InvasionWon()
    {
        return _spawnerManager.DefendingShipsList.Count <= 0 && _spawnerManager.EnemiesToSpawnLeft <= 0;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(Random.Range(15, 25));
        _spawnerManager.StartCoroutine(_spawnerManager.SpawnEnemiesOverTime(_shipsToSpawnInvading, _spawnRateDefending / 2, (int)Mathf.Round(_amountOfEnemiesDefending / 2), 350f, _spawnerManager.SpecialEnemiesList));
        Background.Instance.PlayInvasionMusic();
        UIManager.Instance.MidScreenWarningText($"An invasion is occuring!", 3.5f);
        yield return new WaitForSeconds(3.5f);
        ObjectiveBase invasionObjective = ObjectiveManager.Instance.GetObjectiveFromPool("InvasionObjective");
        if (invasionObjective != null)
        {

            _levelObjectives.Add(invasionObjective);
        }
        ObjectiveManager.Instance.StartObjectivesForLevel(this);
    }

    public void RegisterInvaderKill(GameObject invader)
    {
        if (_spawnerManager.SpecialEnemiesList.Contains(invader))
        {
            _spawnerManager.SpecialEnemiesList.Remove(invader);
        }
    }

}
