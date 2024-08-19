using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : Upgrade
{
    public int healthUpgradeAmount = 10;
    public override void ApplyUpgrade()
    {
        if (GameManager.Instance.GetPlayer().currency >= upgradeCost)
        {
            GameManager.Instance.GetPlayer().currency -= upgradeCost;
            GameManager.Instance.GetPlayer().playerHealth += healthUpgradeAmount;
            Debug.Log("Health Upgraded");
            upgradeCost += 50;
            healthUpgradeAmount += 10;
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
