using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public bool isUnlocked;
    public string abilityName;
    public Sprite icon;
    public float cooldown = 1f;
    public float currentCooldown;

    public abstract void ResetStats();
    public abstract void AbilityLogic(GameObject owner, Transform target);

    public void TriggerAbility(GameObject owner, Transform target)
    {
        if (!isUnlocked)
        {
            Debug.Log("Ability is locked");
            return;
        }
        if (currentCooldown >= cooldown)
        {
            AbilityLogic(owner, target);
            currentCooldown = 0f;

        }
        else
        {
            Debug.Log("Ability is on cooldown");
        }
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (currentCooldown < cooldown)
        {
            currentCooldown += deltaTime;
            currentCooldown = Mathf.Clamp(currentCooldown, 0, cooldown);
        }
    }
}
