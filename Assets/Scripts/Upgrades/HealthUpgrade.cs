using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : Upgrade
{
    public int healthUpgradeAmount = 5;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.Player().currency >= upgradeCost)
        {
            PlayerManager.Player().currency -= upgradeCost;
            PlayerManager.Player().playerHealth += healthUpgradeAmount;
            PlayerManager.Player().maxHealth += healthUpgradeAmount;

            upgradeCost += 50;
        }
        else
        {
            Debug.Log("Not enough currency");
        }



    }

    public override void Initialize(string name, string description, int cost)
    {
        upgradeName = name;
        upgradeDescription = description;
        upgradeCost = cost;
    }



}
