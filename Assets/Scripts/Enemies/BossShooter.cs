using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : ShooterEnemy, IBoss
{
    public override void Destroy()
    {
        base.Destroy();
        ObjectivesManager.Instance.DestroyShooterBoss();
    }

}
