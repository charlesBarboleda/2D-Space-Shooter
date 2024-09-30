using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "InvasionObjective", menuName = "Objective/InvasionObjective")]
public class InvasionObjective : ObjectiveBase
{
    List<GameObject> _defendingShips;
    List<GameObject> _invadingShips;
    int _shipsToSpawnCount;
    bool _hasStartedInvasion = false;
    SpawnerManager _spawnerManager;

    public override void Initialize()
    {
        EventManager.OnEnemyDestroyed += RegisterShipDestroyed;
        _spawnerManager = SpawnerManager.Instance;
        _defendingShips = _spawnerManager.DefendingShipsList;
        _invadingShips = _spawnerManager.SpecialEnemiesList; // Invaders list from the level
        _shipsToSpawnCount = _spawnerManager.EnemiesToSpawnLeft;

        objectiveDescription = $"Invasion imminent! Eliminate the {InvasionManager.Instance.DefendingFaction} OR Destroy the {InvasionManager.Instance.InvadingFactions[0]}!";
        _spawnerManager.StartCoroutine(InvasionPrepStart());
    }

    public override void UpdateObjective()
    {
        Debug.Log("_invadingShips.Count: " + _invadingShips.Count);
        Debug.Log("_defendingShips.Count: " + _defendingShips.Count);
        Debug.Log("_shipsToSpawnCount: " + _shipsToSpawnCount);
        if (_hasStartedInvasion)
        {
            if (AllInvadingShipsDestroyed())
            {
                Debug.Log("Invaders lost");
                CompleteObjective(true); // Defenders won
            }
            if (AllDefendingShipsDestroyed())
            {
                Debug.Log("Defenders lost");
                CompleteObjective(false); // Invaders won
            }
        }
        ObjectiveManager.Instance.UpdateObjectivesUI();
    }

    IEnumerator InvasionPrepStart()
    {
        yield return new WaitForSeconds(10f);
        if (!_hasStartedInvasion)
        {
            _hasStartedInvasion = true;
        }
    }

    private void CompleteObjective(bool defendersWin)
    {
        isObjectiveCompleted = true;
        string resultMessage = defendersWin ? $"Invasion over! The {InvasionManager.Instance.InvadingFactions[0]} have lost!" : $"Invasion over! The {InvasionManager.Instance.DefendingFaction} have been eliminated!";
        objectiveDescription = resultMessage;

        // Force UI to update
        ObjectiveManager.Instance.UpdateObjectivesUI();
        Debug.Log("Updated Objective from Invasion Objective");

        // Notify ObjectiveManager of completion
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
    }

    private bool AllInvadingShipsDestroyed()
    {
        return _invadingShips.Count <= 0 && _shipsToSpawnCount <= 0 && _hasStartedInvasion;
    }

    private bool AllDefendingShipsDestroyed()
    {
        return _defendingShips.Count <= 0 && _shipsToSpawnCount <= 0;
    }

    private void RegisterShipDestroyed(GameObject ship)
    {
        if (_defendingShips.Contains(ship))
        {
            _defendingShips.Remove(ship);
        }
        else if (_invadingShips.Contains(ship))
        {
            _invadingShips.Remove(ship);
        }
    }
    public override void FailObjective()
    {
        // Not implemented since this objective does not have a fail state
    }

    public override void ResetObjective()
    {
        base.ResetObjective();
        _defendingShips.Clear();
        _invadingShips.Clear();
    }
}
