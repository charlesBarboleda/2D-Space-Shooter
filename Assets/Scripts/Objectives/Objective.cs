using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject, IObjective
{
    [TextArea]
    [SerializeField] private string description;
    protected bool isCompleted;
    [SerializeField] int reward;

    public string Description => description;
    public bool IsCompleted => isCompleted;
    public int Reward => reward;

    public abstract void Initialize();
    public abstract void UpdateObjective();

    protected void CompleteObjective()
    {
        isCompleted = true;
        Debug.Log($"{description} completed!");
        GiveReward(reward);
    }

    protected void FailObjective()
    {
        isCompleted = false;
        Debug.LogWarning($"{description} failed.");
    }

    protected void GiveReward(int reward)
    {
        GameManager.Instance.GetPlayer().AddCurrency(reward);
    }
}
