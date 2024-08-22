using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{

    public List<Ability> abilities;
    public Transform target;


    void Start()
    {
        ResetAbilityStats();
    }
    // Update is called once per frame
    void Update()
    {
        foreach (Ability ability in abilities)
        {
            ability.UpdateCooldown(Time.deltaTime);
        }


    }

    void ResetAbilityStats()
    {
        foreach (Ability ability in abilities)
        {
            if (ability is AbilityTeleport)
            {
                // Reset the teleport stats using type casting
                ((AbilityTeleport)ability).ResetTeleportStats();
            }
            else if (ability is AbilityShield)
            {
                // Reset the shield stats using type casting
                ((AbilityShield)ability).ResetShieldStats();
            }
            else if (ability is AbilityTurrets)
            {
                // Reset the turret stats using type casting
                ((AbilityTurrets)ability).ResetTurretStats();
            }
            else if (ability is AbilityLaser)
            {
                // Reset the laser stats using type casting
                ((AbilityLaser)ability).ResetLaserStats();
            }
        }
    }



}
