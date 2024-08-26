using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    [SerializeField] string objectiveDescription;
    [SerializeField] bool isCompleted;
    [SerializeField] bool isActive;
    [SerializeField] bool isFailed;

    public abstract void InitObjective();
    public abstract void UpdateObjective();
    public abstract void CompleteObjective();
    public abstract void FailedObjective();

    protected void MarkObjectiveFailed()
    {
        isFailed = true;
        isCompleted = false;
        isActive = false;
    }
    protected void MarkObjectiveCompleted()
    {
        isCompleted = true;
        isActive = false;
        isFailed = false;
    }
    public string GetObjectiveDescription()
    {
        return objectiveDescription;
    }
    public void SetObjectiveDescription(string description)
    {
        objectiveDescription = description;
    }
    public bool GetIsCompleted()
    {
        return isCompleted;
    }
    public void SetIsCompleted(bool completed)
    {
        isCompleted = completed;
    }
    public bool GetIsActive()
    {
        return isActive;
    }
    public void SetIsActive(bool active)
    {
        isActive = active;
    }

    public bool GetIsFailed()
    {
        return isFailed;
    }

    public void SetISFailed(bool status)
    {
        isFailed = status;
    }


}
