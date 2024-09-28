using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InvasionObjective", menuName = "Objectives/InvasionObjective", order = 1)]
public class InvasionObjective : Objective
{

    [SerializeField] int _requiredKills;
    [SerializeField] int _currentKills;


    public override void InitObjective()
    {
        _requiredKills =
        _currentKills = 0;
        IsCompleted = false;
        IsActive = true;
        IsFailed = false;
        ObjectiveID = Guid.NewGuid().ToString();

    }
    public override void UpdateObjective()
    {
        if (IsCompleted || IsFailed) return;


        if (_currentKills >= _requiredKills)
        {
            CompleteObjective();
            _currentKills = _requiredKills;
        }

        if (IsCompleted) ObjectiveDescription = "Objective Completed";
        if (IsActive && !IsCompleted && !IsFailed) ObjectiveDescription = $"Eliminate the invading forces OR protect the defending faction!";




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


    public float TimeToDestroy { get => _timeToDestroy; set => _timeToDestroy = value; }
    public float RequiredKills { get => _requiredKills; set => _requiredKills = (int)value; }
    public float ElapsedTime { get => _elapsedTime; set => _elapsedTime = value; }
    public int CurrentKills { get => _currentKills; set => _currentKills = value; }


}
