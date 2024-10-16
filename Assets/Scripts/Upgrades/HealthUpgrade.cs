using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : Upgrade
{
    public int healthUpgradeAmount = 5;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetInstance().Currency() >= upgradeCost)
        {
            PlayerManager.GetInstance().SetCurrency(-upgradeCost);
            PlayerManager.GetInstance().SetCurrentHealth(healthUpgradeAmount);
            PlayerManager.GetInstance().SetMaxHealth(healthUpgradeAmount);

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
