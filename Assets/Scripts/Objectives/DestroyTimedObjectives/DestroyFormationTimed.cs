using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyCrimsonFleetTimed", menuName = "Objectives/DestroyCrimsonFleetTimed", order = 1)]
public class DestroyFormationTimed : Objective
{
    [SerializeField] float _timeToDestroy;
    [SerializeField] float _elapsedTime;
    [SerializeField] int _requiredKills;
    [SerializeField] int _currentKills;
    [SerializeField] List<string> _shipNames;
    [SerializeField] List<string> _bossNames;
    [SerializeField] List<string> _formationNames;
    [SerializeField] List<Transform> _formationSpawnPoints;


    public override void InitObjective()
    {
        _elapsedTime = _timeToDestroy;
        _currentKills = 0;
        IsCompleted = false;
        IsActive = true;
        IsFailed = false;
        ObjectiveID = Guid.NewGuid().ToString();


        foreach (string formationName in _formationNames)
        {
            int _randomIndex = UnityEngine.Random.Range(0, _formationSpawnPoints.Count);
            Vector2 targetDirection = PlayerManager.Instance.transform.position - _formationSpawnPoints[_randomIndex].position;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);

            // Spawn the formation positions
            GameObject formation = ObjectPooler.Instance.SpawnFromPool(formationName, _formationSpawnPoints[_randomIndex].position, targetRotation);

            // Get the total positions of the formation
            FleetFormation formationScript = formation.GetComponent<FleetFormation>();
            _requiredKills = formationScript.FormationPositions.Count + formationScript.BossPositions.Count;
            // Spawn the ships in the formation
            foreach (Transform shipSpawn in formationScript.FormationPositions)
            {
                // Spawn the ships in the formation
                GameObject ship = ObjectPooler.Instance.SpawnFromPool(_shipNames[UnityEngine.Random.Range(0, _shipNames.Count)], shipSpawn.position, Quaternion.identity);
                Enemy enemyScript = ship.GetComponent<Enemy>();
                enemyScript.EnemyID = ObjectiveID;
                GameManager.Instance.AddEnemy(ship);

            }
            foreach (Transform bossSpawn in formationScript.BossPositions)
            {
                // Spawn the bosses in the formation
                GameObject boss = ObjectPooler.Instance.SpawnFromPool(_bossNames[UnityEngine.Random.Range(0, _bossNames.Count)], bossSpawn.position, Quaternion.identity);
                Enemy enemyScript = boss.GetComponent<Enemy>();
                enemyScript.EnemyID = ObjectiveID;
                GameManager.Instance.AddEnemy(boss);

            }
            formation.SetActive(false);
        }

    }

    public override void UpdateObjective()
    {
        if (IsCompleted || IsFailed) return;

        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0) FailedObjective();
        if (_currentKills >= _requiredKills && _elapsedTime > 0)
        {
            CompleteObjective();
            _currentKills = _requiredKills;
        }

        if (IsCompleted) ObjectiveDescription = "Objective Completed";
        else if (IsFailed) ObjectiveDescription = "Objective Failed";
        else if (IsActive && !IsCompleted && !IsFailed) ObjectiveDescription = $"Eliminate {_requiredKills} invading Crimson Fleet: {_currentKills} destroyed. Time Limit: {_elapsedTime:F0} seconds";

    }
    public override void FailedObjective()
    {
        MarkObjectiveFailed();
    }
    public override void CompleteObjective()
    {

        MarkObjectiveCompleted();

    }

    public void RegisterKill()
    {
        if (IsActive && !IsCompleted && !IsFailed)
        {
            _currentKills++;
        }
    }

    public int CurrentKills { get => _currentKills; set => _currentKills = value; }
    public int RequiredKills { get => _requiredKills; set => _requiredKills = value; }
    public float TimeToDestroy { get => _timeToDestroy; set => _timeToDestroy = value; }

}
