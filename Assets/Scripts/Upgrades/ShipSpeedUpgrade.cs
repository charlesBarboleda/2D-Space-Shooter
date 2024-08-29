using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpeedUpgrade : Upgrade
{
    public float shipSpeedUpgradeAmount = 0.1f;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetPlayer().currency >= upgradeCost)
        {
            PlayerManager.GetPlayer().currency -= upgradeCost;
            PlayerManager.GetPlayer().playerController.moveSpeed += shipSpeedUpgradeAmount;
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
