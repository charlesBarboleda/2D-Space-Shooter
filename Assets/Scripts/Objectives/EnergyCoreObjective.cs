using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyCoreObjective", menuName = "Objectives/EnergyCoreObjective")]

public class EnergyCoreObjective : ObjectiveBase
{
    public int coresNeeded = 2;
    public float timeRestriction = 180f;
    GameObject energyBuilding;
    float elapsedTime;

    public override void Initialize()
    {
        objectiveName = "Push";
        isObjectiveCompleted = false;
        isObjectiveFailed = false;
        Vector3 spawnPoint = SpawnerManager.Instance.GetRandomPositionOutsideBuildings();
        energyBuilding = ObjectPooler.Instance.SpawnFromPool("EnergyCoreBuilding", spawnPoint, Quaternion.identity);
        NavMeshScript.Instance.UpdateNavMesh();
        EnergyCoreBuilding script = energyBuilding.GetComponent<EnergyCoreBuilding>();
        script.coresNeeded = coresNeeded;
        for (int i = 0; i < coresNeeded; i++)
        {
            Vector3 spawn = SpawnerManager.Instance.GetRandomPositionOutsideBuildings();
            ObjectPooler.Instance.SpawnFromPool("EnergyCore", spawn, Quaternion.identity);
        }
        rewardPoints = LevelManager.Instance.CurrentLevelIndex * 100 * coresNeeded;

        elapsedTime = timeRestriction;

        EventManager.OnCoreEnergize += DecreaseCoreRequirement;
    }

    public override void UpdateObjective()
    {
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            if (coresNeeded <= 0)
            {
                CompleteObjective();
            }
            objectiveDescription = $"{coresNeeded} Energy Core has spawned around the map; Push them into the Ancient Relic to activate its power!: " + Mathf.Round(elapsedTime) + " seconds";
            ObjectiveManager.Instance.UpdateObjectivesUI();

        }
        else
        {
            FailObjective();
            energyBuilding.SetActive(false);
        }
    }
    public override void CompleteObjective()
    {
        isObjectiveCompleted = true;
        // Notify ObjectiveManager of completion
        objectiveDescription = "The Ancient Relic has erupted!";
        UIManager.Instance.MidScreenWarningText("Bonus Objective succeeded!", 3f);
        EventManager.ObjectiveCompletedEvent();
        // Force UI to update
        ObjectiveManager.Instance.RemoveObjective("Push");
        UIManager.Instance.RemoveObjectiveFromUI("Push");
        ObjectiveManager.Instance.UpdateObjectivesUI();
        base.CompleteObjective();
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);

        EventManager.OnCoreEnergize -= DecreaseCoreRequirement;

    }
    public override void FailObjective()
    {
        isObjectiveFailed = true;

        objectiveDescription = "The Ancient Relic has deactivated";
        ObjectiveManager.Instance.RemoveObjective("Push");
        UIManager.Instance.RemoveObjectiveFromUI("Push");
        ObjectiveManager.Instance.HandleObjectiveFailure(this);

        EventManager.OnCoreEnergize -= DecreaseCoreRequirement;

    }

    void DecreaseCoreRequirement()
    {
        coresNeeded--;
    }
}

