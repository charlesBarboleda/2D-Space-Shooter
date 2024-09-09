using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrimsonFleetTimed : Objective
{
    [SerializeField] float _timeToDestroy;
    [SerializeField] float _elapsedTime;
    [SerializeField] int _requiredKills;
    [SerializeField] int _currentKills;


    public override void InitObjective()
    {
        _elapsedTime = _timeToDestroy;
        _currentKills = _requiredKills;
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
        if (GetIsFailed()) SetObjectiveDescription("Objective Failed");
        if (GetIsActive() && !GetIsCompleted() && !GetIsFailed()) SetObjectiveDescription($"Destroy {_currentKills} ships in {_elapsedTime:F0} seconds");




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
