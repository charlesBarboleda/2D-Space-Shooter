using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBulletUpgrade : Upgrade
{
    public int extraBulletUpgradeAmount = 1;
    public override void ApplyUpgrade()
    {
        if (GameManager.Instance.GetPlayer().currency >= upgradeCost)
        {
            GameManager.Instance.GetPlayer().currency -= upgradeCost;
            GameManager.Instance.GetPlayer().weapon.amountOfBullets += extraBulletUpgradeAmount;
            upgradeCost += 1000;

        }
    }

    public override void Initialize(string name, string description, int cost)
    {
        this.upgradeName = name;
        this.upgradeDescription = description;
        this.upgradeCost = cost;
    }

}
