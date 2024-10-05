using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EscortObjective", menuName = "Objectives/EscortObjective")]
public class EscortObjective : ObjectiveBase
{
    GameObject _cargoShip;
    Health _cargoShipHealth;
    List<Vector2> _wayPoints = new List<Vector2>();
    [SerializeField] int _waypointsAmount = 3;
    Vector2 _mapMinSize = new Vector2(-200, -200);
    Vector2 _mapMaxSize = new Vector2(200, 200);
    public override void Initialize()
    {

        _wayPoints = GeneratePath(_waypointsAmount);
        _cargoShip = ObjectPooler.Instance.SpawnFromPool("CargoShip", _wayPoints[0], Quaternion.identity);
        SpawnerManager.Instance.EnemiesList.Add(_cargoShip);
        _cargoShipHealth = _cargoShip.GetComponent<Health>();
        _cargoShipHealth.MaxHealth = LevelManager.Instance.CurrentLevelIndex * 1000;
        _cargoShipHealth.CurrentHealth = _cargoShipHealth.MaxHealth;
        rewardPoints = (int)_cargoShipHealth.MaxHealth / 2;
        _cargoShip.GetComponent<CargoShip>().SetWaypoints(_wayPoints);
        objectiveDescription = "Escort the Cargo Ship to its destination";
        ObjectiveManager.Instance.UpdateObjectivesUI();
    }

    public override void UpdateObjective()
    {
        if (_cargoShipHealth.isDead)
        {
            FailObjective();
        }
        else if (_cargoShip == null || !_cargoShip.activeInHierarchy)
        {
            CompleteObjective();
        }
    }

    public override void CompleteObjective()
    {
        // Notify ObjectiveManager of completion
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
        base.CompleteObjective();
        isObjectiveCompleted = true;
        SpawnerManager.Instance.EnemiesList.Remove(_cargoShip);

        objectiveDescription = "The Cargo Ship has reached its destination and sucessfully escaped!";
        // Force UI to update
        Debug.Log("Updated Objective from Invasion Objective");

        ObjectiveManager.Instance.UpdateObjectivesUI();

    }
    public override void FailObjective()
    {
        isObjectiveFailed = true;
        SpawnerManager.Instance.EnemiesList.Remove(_cargoShip);
        objectiveDescription = "The Cargo Ship has been destroyed";
        ObjectiveManager.Instance.UpdateObjectivesUI();
        ObjectiveManager.Instance.HandleObjectiveFailure(this);
    }

    Vector2 ClampToMapBounds(Vector2 waypoint)
    {
        float x = Mathf.Clamp(waypoint.x, _mapMinSize.x, _mapMaxSize.x);
        float y = Mathf.Clamp(waypoint.y, _mapMinSize.y, _mapMaxSize.y);
        return new Vector2(x, y);
    }
    Vector2 GenerateRandomWaypoint()
    {
        float x = Random.Range(_mapMinSize.x, _mapMaxSize.x);
        float y = Random.Range(_mapMinSize.y, _mapMaxSize.y);
        return new Vector2(x, y);
    }

    List<Vector2> GeneratePath(int amountofWaypoints)
    {
        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < amountofWaypoints; i++)
        {
            path.Add(ClampToMapBounds(GenerateRandomWaypoint()));
        }
        return path;
    }

}
