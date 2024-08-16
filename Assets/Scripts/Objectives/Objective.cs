using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string objectiveName;
    public string objectiveDescription;
    public bool isCompleted;
    public int objectiveReward;

    public static event Action OnObjectiveCompleted;
    public abstract void CheckCompletion();

    protected void CompleteObjective()
    {
        if (!isCompleted)
        {
            isCompleted = true;
            OnObjectiveCompleted?.Invoke();
            GameManager.Instance.GetPlayer().AddCurrency(objectiveReward);
        }
    }
}
