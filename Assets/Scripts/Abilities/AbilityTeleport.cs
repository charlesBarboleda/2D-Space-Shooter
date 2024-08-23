using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Teleport")]
public class AbilityTeleport : Ability
{
    [SerializeField] GameObject _teleportEffect;
    public float _teleportDistance;
    public float _teleportSize;
    public float _teleportDamage;
    public float _teleportDuration;
    public override void AbilityLogic(GameObject owner, Transform target)
    {
        // Initial teleport effect
        GameObject tpEffect = Instantiate(_teleportEffect, owner.transform.position, Quaternion.identity);
        tpEffect.transform.localScale = new Vector3(_teleportSize, _teleportSize, _teleportSize);
        PlayerTeleportPrefab teleportScript = tpEffect.GetComponent<PlayerTeleportPrefab>();
        teleportScript.SetDamage(_teleportDamage);
        Destroy(tpEffect, _teleportDuration);

        // Teleport the player based on movement input
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        owner.transform.position += new Vector3(inputX, inputY, 0f) * _teleportDistance;

        // Post teleport effect
        GameObject postTPEffect = Instantiate(_teleportEffect, owner.transform.position, Quaternion.identity);
        postTPEffect.transform.localScale = new Vector3(_teleportSize, _teleportSize, _teleportSize);
        PlayerTeleportPrefab teleportScript2 = postTPEffect.GetComponent<PlayerTeleportPrefab>();
        teleportScript2.SetDamage(_teleportDamage);
        Destroy(postTPEffect, _teleportDuration);
    }

    public void ResetTeleportStats()
    {
        currentCooldown = 30f;
        cooldown = 30f;
        _teleportDistance = 2;
        _teleportDamage = 10;
        _teleportSize = 1;
        _teleportDuration = 1f;
        isUnlocked = false;
    }

}

