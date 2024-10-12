using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebuffs : MonoBehaviour
{
    public Dictionary<string, bool> debuffsList = new Dictionary<string, bool>()
    {
        { "Corrode", false },
        { "Frozen", false },
    };

    public void ApplyDebuff(string debuffName)
    {
        debuffsList[debuffName] = true;
    }

    public void RemoveDebuff(string debuffName)
    {
        debuffsList[debuffName] = false;
    }

}
