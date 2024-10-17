using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboManager : MonoBehaviour
{
    ComboManager _comboManager;
    int comboCount;
    PlayerManager _playerManager;
    GameObject damageEffect;
    AudioSource _audioSource;
    [SerializeField] AudioClip _onComboAbilityExecute;

    // Track whether a buff has been activated for each threshold
    Dictionary<int, bool> buffsActivated;
    Dictionary<int, Action> comboActions;

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
            { 250, false },
            { 500, false }
        };

        // Define combo actions
        comboActions = new Dictionary<int, Action>
        {
            { 25, IncreasePlayerSpeed },
            { 50, IncreasePlayerBulletSpeed },
            { 75, IncreasePlayerPickUpRadius },
            { 100, IncreasePlayerHealth },
            { 150, IncreasePlayerDamage },
            { 250, IncreaseBulletAmount },
            { 500, DealAOEDamage } // Can handle any multiple of 500
        };
    }

    void Update()
    {
        int comboCount = _comboManager.comboCount;

        foreach (var comboAction in comboActions)
        {
            if (comboCount >= comboAction.Key && ShouldActivateCombo(comboAction.Key))
            {
                UIManager.Instance.ActivateComboKey();

                if (Input.GetKeyDown(KeyCode.R))
                {
                    ExecuteComboAction(comboAction.Key);
                    break; // Prevent multiple activations at once
                }
            }
        }
    }

    bool ShouldActivateCombo(int comboThreshold)
    {
        // For multiples of 500, check divisibility
        if (comboThreshold == 500)
        {
            return (_comboManager.comboCount % 500 == 0) && !buffsActivated[comboThreshold];
        }

        return !buffsActivated[comboThreshold];
    }

    void ExecuteComboAction(int comboThreshold)
    {
        UIManager.Instance.DeactivateComboKey();
        comboActions[comboThreshold].Invoke();
        ComboAnimation();
        _playerManager.ActivateBuffAnimations();
        buffsActivated[comboThreshold] = true;
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
        _playerManager.SetMaxHealth(_playerManager.MaxHealth() * 1.25f);
        _playerManager.SetCurrentHealth(_playerManager.CurrentHealth() * 1.25f);
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

    public void RemoveAllBuffs()
    {
        _playerManager.DeactivateBuffAnimations();
        foreach (var key in buffsActivated.Keys)
        {
            if (buffsActivated[key])
            {
                RemoveBuff(key);
                buffsActivated[key] = false;
            }
        }
    }

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
                _playerManager.SetMaxHealth(_playerManager.MaxHealth() / 1.25f);
                _playerManager.SetCurrentHealth(_playerManager.CurrentHealth() / 1.25f);
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











