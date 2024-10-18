using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Teleport")]
public class AbilityTeleport : Ability
{
    [SerializeField] GameObject _teleportEffect;
    public float teleportDistance;
    public float teleportSize;
    public float teleportDamage;

    public float ultimateDamageMultiplier = 10f;
    public override void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false)
    {

        if (isUltimate)
        {
            UIManager.Instance.UltimateActivateCrackAndShatter();
            GameManager.Instance.StartCoroutine(ActivateUltimateTeleport(owner));
        }
        else
        {
            EventManager.TeleportEvent();
            GameManager.Instance.StartCoroutine(NormalTeleport(owner));
        }

    }
    IEnumerator NormalTeleport(GameObject owner)
    {
        // Initial teleport effect
        GameObject tpEffect = ObjectPooler.Instance.SpawnFromPool("PlayerTeleport", owner.transform.position, Quaternion.identity);
        tpEffect.transform.localScale = new Vector3(teleportSize, teleportSize, teleportSize);
        PlayerTeleportPrefab teleportScript = tpEffect.GetComponent<PlayerTeleportPrefab>();
        teleportScript.SetDamage(teleportDamage);

        // Teleport the player based on movement input
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        owner.transform.position += new Vector3(inputX, inputY, 0f) * teleportDistance;

        // Post teleport effect
        GameObject postTPEffect = ObjectPooler.Instance.SpawnFromPool("PlayerTeleport", owner.transform.position, Quaternion.identity);
        postTPEffect.transform.localScale = new Vector3(teleportSize, teleportSize, teleportSize);
        PlayerTeleportPrefab teleportScript2 = postTPEffect.GetComponent<PlayerTeleportPrefab>();
        teleportScript2.SetDamage(teleportDamage);

        yield return new WaitForSeconds(duration);
        tpEffect.SetActive(false);
        postTPEffect.SetActive(false);
    }
    IEnumerator ActivateUltimateTeleport(GameObject owner)
    {
        yield return new WaitForSeconds(1f);

        GameObject tpEffect = ObjectPooler.Instance.SpawnFromPool("PlayerTeleport", owner.transform.position, Quaternion.identity);
        tpEffect.transform.localScale = new Vector3(60, 60, 60);
        tpEffect.transform.SetParent(owner.transform);
        PlayerTeleportPrefab teleportScript = tpEffect.GetComponent<PlayerTeleportPrefab>();
        teleportScript.SetDamage(teleportDamage * ultimateDamageMultiplier);
        yield return new WaitForSeconds(ultimateDuration);
        tpEffect.SetActive(false);
    }

    public override void ResetStats()
    {
        currentCooldown = 30f;
        cooldown = 30f;
        teleportDistance = 5;
        teleportDamage = 10;
        teleportSize = 1;
        duration = 1f;
        ultimateCooldown = 180f;
        ultimateDuration = 6f;
        ultimateDamageMultiplier = 10f;
        isUnlocked = false;
        isUltimateUnlocked = false;
    }

}

