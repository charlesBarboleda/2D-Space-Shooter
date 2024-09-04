using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpeedUpgrade : Upgrade
{
    float _shipSpeedUpgradeAmount = 0.5f;
    public override void ApplyUpgrade()
    {
        if (PlayerManager.GetPlayer().currency >= upgradeCost)
        {
            PlayerManager.GetPlayer().currency -= upgradeCost;
            PlayerManager.GetPlayer().playerMovementBehaviour.SetMoveSpeed(
                PlayerManager.GetPlayer().playerMovementBehaviour.GetMoveSpeed() +
                _shipSpeedUpgradeAmount
                );
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
