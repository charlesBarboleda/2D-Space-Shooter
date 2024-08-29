using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : SpawnerEnemy
{
    public override void Destroy()
    {
        base.Destroy();
        ObjectivesManager.Instance.DestroySpawnerBoss();

    }

    public override void IncreaseStatsPerLevel()
    {

        SetHealth(GetHealth() + GameManager.Instance.Level() * 10f);
        SetCurrencyDrop(GetCurrencyDrop() + GameManager.Instance.Level() * 10f);
        SetSpeed(GetSpeed() + GameManager.Instance.Level() * 0.05f);
    }

}
