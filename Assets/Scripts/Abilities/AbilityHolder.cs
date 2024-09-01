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
            ability.ResetStats();
        }
    }



}
