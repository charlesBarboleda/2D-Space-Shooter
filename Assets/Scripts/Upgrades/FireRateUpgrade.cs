using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateUpgrade : Upgrade
{
    public float fireRateUpgradeAmount = 0.01f;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetInstance().Currency() >= upgradeCost)
        {
            PlayerManager.GetInstance().SetCurrency(-upgradeCost);
            PlayerManager.GetInstance().Weapon().fireRate -= fireRateUpgradeAmount;
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
