using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageUpgrade : Upgrade
{
    public int bulletDamageUpgradeAmount = 50;
    public override void ApplyUpgrade()
    {
        if (GameManager.Instance.GetPlayer().currency >= upgradeCost)
        {
            GameManager.Instance.GetPlayer().currency -= upgradeCost;
            GameManager.Instance.GetPlayer().weapon.bulletDamage += bulletDamageUpgradeAmount;
            upgradeCost += 50;
            bulletDamageUpgradeAmount += 10;

        }
    }

    public override void Initialize(string name, string description, int cost)
    {
        this.upgradeName = name;
        this.upgradeDescription = description;
        this.upgradeCost = cost;
    }

}
