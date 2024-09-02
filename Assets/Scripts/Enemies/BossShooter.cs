using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : ShooterEnemy
{
    public override void Destroy()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        base.Destroy();
        ObjectivesManager.Instance.DestroyShooterBoss();
    }

    public override void IncreaseStatsPerLevel()
    {

        SetHealth(GetHealth() + GameManager.Instance.Level() * 10f);
        SetCurrencyDrop(GetCurrencyDrop() + GameManager.Instance.Level() * 10f);
        SetSpeed(GetSpeed() + GameManager.Instance.Level() * 0.05f);

    }



}
