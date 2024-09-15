using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyShipsTimed", menuName = "Objectives/DestroyShipsTimed", order = 1)]
public class DestroyShipsTimed : Objective
{
    [SerializeField] float _timeToDestroy;
    [SerializeField] float _elapsedTime;
    [SerializeField] int _requiredKills;
    [SerializeField] int _currentKills;


    public override void InitObjective()
    {
        _elapsedTime = _timeToDestroy;
        _currentKills = 0;
        IsCompleted = false;
        IsActive = true;
        IsFailed = false;
        ObjectiveID = Guid.NewGuid().ToString();

    }
    public override void UpdateObjective()
    {
        if (IsCompleted || IsFailed) return;

        _elapsedTime -= Time.deltaTime;
        Debug.Log("Elapsed Time: " + _elapsedTime);
        if (_elapsedTime <= 0) FailedObjective();
        if (_currentKills >= _requiredKills && _elapsedTime > 0)
        {
            CompleteObjective();
            _currentKills = _requiredKills;
        }

        if (IsCompleted) ObjectiveDescription = "Objective Completed";
        if (IsFailed) ObjectiveDescription = "Objective Failed";
        if (IsActive && !IsCompleted && !IsFailed) ObjectiveDescription = $"Destroy {_requiredKills} ships: {_currentKills} destroyed. Time Left: {_elapsedTime:F0} seconds";




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
