using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageUpgrade : Upgrade
{
    public int bulletDamageUpgradeAmount = 5;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.Player().currency >= upgradeCost)
        {
            PlayerManager.Player().currency -= upgradeCost;
            PlayerManager.Player().weapon.bulletDamage += bulletDamageUpgradeAmount;
            upgradeCost += 50;

        }
    }

    public override void Initialize(string name, string description, int cost)
    {
        this.upgradeName = name;
        this.upgradeDescription = description;
        this.upgradeCost = cost;
    }

}
