using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserUltimateObjective", menuName = "Objectives/LaserUltimateObjective")]
public class LaserUltimateObjective : ObjectiveBase
{
    public int hitsRequired = 5000;

    public override void Initialize()
    {
        objectiveName = "Laser";
        hitsRequired = 5000;
        rewardPoints = 0;
        EventManager.OnLaserHit += OnLaserHit;
        objectiveDescription = $"Hit {hitsRequired} times with your Laser ability to unlock a special reward!";
    }

    public override void UpdateObjective()
    {
        if (hitsRequired <= 0)
        {
            hitsRequired = 0;
            CompleteObjective();
        }
        objectiveDescription = $"Hit {hitsRequired} times with your Laser ability to unlock a special reward!";
        ObjectiveManager.Instance.UpdateObjectivesUI();
    }
    public override void FailObjective()
    {
        // Doesn't need implementation
    }
    public override void CompleteObjective()
    {
        EventManager.OnLaserHit -= OnLaserHit;
        // Notify ObjectiveManager of completion
        objectiveDescription = "You have unlocked the Laser Ultimate ability";
        PlayerManager.GetInstance().AbilityHolder().abilities.Find(ability => ability is AbilityLaser).isUltimateUnlocked = true;
        UIManager.Instance.laserUltIconFill.gameObject.SetActive(true);
        UIManager.Instance.MidScreenWarningText("Unlocked Laser Ultimate", 3f);
        ObjectiveManager.Instance.RemoveObjective("Laser");
        UIManager.Instance.RemoveObjectiveFromUI("Laser");
        EventManager.ObjectiveCompletedEvent();
        // Force UI to update
        ObjectiveManager.Instance.UpdateObjectivesUI();
        base.CompleteObjective();
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
        isObjectiveCompleted = true;

    }

    void OnLaserHit()
    {
        hitsRequired--;
    }
}
