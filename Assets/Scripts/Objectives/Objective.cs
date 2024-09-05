using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    [SerializeField] string objectiveDescription;
    [SerializeField] bool isCompleted;
    [SerializeField] bool isActive;
    [SerializeField] bool isFailed;
    [SerializeField] float reward;

    public abstract void InitObjective();
    public abstract void UpdateObjective();
    public abstract void CompleteObjective();
    public abstract void FailedObjective();

    protected void GiveReward()
    {
        PlayerManager.GetInstance().SetCurrency(PlayerManager.GetInstance().Currency() + reward);
    }
    protected void MarkObjectiveFailed()
    {
        isFailed = true;
        isActive = true;
        isCompleted = false;
    }
    protected void MarkObjectiveCompleted()
    {
        GiveReward();
        isCompleted = true;
        isActive = true;
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

    public void SetIsFailed(bool status)
    {
        isFailed = status;
    }
    public float GetReward()
    {
        return reward;
    }

    public void SetReward(float reward)
    {
        this.reward = reward;
    }


}
