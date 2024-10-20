using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboManager : MonoBehaviour
{
    float playerCurrentPostHealth;
    float playerPostHealth;
    ComboManager _comboManager;
    int comboCount;
    PlayerManager _playerManager;
    GameObject damageEffect;
    AudioSource _audioSource;
    [SerializeField] AudioClip _onComboAbilityExecute;

    // Track the last multiple of 500 where the AOE ability was used
    int _lastComboThresholdUsed = 0;

    // Track whether specific buffs have been activated
    Dictionary<int, bool> buffsActivated;

    void Start()
    {
        _comboManager = ComboManager.Instance;
        _playerManager = PlayerManager.Instance;
        _audioSource = GetComponent<AudioSource>();

        // Initialize dictionary to track activated buffs
        buffsActivated = new Dictionary<int, bool>
        {
            { 25, false },
            { 50, false },
            { 75, false },
            { 100, false },
            { 150, false },
            { 250, false }
        };
    }

    void Update()
    {
        int comboCount = _comboManager.comboCount;

        // Handle smaller combo buffs (25, 50, 100, etc.)
        foreach (var comboAction in buffsActivated.Keys)
        {
            if (comboCount >= comboAction && !buffsActivated[comboAction])
            {
                ExecuteComboAction(comboAction);
                buffsActivated[comboAction] = true;
            }
        }

        // Handle the 500 combo ability (and multiples of 500) after passing a threshold
        if (comboCount >= 500 && CanUseAOECombo(comboCount))
        {
            UIManager.Instance.ActivateComboKey();

            if (Input.GetKeyDown(KeyCode.R))
            {
                ExecuteAOEComboAction();
                _lastComboThresholdUsed = comboCount / 500 * 500; // Update the last combo threshold
            }
        }
    }

    bool CanUseAOECombo(int comboCount)
    {
        // Check if the current threshold is a multiple of 500 and has not been used yet
        int currentComboThreshold = comboCount / 500 * 500;
        return currentComboThreshold > _lastComboThresholdUsed;
    }

    void ExecuteComboAction(int comboThreshold)
    {
        switch (comboThreshold)
        {
            case 25:
                IncreasePlayerSpeed();
                break;
            case 50:
                IncreasePlayerBulletSpeed();
                break;
            case 75:
                IncreasePlayerPickUpRadius();
                break;
            case 100:
                IncreasePlayerHealth();
                break;
            case 150:
                IncreasePlayerDamage();
                break;
            case 250:
                IncreaseBulletAmount();
                break;
        }
        _playerManager.ActivateBuffAnimations();
    }

    void ExecuteAOEComboAction()
    {
        UIManager.Instance.DeactivateComboKey();
        DealAOEDamage(); // Trigger the AOE ability
        ComboAnimation();
        AudioManager.Instance.PlaySound(GameManager.Instance._audioSource, _onComboAbilityExecute);
    }

    void IncreasePlayerSpeed()
    {
        var emission = _playerManager.arrowEmission.emission;
        emission.rateOverTime = 5;
        _playerManager.Movement().moveSpeed *= 1.5f;
    }

    void IncreasePlayerBulletSpeed()
    {
        var emission = _playerManager.arrowEmission.emission;
        emission.rateOverTime = 10;
        _playerManager.Weapon().bulletSpeed *= 1.5f;
    }

    void IncreasePlayerDamage()
    {
        var emission = _playerManager.arrowEmission.emission;
        emission.rateOverTime = 20;
        _playerManager.Weapon().bulletDamage *= 1.5f;
    }

    void IncreasePlayerPickUpRadius()
    {
        var emission = _playerManager.arrowEmission.emission;
        emission.rateOverTime = 15;
        _playerManager.PickUpBehaviour().PickUpRadius *= 2f;
    }

    void IncreaseBulletAmount()
    {
        var emission = _playerManager.arrowEmission.emission;
        emission.rateOverTime = 25;
        _playerManager.Weapon().amountOfBullets += 2;
    }

    void IncreasePlayerHealth()
    {
        var emission = _playerManager.arrowEmission.emission;
        emission.rateOverTime = 20;
        playerPostHealth = (_playerManager.MaxHealth() * 1.25f) - _playerManager.MaxHealth();
        playerCurrentPostHealth = (_playerManager.CurrentHealth() * 1.25f) - _playerManager.CurrentHealth();
        _playerManager.SetMaxHealth(playerPostHealth);
        _playerManager.SetCurrentHealth(playerCurrentPostHealth);
    }

    void DealAOEDamage()
    {
        damageEffect = ObjectPooler.Instance.SpawnFromPool("ComboAOE", transform.position, Quaternion.identity);
        damageEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);
        damageEffect.transform.SetParent(_playerManager.transform);
        damageEffect.transform.localScale = Vector3.zero;
        StartCoroutine(IncreaseAOEScale(damageEffect, 70f, 1.5f));
    }

    void ComboAnimation()
    {
        GameObject animEffect = ObjectPooler.Instance.SpawnFromPool("ComboPowerUp", transform.position, Quaternion.identity);
        animEffect.transform.localScale = Vector3.zero;
        StartCoroutine(IncreaseAOEScale(animEffect, 50f, 2f));
        StartCoroutine(FadeOut(animEffect, 2f));
    }

    IEnumerator IncreaseAOEScale(GameObject effect, float size, float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration; // Normalized value between 0 and 1

            // Scale the AOE effect
            effect.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(size, size, size), progress);

            yield return null;
        }

        effect.SetActive(false);
    }

    IEnumerator FadeOut(GameObject target, float duration)
    {
        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("GameObject does not have a SpriteRenderer component.");
            yield break;
        }

        Color originalColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration); // Interpolate from 1 to 0
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha); // Apply the new alpha

            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // Ensure it's fully transparent at the end
        target.SetActive(false); // Optionally deactivate the GameObject after fading
    }

    // Method to remove all buffs and reset combo thresholds
    public void RemoveAllBuffs()
    {
        _playerManager.DeactivateBuffAnimations();

        // Reset all buffs activated for each threshold
        foreach (var key in buffsActivated.Keys)
        {
            if (buffsActivated[key])
            {
                RemoveBuff(key);
                buffsActivated[key] = false; // Mark the buff as deactivated
            }
        }

        // Reset the last combo threshold used for AOE
        _lastComboThresholdUsed = 0;
    }

    // Method to remove individual buffs based on combo threshold
    void RemoveBuff(int comboThreshold)
    {
        switch (comboThreshold)
        {
            case 25:
                _playerManager.Movement().moveSpeed /= 1.5f;
                break;
            case 50:
                _playerManager.Weapon().bulletSpeed /= 1.5f;
                break;
            case 75:
                _playerManager.PickUpBehaviour().PickUpRadius /= 2f;
                break;
            case 100:
                _playerManager.SetMaxHealth(-playerPostHealth);
                _playerManager.SetCurrentHealth(-playerCurrentPostHealth);
                break;
            case 150:
                _playerManager.Weapon().bulletDamage /= 1.5f;
                break;
            case 250:
                _playerManager.Weapon().amountOfBullets -= 2;
                break;
        }
    }
}














