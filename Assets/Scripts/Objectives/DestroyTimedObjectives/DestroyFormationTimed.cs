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
        _requiredKills = 0; // Initialize required kills

        foreach (string formationName in _formationNames)
        {
            int _randomIndex = Random.Range(0, _formationSpawnPoints.Count);
            Vector2 targetDirection = PlayerManager.Instance.transform.position - _formationSpawnPoints[_randomIndex].position;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);

            // Spawn the formation positions
            GameObject formation = ObjectPooler.Instance.SpawnFromPool(formationName, _formationSpawnPoints[_randomIndex].position, targetRotation);

            // Get the total positions of the formation
            FleetFormation formationScript = formation.GetComponent<FleetFormation>();

            // Spawn the ships in the formation
            foreach (Transform shipSpawn in formationScript.FormationPositions)
            {
                // Spawn the ships in the formation
                GameObject ship = ObjectPooler.Instance.SpawnFromPool(_shipNames[Random.Range(0, _shipNames.Count)], shipSpawn.position, Quaternion.identity);
                _requiredKills++;
                GameManager.Instance.AddEnemy(ship);
            }
            foreach (Transform bossSpawn in formationScript.BossPositions)
            {
                // Spawn the bosses in the formation
                GameObject boss = ObjectPooler.Instance.SpawnFromPool(_bossNames[Random.Range(0, _bossNames.Count)], bossSpawn.position, Quaternion.identity);
                _requiredKills++;
                GameManager.Instance.AddEnemy(boss);
            }
            formation.SetActive(false);
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
        else if (GetIsActive() && !GetIsCompleted() && !GetIsFailed()) SetObjectiveDescription($"Eliminate the invading Crimson Fleet: {_currentKills} ships in {_elapsedTime:F0} seconds");




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
