using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpeedUpgrade : Upgrade
{
    public float shipSpeedUpgradeAmount = 0.1f;
    public override void ApplyUpgrade()
    {
        if (GameManager.Instance.GetPlayer().currency >= upgradeCost)
        {
            GameManager.Instance.GetPlayer().currency -= upgradeCost;
            GameManager.Instance.GetPlayer().playerController.moveSpeed += shipSpeedUpgradeAmount;
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
