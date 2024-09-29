using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InvasionObjective", menuName = "Objective/InvasionObjective")]
public class InvasionObjective : ObjectiveBase
{
    List<GameObject> _defendingShips;
    List<GameObject> _invadingShips;
    int _shipsToSpawnCount;
    SpawnerManager _spawnerManager;

    public override void Initialize()
    {
        _spawnerManager = SpawnerManager.Instance;
        _defendingShips = _spawnerManager.DefendingShipsList.ConvertAll(ship => ship.gameObject); // Convert Ships to GameObjects
        _invadingShips = _spawnerManager.SpecialEnemiesList; // Invaders list from the level
        _shipsToSpawnCount = _spawnerManager.EnemiesToSpawnLeft;

        objectiveDescription = $"Invasion imminent! Defend the {InvasionManager.Instance.DefendingFaction} OR destroy the {InvasionManager.Instance.InvadingFactions[0]}!";
    }

    public override void UpdateObjective()
    {
        if (AllInvadingShipsDestroyed())
        {
            CompleteObjective(true); // Defenders won
        }
        else if (AllDefendingShipsDestroyed())
        {
            CompleteObjective(false); // Invaders won
        }
    }

    private void CompleteObjective(bool defendersWin)
    {
        isObjectiveCompleted = true;
        string resultMessage = defendersWin ? "Objective Completed: Defenders won!" : "Objective Completed: Invaders won!";
        objectiveDescription = resultMessage;

        // Notify ObjectiveManager of completion
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
    }

    private bool AllInvadingShipsDestroyed()
    {
        return _invadingShips.Count == 0 && _shipsToSpawnCount == 0;
    }

    private bool AllDefendingShipsDestroyed()
    {
        return _defendingShips.Count == 0 && _shipsToSpawnCount == 0;
    }

    public override void ResetObjective()
    {
        base.ResetObjective();
        _defendingShips.Clear();
        _invadingShips.Clear();
    }
}
