using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroySpawnerBossObjective", menuName = "Objectives/DestroySpawnerBossObjective", order = 1)]
public class DestroySpawnerBossObjective : Objective
{
    [Header("Objective Settings")]
    [SerializeField] int _requiredKills;
    [SerializeField] int _currentKills;
    [SerializeField] float _timeToDestroy;
    [SerializeField] float _elapsedTime;
    [SerializeField] List<string> _bossNames;
    [SerializeField] List<Transform> _spawnPoints;

    [Header("Boss Stats")]

    [SerializeField] List<string> _spawnerNames;
    [SerializeField] float _health;
    [SerializeField] float _spawnRate;
    [SerializeField] float _currencyDrop;
    [SerializeField] float _speed;
    [SerializeField] float _aimRange;
    [SerializeField] float _stopDistance;

    public override void InitObjective()
    {
        _elapsedTime = _timeToDestroy;
        _requiredKills = _bossNames.Count;
        _currentKills = 0;
        IsCompleted = false;
        IsActive = true;
        IsFailed = false;
        ObjectiveID = Guid.NewGuid().ToString();
        foreach (string bossName in _bossNames)
        {
            GameObject bossShip = ObjectPooler.Instance.SpawnFromPool(bossName, _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position, Quaternion.identity);
            SpawnerManager.Instance.AddEnemy(bossShip);
            BossSpawner bossScript = bossShip.GetComponent<BossSpawner>();
            bossScript.EnemyID = ObjectiveID;
            if (bossShip.GetComponent<BossSpawner>() != null)
            {
                bossScript.Health.CurrentHealth = _health;
                bossScript.Health.MaxHealth = _health;

                bossScript.AttackManager.AttackCooldown = _spawnRate;
                bossScript.Health.CurrencyDrop = _currencyDrop;
                bossScript.Kinematics.MaxSpeed = _speed;
                bossScript.Kinematics.StopDistance = _stopDistance;
                bossScript.Ships = _spawnerNames;
            }


        }
    }

    public override void UpdateObjective()
    {
        if (IsCompleted || IsFailed) return;
        if (_elapsedTime <= 0) FailedObjective();
        if (_currentKills >= _requiredKills && _elapsedTime > 0)
        {
            _currentKills = _requiredKills;
            CompleteObjective();
        }
        _elapsedTime -= Time.deltaTime;

        if (IsCompleted) ObjectiveDescription = "Objective Completed";
        if (IsFailed) ObjectiveDescription = "Objective Failed";
        if (IsActive && !IsCompleted && !IsFailed) ObjectiveDescription = $"Destroy {_requiredKills} Syndicate Carrier ship: {_currentKills} destroyed. " + " Time Left: " + Mathf.Round(_elapsedTime) + " seconds";

    }
    public override void CompleteObjective()
    {
        MarkObjectiveCompleted();
    }

    public override void FailedObjective()
    {
        MarkObjectiveFailed();
    }

    public void RegisterKill()
    {
        if (IsActive && !IsCompleted && !IsFailed)
        {
            _currentKills++;
        }
    }

    public int CurrentKills { get => _currentKills; set => _currentKills = value; }
}
