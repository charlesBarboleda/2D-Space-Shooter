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
        foreach (string bossName in _bossNames)
        {
            GameObject bossShip = ObjectPooler.Instance.SpawnFromPool(bossName, _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position, Quaternion.identity);
            GameManager.Instance.enemies.Add(bossShip);
            BossSpawner bossScript = bossShip.GetComponent<BossSpawner>();
            if (bossShip.GetComponent<ShooterEnemy>() != null)
            {
                bossScript.SetHealth(_health);
                bossScript.SetSpawnRate(_spawnRate);
                bossScript.SetCurrencyDrop(_currencyDrop);
                bossScript.SetSpeed(_speed);
                bossScript.SetStopDistance(_stopDistance);
                bossScript.SetShips(_spawnerNames);
            }


        }
        SetIsCompleted(false);
        SetIsActive(true);
        SetIsFailed(false);
    }

    public override void UpdateObjective()
    {
        if (GetIsCompleted() || GetIsFailed()) return;
        if (_elapsedTime <= 0) FailedObjective();
        if (_currentKills == _requiredKills && _elapsedTime > 0)
        {
            _currentKills = 0;
            CompleteObjective();
        }
        _elapsedTime -= Time.deltaTime;

        if (GetIsCompleted()) SetObjectiveDescription("Objective Completed");
        if (GetIsFailed()) SetObjectiveDescription("Objective Failed");
        if (GetIsActive() && !GetIsCompleted() && !GetIsFailed()) SetObjectiveDescription("Destroy the Carrier ships: " + _currentKills + "/" + _requiredKills + " in " + Mathf.Round(_elapsedTime) + " seconds");

    }
    public override void CompleteObjective()
    {
        MarkObjectiveCompleted();
    }

    public override void FailedObjective()
    {
        MarkObjectiveFailed();
    }

    public void SetCurrentKills(int kills)
    {
        _currentKills = kills;
    }
    public int GetCurrentKills()
    {
        return _currentKills;
    }
}
