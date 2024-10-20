using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SurviveObjective", menuName = "Objectives/SurviveObjective")]

public class SurviveObjective : ObjectiveBase
{
    public float elapsedTime;
    public float requiredTime;
    List<string> shipsToSpawn = new List<string>();
    public override void Initialize()
    {
        objectiveName = "Survive";
        isObjectiveCompleted = false;
        isObjectiveFailed = false;
        elapsedTime = requiredTime;
        rewardPoints = (LevelManager.Instance.CurrentLevelIndex * 200) + ((int)requiredTime * 10);
        shipsToSpawn = new List<string>(SpawnerManager.Instance.GetFormationShipNames());
        GameManager.Instance.StartCoroutine(SpawnShipsInfinitely());
    }

    public override void UpdateObjective()
    {
        elapsedTime -= Time.deltaTime;
        if (elapsedTime <= 0)
        {
            elapsedTime = 0;
            CompleteObjective();

        }
        objectiveDescription = "Survive for " + Mathf.Round(elapsedTime) + " seconds!";
        ObjectiveManager.Instance.UpdateObjectivesUI();
    }

    public override void CompleteObjective()
    {
        UIManager.Instance.MidScreenWarningText("Bonus Objective succeeded!", 3f);
        isObjectiveCompleted = true;
        // Notify ObjectiveManager of completion
        objectiveDescription = "The Ancient Relic has erupted!";
        EventManager.ObjectiveCompletedEvent();
        // Force UI to update
        ObjectiveManager.Instance.RemoveObjective("Survive");
        UIManager.Instance.RemoveObjectiveFromUI("Survive");
        ObjectiveManager.Instance.UpdateObjectivesUI();
        base.CompleteObjective();
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
    }
    public override void FailObjective()
    {
        isObjectiveFailed = true;
        UIManager.Instance.MidScreenWarningText("Bonus Objective failed!", 3f);
        ObjectiveManager.Instance.RemoveObjective("Survive");
        UIManager.Instance.RemoveObjectiveFromUI("Survive");
        ObjectiveManager.Instance.HandleObjectiveFailure(this);

    }
    IEnumerator SpawnShipsInfinitely()
    {
        while (elapsedTime > 0)
        {
            SpawnerManager.Instance.SpawnShip(shipsToSpawn[Random.Range(0, shipsToSpawn.Count)], PlayerManager.Instance.transform.position + new Vector3(Random.Range(-200, 200), Random.Range(-200, 200), 0), Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }

    }
}
