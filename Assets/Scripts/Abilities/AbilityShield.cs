using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shield")]
public class AbilityShield : Ability
{
    public float _duration;
    public float _shieldSize;
    public float _shieldDamage;
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
        shieldScript.SetDamage(_shieldDamage);

    }

    public void ResetShieldStats()
    {
        currentCooldown = 30f;
        _shieldSize = 1.5f;
        _shieldDamage = 10;
        _duration = 3f;
        cooldown = 30f;
        isUnlocked = false;
    }


}
