using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : ShooterEnemy
{
    [SerializeField] BossShooter _bossShooterEnemy;

    public override void Start()
    {
        InitializeTurrets();
    }
    // Update is called once per frame
    public override void Update()
    {
        Aim(CheckForTargets());
        float distanceToTarget = Vector2.Distance(transform.position, CheckForTargets().position);
        if (distanceToTarget < aimRange && Time.time >= nextFireTime)

        {
            Attack();
        }
        else
        {
            if (firingCoroutine != null)
            {
                StopAttack();

            }


        }
    }

    public override void IncreaseStatsPerLevel()
    {
        _bossShooterEnemy.IncreaseStatsPerLevel();
    }

    void InitializeTurrets()
    {
        SetAimRange(_bossShooterEnemy.GetAimRange());
        SetBulletAmount(_bossShooterEnemy.GetBulletAmount());
        SetBulletDamage(_bossShooterEnemy.GetBulletDamage());
        SetBulletSpeed(_bossShooterEnemy.GetBulletSpeed());
        SetFireRate(_bossShooterEnemy.GetFireRate());
        SetShootingAngle(_bossShooterEnemy.GetShootingAngle());


    }
}
