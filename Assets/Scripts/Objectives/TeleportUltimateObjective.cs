using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportUltimateObjective", menuName = "Objectives/TeleportUltimateObjective")]
public class TeleportUltimateObjective : ObjectiveBase
{
    public float teleportUses = 20;

    public override void Initialize()
    {
        objectiveName = "Teleport";
        isObjectiveCompleted = false;
        isObjectiveFailed = false;
        teleportUses = 20;
        rewardPoints = 0;
        EventManager.OnTeleport += OnTeleport;
        objectiveDescription = $"Use your Teleport ability {teleportUses} times to unlock a special reward!";
    }

    public override void UpdateObjective()
    {
        if (teleportUses <= 0)
        {
            teleportUses = 0;
            CompleteObjective();
        }
        objectiveDescription = $"Use your Teleport ability {teleportUses} times to unlock a special reward!";
        ObjectiveManager.Instance.UpdateObjectivesUI();
    }
    public override void FailObjective()
    {
        // Doesn't need implementation
    }
    public override void CompleteObjective()
    {
        EventManager.OnTeleport -= OnTeleport;
        // Notify ObjectiveManager of completion
        PlayerManager.GetInstance().AbilityHolder().abilities.Find(ability => ability is AbilityTeleport).isUltimateUnlocked = true;
        UIManager.Instance.teleportUltIconFill.gameObject.SetActive(true);
        UIManager.Instance.MidScreenWarningText("Unlocked Teleport Ultimate", 3f);
        EventManager.ObjectiveCompletedEvent();
        ObjectiveManager.Instance.RemoveObjective("Teleport");
        UIManager.Instance.RemoveObjectiveFromUI("Teleport");
        // Force UI to update
        ObjectiveManager.Instance.UpdateObjectivesUI();
        base.CompleteObjective();
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
        isObjectiveCompleted = true;

    }

    void OnTeleport()
    {
        teleportUses--;
    }
}
