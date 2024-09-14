using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : SpawnerEnemy
{


    public override void IncreaseStatsPerLevel()
    {

        Health.CurrentHealth += GameManager.Instance.Level * 10f;
        Health.CurrencyDrop += GameManager.Instance.Level * 1f;
        Kinematics.Speed += GameManager.Instance.Level * 0.1f;
    }

}
