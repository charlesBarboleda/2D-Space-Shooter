using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateUpgrade : Upgrade
{
    public float fireRateUpgradeAmount = 0.001f;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetPlayer().currency >= upgradeCost)
        {
            PlayerManager.GetPlayer().currency -= upgradeCost;
            PlayerManager.GetPlayer().weapon.fireRate -= fireRateUpgradeAmount;
            upgradeCost += 100;

        }
    }

    public override void Initialize(string name, string description, int cost)
    {
        this.upgradeName = name;
        this.upgradeDescription = description;
        this.upgradeCost = cost;
    }

}
