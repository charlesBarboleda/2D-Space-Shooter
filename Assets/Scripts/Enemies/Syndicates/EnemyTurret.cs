using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : ShooterEnemy
{
    [SerializeField] BossShooter _bossShooterEnemy;

    protected override void Start()
    {
        base.Start();
        if (_bossShooterEnemy != null) InitializeTurrets();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void IncreaseStatsPerLevel()
    {
        if (_bossShooterEnemy != null) _bossShooterEnemy.IncreaseStatsPerLevel();
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
