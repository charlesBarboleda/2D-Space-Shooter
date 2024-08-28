using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyShooterBossObjective", menuName = "Objectives/DestroyShooterBossObjective", order = 1)]
public class DestroyShooterBossObjective : Objective
{
    [Header("Objective Settings")]
    [SerializeField] int _requiredKills;
    [SerializeField] int _currentKills;
    [SerializeField] float _timeToDestroy;
    [SerializeField] float _elapsedTime;
    [SerializeField] List<GameObject> _bossPrefabs;
    [SerializeField] List<Transform> _spawnPoints;

    [Header("Boss Stats")]

    [SerializeField] int _health;
    [SerializeField] int _fireRate;
    [SerializeField] int _bulletAmount;
    [SerializeField] int _bulletSpeed;
    [SerializeField] int _bulletDamage;
    [SerializeField] int _shootingAngle;
    [SerializeField] int _currencyDrop;
    [SerializeField] int _speed;
    [SerializeField] int _aimRange;
    [SerializeField] int _stopDistance;

    public override void InitObjective()
    {
        _elapsedTime = _timeToDestroy;
        _requiredKills = _bossPrefabs.Count;
        _currentKills = 0;
        foreach (GameObject boss in _bossPrefabs)
        {
            GameObject bossShip = Instantiate(boss, _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position, Quaternion.identity);
            BossShooter bossScript = bossShip.GetComponent<BossShooter>();
            if (bossShip.GetComponent<ShooterEnemy>() != null)
            {
                bossScript.SetHealth(_health);
                bossScript.SetBulletAmount(_bulletAmount);
                bossScript.SetBulletSpeed(_bulletSpeed);
                bossScript.SetBulletDamage(_bulletDamage);
                bossScript.SetFireRate(_fireRate);
                bossScript.SetShootingAngle(_shootingAngle);
                bossScript.SetCurrencyDrop(_currencyDrop);
                bossScript.SetSpeed(_speed);
                bossScript.SetAimRange(_aimRange);
                bossScript.SetStopDistance(_stopDistance);
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
        if (GetIsActive() && !GetIsCompleted() && !GetIsFailed()) SetObjectiveDescription("Destroy the Assault ships: " + _currentKills + "/" + _requiredKills + " in " + Math.Round(_elapsedTime, 0) + " seconds");

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

