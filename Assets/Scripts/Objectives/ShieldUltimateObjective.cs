using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldUltimateObjective", menuName = "Objectives/ShieldUltimateObjective")]
public class ShieldUltimateObjective : ObjectiveBase
{
    public float damageAbsorbed = 20000;

    public override void Initialize()
    {
        objectiveName = "Shield";
        damageAbsorbed = 20000;
        rewardPoints = 0;
        EventManager.OnShieldAbsorb += OnShieldAbsorb;
        objectiveDescription = $"Absorb {damageAbsorbed} damage with your Shield ability to unlock a special reward!";
    }

    public override void UpdateObjective()
    {
        if (damageAbsorbed <= 0)
        {
            damageAbsorbed = 0;
            CompleteObjective();
        }
        objectiveDescription = $"Absorb {damageAbsorbed} damage with your Shield ability to unlock a special reward!";
        ObjectiveManager.Instance.UpdateObjectivesUI();
    }
    public override void FailObjective()
    {
        // Doesn't need implementation
    }
    public override void CompleteObjective()
    {
        EventManager.OnShieldAbsorb -= OnShieldAbsorb;
        // Notify ObjectiveManager of completion
        PlayerManager.GetInstance().AbilityHolder().abilities.Find(ability => ability is AbilityShield).isUltimateUnlocked = true;
        UIManager.Instance.shieldUltIconFill.gameObject.SetActive(true);
        UIManager.Instance.MidScreenWarningText("Unlocked Shield Ultimate", 3f);
        EventManager.ObjectiveCompletedEvent();
        ObjectiveManager.Instance.RemoveObjective("Shield");
        UIManager.Instance.RemoveObjectiveFromUI("Shield");
        // Force UI to update
        ObjectiveManager.Instance.UpdateObjectivesUI();
        base.CompleteObjective();
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
        isObjectiveCompleted = true;

    }

    void OnShieldAbsorb(float damage)
    {
        damageAbsorbed -= damage;
    }
}
