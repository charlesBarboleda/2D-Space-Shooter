using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloShooterBossLevel : Level
{

    string _bossName;
    List<Vector3> _spawnPoints = new List<Vector3>();
    LevelManager _levelManager;
    SpawnerManager _spawnerManager;
    GameObject _bossShip;
    float _health;
    float _bulletDamage;
    float _bulletSpeed;
    float _fireRate;
    float _speed;
    float _stopDistance;
    float _attackRange;
    float _fireAngle;
    float _currencyDrop;


    public SoloShooterBossLevel(float health, float bulletDamage, float bulletSpeed, float firerate, float speed, float stopDistance, float attackRange, float fireAngle, float currencyDrop, List<Vector3> spawnPoints, string bossName, LevelManager levelManager, SpawnerManager spawnerManager)
    {
        _health = health;
        _bulletDamage = bulletDamage;
        _bulletSpeed = bulletSpeed;
        _fireRate = firerate;
        _speed = speed;
        _stopDistance = stopDistance;
        _attackRange = attackRange;
        _fireAngle = fireAngle;
        _spawnPoints = spawnPoints;
        _bossName = bossName;
        _currencyDrop = currencyDrop;
        _leveltype = LevelType.Boss;
        _levelManager = levelManager;
        _spawnerManager = spawnerManager;
    }
    public override void StartLevel()
    {
        Debug.Log("Starting Boss Level");
        _bossShip = _spawnerManager.SpawnShip(_bossName, _spawnPoints[Random.Range(0, _spawnPoints.Count)], Quaternion.identity);

        // Get the components
        Health health = _bossShip.GetComponent<Health>();
        Kinematics kinematics = _bossShip.GetComponent<Kinematics>();
        AttackManager attackManager = _bossShip.GetComponent<AttackManager>();
        ShooterEnemy shooterEnemy = _bossShip.GetComponent<ShooterEnemy>();

        // Set the stats
        health.MaxHealth = _health;
        health.CurrentHealth = _health;
        health.CurrencyDrop = _currencyDrop;
        shooterEnemy.BulletDamage = _bulletDamage;
        shooterEnemy.BulletSpeed = _bulletSpeed;
        shooterEnemy.ShootingAngle = _fireAngle;
        kinematics.Speed = _speed;
        kinematics.StopDistance = _stopDistance;
        attackManager.AttackCooldown = _fireRate;
        attackManager.AimRange = _attackRange;
    }

    public override void UpdateLevel()
    {
        Debug.Log("Updating Boss Level");
        if (_bossShip == null || _bossShip.GetComponent<Health>().isDead || !_bossShip.activeInHierarchy)
            CompleteLevel();

    }

    public override void CompleteLevel()
    {
        Debug.Log("Completing Level");
        _levelManager.CompleteLevel();
    }


}
