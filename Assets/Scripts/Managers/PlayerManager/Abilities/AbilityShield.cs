using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shield")]
public class AbilityShield : Ability
{
    [SerializeField] private float _duration;
    [SerializeField] private GameObject _shieldprefab;
    [SerializeField] private float _shieldSize;
    [SerializeField] private float _shieldDamage;


    public override void UseAbility(GameObject owner, Transform target)
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


}
