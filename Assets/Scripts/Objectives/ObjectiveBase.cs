using UnityEngine;

public abstract class ObjectiveBase : ScriptableObject
{
    public string objectiveName;
    public string objectiveDescription;
    public bool isObjectiveCompleted;
    public bool isObjectiveFailed;
    public int rewardPoints;

    public abstract void Initialize();
    public abstract void UpdateObjective();
    public abstract void FailObjective();
    public virtual void CompleteObjective()
    {
        isObjectiveCompleted = true;
        PlayerManager.Instance.SetCurrency(PlayerManager.Instance.Currency() + rewardPoints);
    }

    public virtual void ResetObjective()
    {
        isObjectiveCompleted = false;
    }
}
