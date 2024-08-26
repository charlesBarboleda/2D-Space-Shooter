using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;
    public List<Objective> objectives = new List<Objective>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ObjectivesUIManager.Instance.InitializeUI(objectives); // Initialize UI with the objectives
        Debug.Log("Objectives Initialized");
        foreach (Objective objective in objectives)
        {
            objective.InitObjective();
        }
    }

    void Update()
    {
        foreach (Objective objective in objectives)
        {
            if (objective.GetIsActive())
            {
                objective.UpdateObjective();
                ObjectivesUIManager.Instance.UpdateObjectiveUI(objective); // Update UI when objectives change
            }
        }
    }

    public void AddObjective(Objective newObjective)
    {
        objectives.Add(newObjective);
        newObjective.InitObjective(); // Initialize the new objective
        ObjectivesUIManager.Instance.AddObjectiveUI(newObjective); // Update UI with the new objective
    }
    public void RemoveAllObjectives()
    {
        objectives.Clear();
    }


    public void DestroyShip()
    {
        foreach (Objective objective in objectives)
        {
            if (objective is DestroyShipsTimed destroyShipsTimed)
            {
                destroyShipsTimed.SetCurrentKills(destroyShipsTimed.GetCurrentKills() - 1);
            }
        }
    }
}
