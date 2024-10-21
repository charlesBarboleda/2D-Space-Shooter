using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CometObjective", menuName = "Objectives/CometObjective")]
public class CometObjective : ObjectiveBase
{
    public int amountToDestroy = 5;
    public float elapsedTime;
    public float requiredTime;

    public override void Initialize()
    {
        objectiveName = "Comet";
        amountToDestroy = Random.Range(3, 10);
        requiredTime = amountToDestroy * 60;
        elapsedTime = requiredTime;
        rewardPoints = amountToDestroy * 500;
        isObjectiveCompleted = false;
        isObjectiveFailed = false;


        EventManager.OnCometDestruction += OnCometDestruction;
    }
    public override void UpdateObjective()
    {
        elapsedTime -= Time.deltaTime;
        if (amountToDestroy <= 0)
        {
            CompleteObjective();
        }
        if (elapsedTime <= 0 || SpawnerManager.Instance.cometsCount.Count <= 0)
        {
            FailObjective();
        }
        objectiveDescription = "Destroy " + amountToDestroy + " comets before the round ends or the time runs out in " + Mathf.Round(elapsedTime) + " seconds!";
        ObjectiveManager.Instance.UpdateObjectivesUI();
    }

    public override void FailObjective()
    {
        isObjectiveFailed = true;
        UIManager.Instance.MidScreenWarningText("Bonus Objective failed!", 3f);
        ObjectiveManager.Instance.RemoveObjective("Comet");
        UIManager.Instance.RemoveObjectiveFromUI("Comet");
        ObjectiveManager.Instance.HandleObjectiveFailure(this);
    }
    public override void CompleteObjective()
    {
        // Notify ObjectiveManager of completion
        objectiveDescription = "The comets have been destroyed";
        UIManager.Instance.MidScreenWarningText("Bonus Objective succeeded!", 3f);
        EventManager.ObjectiveCompletedEvent();
        // Force UI to update
        ObjectiveManager.Instance.RemoveObjective("Comet");
        UIManager.Instance.RemoveObjectiveFromUI("Comet");
        ObjectiveManager.Instance.UpdateObjectivesUI();
        base.CompleteObjective();
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
        isObjectiveCompleted = true;
    }

    void OnCometDestruction()
    {
        amountToDestroy--;
    }

}

