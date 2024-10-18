using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shield")]
public class AbilityShield : Ability
{
    public float shieldSize = 4f;
    public float shieldDamage = 20f;
    public float ultimateDamageMultiplier = 3f;
    public float shieldSpawnInterval = 0.2f; // How quickly new shields are spawned during ultimate
    public float shieldTravelSpeed = 5f; // Speed at which shields fly outward

    public override void AbilityLogic(GameObject owner, Transform target, bool isUltimate = true)
    {
        if (isUltimate)
        {
            UIManager.Instance.UltimateActivateCrackAndShatter();
            GameManager.Instance.StartCoroutine(UltimateShieldLogic(owner));
        }
        else
        {
            GameObject shield = ObjectPooler.Instance.SpawnFromPool("PlayerShield", owner.transform.position, Quaternion.identity);
            shield.SetActive(true);
            shield.transform.SetParent(owner.transform);
            shield.transform.localScale = new Vector3(shieldSize, shieldSize, shieldSize);
            GameManager.Instance.StartCoroutine(DeactivateShield(shield, duration));

            // Pass the damage value to the shield
            PlayerShieldPrefab shieldScript = shield.GetComponent<PlayerShieldPrefab>();
            shieldScript.Dps = shieldDamage;
        }
    }

    IEnumerator UltimateShieldLogic(GameObject owner)
    {
        yield return new WaitForSeconds(1f);
        GameObject firstShield = ObjectPooler.Instance.SpawnFromPool("PlayerShield", owner.transform.position, Quaternion.identity);
        firstShield.SetActive(true);
        firstShield.transform.localScale = new Vector3(shieldSize * 3, shieldSize * 3, shieldSize * 3);
        firstShield.transform.SetParent(owner.transform);
        firstShield.GetComponent<PlayerShieldPrefab>().Dps = shieldDamage * ultimateDamageMultiplier;

        float elapsedTime = 0f;

        while (elapsedTime < ultimateDuration)
        {
            // Spawn a new shield at the owner's position with a size of zero
            GameObject shield = ObjectPooler.Instance.SpawnFromPool("PlayerShield", owner.transform.position, Quaternion.identity);
            shield.SetActive(true);
            shield.transform.localScale = Vector3.zero;
            PlayerShieldPrefab shieldScript = shield.GetComponent<PlayerShieldPrefab>();
            shieldScript.Dps = shieldDamage * ultimateDamageMultiplier;

            // Start expanding the shield and moving it in a random direction
            GameManager.Instance.StartCoroutine(ExpandAndMoveShield(shield, owner.transform.position));

            // Wait for the shield spawn interval before spawning the next shield
            yield return new WaitForSeconds(shieldSpawnInterval);

            elapsedTime += shieldSpawnInterval;
        }
    }

    IEnumerator ExpandAndMoveShield(GameObject shield, Vector3 spawnPosition)
    {
        float expansionDuration = 1f; // Time for the shield to fully expand
        float movementTime = 0f;
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0).normalized;

        // Expand the shield
        while (movementTime < expansionDuration)
        {
            movementTime += Time.deltaTime;
            float t = movementTime / expansionDuration;

            // Expand the shield size smoothly
            shield.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(shieldSize * 2, shieldSize * 2, shieldSize * 2), t);

            // Continuously move the shield in the random direction
            shield.transform.position += randomDirection * shieldTravelSpeed * Time.deltaTime;

            yield return null;
        }

        // After expansion, keep the shield moving for the remaining duration
        float remainingTime = ultimateDuration - movementTime;
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // Continue moving the shield in the random direction
            shield.transform.position += randomDirection * shieldTravelSpeed * Time.deltaTime;

            yield return null;
        }

        // Deactivate the shield when its duration ends
        shield.SetActive(false);
    }

    IEnumerator DeactivateShield(GameObject shield, float duration)
    {
        yield return new WaitForSeconds(duration);
        shield.SetActive(false);
    }

    public override void ResetStats()
    {
        currentCooldown = 30f;
        shieldSize = 4f;
        shieldDamage = 20f;
        duration = 5f;
        cooldown = 30f;
        ultimateDuration = 6f;
        currentUltimateCooldown = 30f;
        shieldSpawnInterval = 0.05f;
        shieldTravelSpeed = 50f;
        ultimateDamageMultiplier = 3f;
        isUnlocked = false;
        isUltimateUnlocked = false;
    }
}
