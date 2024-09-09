using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : ShooterEnemy
{
    public override void Die()
    {
        base.Die();
        ObjectivesManager.Instance.DestroyBoss();
    }

    public override void IncreaseStatsPerLevel()
    {

        Health += GameManager.Instance.Level() * 10f;
        CurrencyDrop += GameManager.Instance.Level() * 1f;
        Speed += GameManager.Instance.Level() * 0.1f;
    }

}
