using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(AbilityHolder))]
public class PlayerAbilitiesBehaviour : MonoBehaviour
{
    AbilityHolder _abilityHolder;
    void Start()
    {
        _abilityHolder = GetComponent<AbilityHolder>();

    }

    // Update is called once per frame
    void Update()
    {
        Abilities();
    }

    void Abilities()
    {
        // Use the Ability Laser
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _abilityHolder.abilities[0].TriggerAbility(gameObject, _abilityHolder.target);

        }

        // Use the Ability Shield
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _abilityHolder.abilities[2].TriggerAbility(gameObject, transform);

        }

        // Use the Ability Teleport
        if (Input.GetKeyDown(KeyCode.E))
        {
            _abilityHolder.abilities[3].TriggerAbility(gameObject, _abilityHolder.target);

        }


    }
}
