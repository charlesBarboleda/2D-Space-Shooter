using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        _health += health;
        _speed += speed;
        _stopDistance += stopDistance;
        _attackRange += attackRange;
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
        if (Random.value < 0.15f)
            _spawnerManager.StartCoroutine(StartRandomObjective());
        Background.Instance.PlaySoloBossMusic();
        var chosenSpawnPoint = SpawnerManager.Instance.SoloBossSpawnPoints[Random.Range(0, SpawnerManager.Instance.SoloBossSpawnPoints.Count)];
        Debug.Log("Chosen Boss Spawn Point: " + chosenSpawnPoint);
        _bossShip = _spawnerManager.SpawnShip(_bossName, chosenSpawnPoint, Quaternion.identity);

        Debug.Log("Boss Ship Spawned at: " + _bossShip.transform.position);
        _spawnerManager.StartCoroutine(CheckBossPositionAfterDelay());

        _spawnerManager.StartCoroutine(CameraFollowBehaviour.Instance.PanToTargetAndBack(_bossShip.transform, 6f));
        // Get the components
        Health health = _bossShip.GetComponent<Health>();
        Kinematics kinematics = _bossShip.GetComponent<Kinematics>();
        AttackManager attackManager = _bossShip.GetComponent<AttackManager>();
        SpawnerEnemy enemy = _bossShip.GetComponent<SpawnerEnemy>();
        NavMeshAgent navMeshAgent = _bossShip.GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
        navMeshAgent.enabled = true;


        // Set the stats
        health.MaxHealth = _health;
        health.CurrentHealth = _health;
        health.CurrencyDrop = _currencyDrop;
        enemy.ShipsPerSpawn = _shipsPerSpawn;
        kinematics.Speed = _speed;
        kinematics.StopDistance = _stopDistance;
        attackManager.AttackCooldown = _spawnRate;
        attackManager.AimRange = _attackRange;
    }
    IEnumerator CheckBossPositionAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Boss Ship Position After Delay: " + _bossShip.transform.position);
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

    IEnumerator StartRandomObjective()
    {
        yield return new WaitForSeconds(Random.Range(0, 10));
        ObjectiveBase randomObjective = ObjectiveManager.Instance.GetRandomObjectiveFromPool();
        if (randomObjective != null)
        {
            _levelObjectives.Add(randomObjective);
        }
        ObjectiveManager.Instance.StartObjectivesForLevel(this);
    }


}
