using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    Level _level;
    [SerializeField] string _objectiveDescription;
    [SerializeField] bool _isCompleted;
    [SerializeField] bool _isActive;
    [SerializeField] bool _isFailed;
    [SerializeField] float _reward;
    [SerializeField] string _objectiveID;

    public abstract void InitObjective();
    public abstract void UpdateObjective();
    public abstract void CompleteObjective();
    public abstract void FailedObjective();


    protected void GiveReward()
    {
        PlayerManager.GetInstance().SetCurrency(PlayerManager.GetInstance().Currency() + _reward);
    }
    protected void MarkObjectiveFailed()
    {
        _isFailed = true;
        _isActive = true;
        _isCompleted = false;
    }
    protected void MarkObjectiveCompleted()
    {
        GiveReward();
        _isCompleted = true;
        _isActive = true;
        _isFailed = false;
    }

    public string ObjectiveDescription { get => _objectiveDescription; set => _objectiveDescription = value; }
    public bool IsCompleted { get => _isCompleted; set => _isCompleted = value; }

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsFailed { get => _isFailed; set => _isFailed = value; }
    public float Reward { get => _reward; set => _reward = value; }
    public string ObjectiveID { get => _objectiveID; set => _objectiveID = value; }




}
