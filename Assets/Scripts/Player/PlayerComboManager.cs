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
            { 500, DealAOEDamage }
        };
    }

    void Update()
    {
        comboCount = _comboManager.comboCount;

        foreach (var comboAction in comboActions)
        {
            // Check if the combo threshold is reached and the buff hasn't been activated yet
            if (comboCount >= comboAction.Key && !buffsActivated[comboAction.Key])
            {
                UIManager.Instance.ActivateComboKey();

                if (Input.GetKeyDown(KeyCode.R))
                {
                    // Activate the buff and disable the UI once the player presses the key
                    AudioManager.Instance.PlaySound(GameManager.Instance._audioSource, _onComboAbilityExecute);
                    UIManager.Instance.DeactivateComboKey();
                    comboAction.Value.Invoke();
                    ComboAnimation();
                    _playerManager.ActivateBuffAnimations();
                    buffsActivated[comboAction.Key] = true;  // Mark this buff as activated
                    break;  // Avoid multiple activations at once
                }
            }
        }
    }

    void IncreasePlayerSpeed()
    {
        var emission = _playerManager.arrowEmission.emission;
        emission.rateOverTime = 5;
        _playerManager.SetMoveSpeed(_playerManager.MoveSpeed() * 1.5f);
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
        _playerManager.SetMaxHealth(_playerManager.MaxHealth() * 1.5f);
        _playerManager.SetCurrentHealth(_playerManager.CurrentHealth() * 1.5f);
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
        if (buffsActivated[25] == true)
        {
            _playerManager.SetMoveSpeed(_playerManager.MoveSpeed() / 1.5f);
            buffsActivated[25] = false;
        }

        if (buffsActivated[50] == true)
        {
            _playerManager.Weapon().bulletSpeed /= 1.5f;
            buffsActivated[50] = false;
        }

        if (buffsActivated[75] == true)
        {
            _playerManager.PickUpBehaviour().PickUpRadius /= 2f;
            buffsActivated[75] = false;
        }
        if (buffsActivated[100] == true)
        {
            _playerManager.SetMaxHealth(_playerManager.MaxHealth() / 1.5f);
            _playerManager.SetCurrentHealth(_playerManager.CurrentHealth() / 1.5f);
            buffsActivated[100] = false;
        }
        if (buffsActivated[150] == true)
        {
            _playerManager.Weapon().bulletDamage /= 1.5f;
            buffsActivated[150] = false;
        }
        if (buffsActivated[250] == true)
        {
            _playerManager.Weapon().amountOfBullets -= 2;
            buffsActivated[250] = false;
        }


    }
}











