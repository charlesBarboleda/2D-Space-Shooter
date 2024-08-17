using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private List<Objective> objectives = new List<Objective>();

    public void AddObjective(Objective objective)
    {
        objectives.Add(objective);
        objective.OnStateChanged += HandleObjectiveStateChange;
    }

    private void HandleObjectiveStateChange(ObjectiveState state)
    {
        Debug.Log($"Objective state changed to: {state}");
    }

    public void StartAllObjectives()
    {
        foreach (var objective in objectives)
        {
            objective.StartObjective();
        }
    }

    private void ClearObjectives()
    {
        foreach (var objective in objectives)
        {
            objective.OnStateChanged -= HandleObjectiveStateChange;
        }
        objectives.Clear();
    }

}
