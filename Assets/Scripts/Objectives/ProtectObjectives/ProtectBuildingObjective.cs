using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProtectBuildingObjective", menuName = "Objectives/ProtectBuildingObjective", order = 1)]
public class ProtectBuildingObjective : Objective
{
    [SerializeField] float _requiredTime;
    [SerializeField] float _elapsedTime;
    [SerializeField] float _health;
    [SerializeField] List<string> buildingNames;
    [SerializeField] List<Transform> _spawnPoints;
    GameObject _building;

    public override void InitObjective()
    {

        foreach (string buildingName in buildingNames)
        {
            _building = ObjectPooler.Instance.SpawnFromPool(buildingName, _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position, Quaternion.identity);
            GameManager.Instance.AddEnemy(_building);
            Health buildingHealth = _building.GetComponent<Health>();
            if (_building.GetComponent<Health>() != null)
            {
                buildingHealth.CurrentHealth = _health;
                buildingHealth.MaxHealth = _health;

            }
        }
        ObjectiveID = Guid.NewGuid().ToString();
        _elapsedTime = _requiredTime;
        IsCompleted = false;
        IsFailed = false;
        IsActive = true;
    }

    public override void UpdateObjective()
    {
        if (IsCompleted || IsFailed) return;
        if (!_building.activeSelf) FailedObjective();

        if (_elapsedTime <= 0 && _building.activeSelf)
        {
            _building.GetComponent<Building>().TeleportAway();
            CompleteObjective();
        }
        _elapsedTime -= Time.deltaTime;

        if (IsCompleted) ObjectiveDescription = "Objective Completed";
        if (IsFailed) ObjectiveDescription = "Objective Failed";
        if (IsActive && !IsCompleted && !IsFailed) ObjectiveDescription = "Protect the Ancient building for " + Mathf.Round(_elapsedTime) + " seconds";
    }
    public override void CompleteObjective()
    {

        MarkObjectiveCompleted();
    }

    public override void FailedObjective()
    {
        MarkObjectiveFailed();
    }

}
