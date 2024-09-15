using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shield")]
public class AbilityShield : Ability
{
    public float _duration = 5f;
    public float _shieldSize = 3f;
    public float _shieldDamage = 10f;
    [SerializeField] GameObject _shieldprefab;


    public override void AbilityLogic(GameObject owner, Transform target)
    {

        GameObject shield = Instantiate(_shieldprefab, target.position, Quaternion.identity);
        shield.SetActive(true);
        shield.transform.SetParent(owner.transform);
        shield.transform.localScale = new Vector3(_shieldSize, _shieldSize, _shieldSize);
        Destroy(shield, _duration);

        //Pass the damage value to the shield
        PlayerShieldPrefab shieldScript = shield.GetComponent<PlayerShieldPrefab>();
        shieldScript.Dps = _shieldDamage;

    }

    public override void ResetStats()
    {
        currentCooldown = 30f;
        _shieldSize = 3f;
        _shieldDamage = 10f;
        _duration = 5f;
        cooldown = 30f;
        isUnlocked = false;
    }


}
