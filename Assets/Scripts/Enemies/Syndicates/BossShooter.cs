using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : ShooterEnemy
{

    public override void IncreaseStatsPerLevel()
    {

        Health.CurrentHealth += LevelManager.Instance.CurrentLevelIndex * 10f;
        Health.MaxHealth += LevelManager.Instance.CurrentLevelIndex * 10f;
        Health.CurrencyDrop += LevelManager.Instance.CurrentLevelIndex * 1f;
        Kinematics.MaxSpeed += LevelManager.Instance.CurrentLevelIndex * 0.1f;
    }

}
