using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpUpgrade : Upgrade
{
    public float pickUpUpgradeAmount = 0.5f;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetPlayer().currency >= upgradeCost)
        {
            PlayerManager.GetPlayer().currency -= upgradeCost;
            PlayerManager.GetPlayer().pickUpRadius += pickUpUpgradeAmount;
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
