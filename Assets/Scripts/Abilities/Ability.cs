using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public bool isUnlocked;
    public string abilityName;
    public Sprite icon;
    public float duration;
    public float cooldown = 10f;
    public float currentCooldown;
    public float ultimateCooldown = 30f; // Ultimate cooldown time
    public float currentUltimateCooldown;
    public bool isUltimateUnlocked;
    public bool isUltimateReady = false; // Flag to check if ultimate can be triggered
    public float ultimateTriggerWindow = 3f; // Time window to press the same key for ultimate
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _audioClip;
    public abstract void ResetStats();
    public abstract void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false); // Add flag for ultimate

    public void TriggerAbility(GameObject owner, Transform target)
    {
        if (!isUnlocked)
        {
            return;
        }

        if (isUltimateReady && currentUltimateCooldown >= ultimateCooldown && isUltimateUnlocked)
        {
            // Trigger ultimate ability
            AbilityLogic(owner, target, true);
            currentUltimateCooldown = 0f;
            isUltimateReady = false; // Reset ultimate trigger
        }
        else if (currentCooldown >= cooldown)
        {
            // Trigger normal ability
            AbilityLogic(owner, target, false);
            currentCooldown = 0f;
            GameManager.Instance.StartCoroutine(TriggerUltimateWindow()); // Start window for ultimate trigger
        }
        else
        {
            Debug.Log("Ability is on cooldown");
        }
    }

    private IEnumerator TriggerUltimateWindow()
    {
        isUltimateReady = true; // Ultimate can now be triggered
        yield return new WaitForSeconds(ultimateTriggerWindow); // Wait for the window duration
        isUltimateReady = false; // Reset after the window expires
    }

    public void UpdateCooldown(float deltaTime)
    {
        // Update normal cooldown
        if (currentCooldown < cooldown)
        {
            currentCooldown += deltaTime;
            currentCooldown = Mathf.Clamp(currentCooldown, 0, cooldown);
        }

        // Update ultimate cooldown
        if (currentUltimateCooldown < ultimateCooldown)
        {
            currentUltimateCooldown += deltaTime;
            currentUltimateCooldown = Mathf.Clamp(currentUltimateCooldown, 0, ultimateCooldown);
        }
    }
}

