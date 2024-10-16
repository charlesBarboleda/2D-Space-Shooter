using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBulletUpgrade : Upgrade
{
    public int extraBulletUpgradeAmount = 1;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetInstance().Currency() >= upgradeCost)
        {
            PlayerManager.GetInstance().SetCurrency(-upgradeCost);
            PlayerManager.GetInstance().Weapon().amountOfBullets += extraBulletUpgradeAmount;
            upgradeCost *= 2;

        }
    }

    public override void Initialize(string name, string description, int cost)
    {
        this.upgradeName = name;
        this.upgradeDescription = description;
        this.upgradeCost = cost;
    }

}
