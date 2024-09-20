using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : ShooterEnemy
{
    [SerializeField] BossShooter _bossShooterEnemy;

    protected void Start()
    {
        if (_bossShooterEnemy != null) InitializeTurrets();
    }

    public override void IncreaseStatsPerLevel()
    {
        if (_bossShooterEnemy != null) _bossShooterEnemy.IncreaseStatsPerLevel();
    }

    void InitializeTurrets()
    {
        AttackManager.AimRange = _bossShooterEnemy.AttackManager.AimRange;
        BulletAmount = _bossShooterEnemy.BulletAmount;
        BulletDamage = _bossShooterEnemy.BulletDamage;
        BulletSpeed = _bossShooterEnemy.BulletSpeed;
        AttackManager.AttackCooldown = _bossShooterEnemy.AttackManager.AttackCooldown;
        ShootingAngle = _bossShooterEnemy.ShootingAngle;

    }
}
