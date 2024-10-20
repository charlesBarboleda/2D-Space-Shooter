using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretUltimateObjective", menuName = "Objectives/TurretUltimateObjective")]
public class TurretUltimateObjective : ObjectiveBase
{
    public float damageNeeded = 30000;

    public override void Initialize()
    {
        objectiveName = "Turret";
        isObjectiveCompleted = false;
        isObjectiveFailed = false;
        damageNeeded = 30000;
        rewardPoints = 0;
        EventManager.OnBulletDamage += OnBulletDamage;
        objectiveDescription = $"Deal {damageNeeded} bullet damage to unlock a special reward!";
    }

    public override void UpdateObjective()
    {
        if (damageNeeded <= 0)
        {
            damageNeeded = 0;
            CompleteObjective();
        }
        objectiveDescription = $"Deal {damageNeeded} bullet damage to unlock a special reward!";
        ObjectiveManager.Instance.UpdateObjectivesUI();
    }
    public override void FailObjective()
    {
        // Doesn't need implementation
    }
    public override void CompleteObjective()
    {
        EventManager.OnBulletDamage -= OnBulletDamage;
        // Notify ObjectiveManager of completion
        PlayerManager.GetInstance().AbilityHolder().abilities.Find(ability => ability is AbilityTurrets).isUltimateUnlocked = true;
        UIManager.Instance.turretUltIconFill.gameObject.SetActive(true);
        UIManager.Instance.MidScreenWarningText("Unlocked Turret Ultimate", 3f);
        ObjectiveManager.Instance.RemoveObjective("Turret");
        UIManager.Instance.RemoveObjectiveFromUI("Turret");
        EventManager.ObjectiveCompletedEvent();
        // Force UI to update
        ObjectiveManager.Instance.UpdateObjectivesUI();
        base.CompleteObjective();
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
        isObjectiveCompleted = true;

    }

    void OnBulletDamage(float damage)
    {
        damageNeeded -= damage;
    }
}
