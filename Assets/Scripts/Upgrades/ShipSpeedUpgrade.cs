using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpeedUpgrade : Upgrade
{
    float _shipSpeedUpgradeAmount = 0.5f;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetInstance().Currency() >= upgradeCost)
        {
            PlayerManager.GetInstance().SetCurrency(PlayerManager.GetInstance().Currency() - upgradeCost);
            PlayerManager.GetInstance().SetMoveSpeed(PlayerManager.GetInstance().MoveSpeed() + _shipSpeedUpgradeAmount);
            upgradeCost += 100;

        }
    }

    public override void Initialize(string name, string description, int cost)
    {
        this.upgradeName = name;
        this.upgradeDescription = description;
        this.upgradeCost = cost;
    }

    public float GetShipSpeedUpgradeAmount() => _shipSpeedUpgradeAmount;


}
