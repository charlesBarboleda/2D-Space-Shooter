using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeedUpgrade : Upgrade
{
    public float bulletSpeedUpgradeAmount = 0.2f;
    public override void ApplyUpgrade()
    {
        if (GameManager.Instance.GetPlayer().currency >= upgradeCost)
        {
            GameManager.Instance.GetPlayer().currency -= upgradeCost;
            GameManager.Instance.GetPlayer().weapon.bulletSpeed += bulletSpeedUpgradeAmount;
            upgradeCost += 50;
            bulletSpeedUpgradeAmount += 0.1f;

        }
    }

    public override void Initialize(string name, string description, int cost)
    {
        this.upgradeName = name;
        this.upgradeDescription = description;
        this.upgradeCost = cost;
    }

}
