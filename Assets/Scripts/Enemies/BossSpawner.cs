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

        SetHealth(GetHealth() + GameManager.Instance.level * 10f);
        SetCurrencyDrop(GetCurrencyDrop() + GameManager.Instance.level * 10f);
        SetSpeed(GetSpeed() + GameManager.Instance.level * 0.05f);
    }

}
