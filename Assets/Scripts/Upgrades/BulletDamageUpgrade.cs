using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageUpgrade : Upgrade
{
    public int bulletDamageUpgradeAmount = 5;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetInstance().Currency() >= upgradeCost)
        {
            PlayerManager.GetInstance().SetCurrency(PlayerManager.GetInstance().Currency() - upgradeCost);
            PlayerManager.GetInstance().Weapon().bulletDamage += bulletDamageUpgradeAmount;
            upgradeCost += 50;

        }
    }

    public override void Initialize(string name, string description, int cost)
    {
        this.upgradeName = name;
        this.upgradeDescription = description;
        this.upgradeCost = cost;
    }

}
