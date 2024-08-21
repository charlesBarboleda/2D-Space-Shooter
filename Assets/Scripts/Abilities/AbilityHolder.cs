using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public int abilityIndex;
    public List<Ability> abilities;
    public Transform target;

    [SerializeField] private KeyCode key;


    void Start()
    {

        ResetTurretCount();
        ResetShieldStats();
        ResetTeleportStats();
    }
    // Update is called once per frame
    void Update()
    {
        foreach (Ability ability in abilities)
        {
            ability.UpdateCooldown(Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            abilities[abilityIndex].TriggerAbility(gameObject, target);
        }


    }
    void ResetTurretCount()
    {
        foreach (Ability ability in abilities)
        {
            if (ability is AbilityTurrets)
            {
                // Reset the number of turrets spawned using type casting
                ((AbilityTurrets)ability).ResetTurretStats();
            }
        }
    }

    void ResetShieldStats()
    {
        foreach (Ability ability in abilities)
        {
            if (ability is AbilityShield)
            {
                // Reset the shield stats using type casting
                ((AbilityShield)ability).ResetShieldStats();
            }
        }
    }

    void ResetTeleportStats()
    {
        foreach (Ability ability in abilities)
        {
            if (ability is AbilityTeleport)
            {
                // Reset the teleport stats using type casting
                ((AbilityTeleport)ability).ResetTeleportStats();
            }
        }
    }


}
