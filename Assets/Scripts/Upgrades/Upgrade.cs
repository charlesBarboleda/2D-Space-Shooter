using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade
{
    public string upgradeName;
    public string upgradeDescription;
    public int upgradeCost;

    public abstract void Initialize(string name, string description, int cost);
    public abstract void ApplyUpgrade();
}
