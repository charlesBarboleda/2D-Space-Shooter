using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyCrimsonFleetTimed", menuName = "Objectives/DestroyCrimsonFleetTimed", order = 1)]
public class DestroyCrimsonFleetTimed : Objective
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
        _requiredKills = 0; // Initialize required kills

        foreach (string formationName in _formationNames)
        {
            int randomIndex = Random.Range(0, _formationSpawnPoints.Count);
            Vector3 targetDirection = (PlayerManager.Instance.transform.position - _formationSpawnPoints[randomIndex].position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Spawn the formation at a random spawn point
            GameObject formation = ObjectPooler.Instance.SpawnFromPool(formationName, _formationSpawnPoints[randomIndex].position, targetRotation);
            if (formation == null) continue;

            FleetFormation fleetFormation = formation.GetComponent<FleetFormation>();
            if (fleetFormation == null) continue;

            // Ensure all spawn points are used
            List<Transform> usedShipSpawnPoints = new List<Transform>(fleetFormation.FormationPositions);
            List<Transform> usedBossSpawnPoints = new List<Transform>(fleetFormation.BossPositions);

            // Spawn ships
            for (int i = 0; i < fleetFormation.FormationPositions.Count; i++)
            {
                if (i >= _shipNames.Count) break;

                Transform shipSpawnPoint = fleetFormation.FormationPositions[i];
                GameObject ship = ObjectPooler.Instance.SpawnFromPool(_shipNames[i % _shipNames.Count], shipSpawnPoint.position, Quaternion.identity);
                if (ship == null) continue;

                GameManager.Instance.AddEnemy(ship);
                _requiredKills++;
            }

            // Spawn bosses
            for (int i = 0; i < fleetFormation.BossPositions.Count; i++)
            {
                if (i >= _bossNames.Count) break;

                Transform bossSpawnPoint = fleetFormation.BossPositions[i];
                GameObject boss = ObjectPooler.Instance.SpawnFromPool(_bossNames[i % _bossNames.Count], bossSpawnPoint.position, Quaternion.identity);
                if (boss == null) continue;

                GameManager.Instance.AddEnemy(boss);
                _requiredKills++;
            }
        }

        _currentKills = _requiredKills; // Initialize current kills
        SetIsCompleted(false);
        SetIsActive(true);
        SetIsFailed(false);
    }

    public override void UpdateObjective()
    {
        if (GetIsCompleted() || GetIsFailed()) return;

        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0) FailedObjective();
        if (_currentKills <= 0 && _elapsedTime > 0)
        {
            _currentKills = 0;
            CompleteObjective();
        }

        if (GetIsCompleted()) SetObjectiveDescription("Objective Completed");
        else if (GetIsFailed()) SetObjectiveDescription("Objective Failed");
        else if (GetIsActive() && !GetIsCompleted() && !GetIsFailed()) SetObjectiveDescription($"Eliminate the Crimson Fleet: {_currentKills} ships in {_elapsedTime:F0} seconds");




    }
    public override void FailedObjective()
    {
        MarkObjectiveFailed();
    }
    public override void CompleteObjective()
    {
        if (_currentKills == 0)
        {
            MarkObjectiveCompleted();
        }
    }

    public int CurrentKills { get => _currentKills; set => _currentKills = value; }
    public int RequiredKills { get => _requiredKills; set => _requiredKills = value; }
    public float TimeToDestroy { get => _timeToDestroy; set => _timeToDestroy = value; }

}
