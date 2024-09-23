using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloSpawnerBossLevel : Level
{

    string _bossName;
    List<Vector3> _spawnPoints = new List<Vector3>();
    LevelManager _levelManager;
    SpawnerManager _spawnerManager;
    GameObject _bossShip;
    float _health;
    int _shipsPerSpawn;
    float _spawnRate;
    float _speed;
    float _stopDistance;
    float _attackRange;
    float _currencyDrop;


    public SoloSpawnerBossLevel(float health, float speed, float spawnRate, float stopDistance, int shipsPerSpawn, float attackRange, float currencyDrop, List<Vector3> spawnPoints, string bossName, LevelManager levelManager, SpawnerManager spawnerManager)
    {
        _health = health;
        _speed = speed;
        _stopDistance = stopDistance;
        _attackRange = attackRange;
        _spawnPoints = spawnPoints;
        _spawnRate = spawnRate;
        _bossName = bossName;
        _shipsPerSpawn = shipsPerSpawn;
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
        SpawnerEnemy enemy = _bossShip.GetComponent<SpawnerEnemy>();

        // Set the stats
        health.MaxHealth = _health;
        health.CurrentHealth = _health;
        health.CurrencyDrop = _currencyDrop;
        enemy.ShipsPerSpawn = _shipsPerSpawn;
        kinematics.MaxSpeed = _speed;
        kinematics.StopDistance = _stopDistance;
        attackManager.AttackCooldown = _spawnRate;
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
