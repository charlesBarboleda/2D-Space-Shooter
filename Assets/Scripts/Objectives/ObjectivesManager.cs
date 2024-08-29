using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;


    public List<Objective> earlyObjectives = new List<Objective>();
    public List<Objective> midObjectives = new List<Objective>();
    public List<Objective> lateObjectives = new List<Objective>();
    public List<Objective> activeObjectives = new List<Objective>();

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

    void Update()
    {
        foreach (Objective objective in activeObjectives)
        {
            if (objective.GetIsActive())
            {
                objective.UpdateObjective();
                ObjectivesUIManager.Instance.UpdateObjectiveUI(objective); // Update UI when objectives change
            }
        }

    }

    public void SetActiveObjectives(List<Objective> objectives, int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, objectives.Count);
            activeObjectives.Add(objectives[randomIndex]);
        }
    }




    public void AddObjective(Objective newObjective)
    {
        activeObjectives.Add(newObjective);
        newObjective.InitObjective(); // Initialize the new objective
        ObjectivesUIManager.Instance.AddObjectiveUI(newObjective); // Update UI with the new objective
    }
    public void RemoveAllObjectives()
    {
        activeObjectives.Clear();
    }


    public void DestroyShip()
    {
        foreach (Objective objective in activeObjectives)
        {
            if (objective is DestroyShipsTimed destroyShipsTimed)
            {
                destroyShipsTimed.SetCurrentKills(destroyShipsTimed.GetCurrentKills() - 1);
            }
        }
    }



    public void DestroySpawnerBoss()
    {
        foreach (Objective objective in activeObjectives)
        {
            if (objective is DestroySpawnerBossObjective destroySpawnerBossObjective)
            {
                destroySpawnerBossObjective.SetCurrentKills(destroySpawnerBossObjective.GetCurrentKills() + 1);
            }
        }
    }
    public void DestroyShooterBoss()
    {
        foreach (Objective objective in activeObjectives)
        {
            if (objective is DestroyShooterBossObjective destroyShooterBossObjective)
            {
                destroyShooterBossObjective.SetCurrentKills(destroyShooterBossObjective.GetCurrentKills() + 1);
            }
        }
    }

    public void StartObjectives()
    {
        ObjectivesUIManager.Instance.InitializeUI(ObjectivesManager.Instance.activeObjectives); // Initialize UI with the objectives
        foreach (Objective objective in ObjectivesManager.Instance.activeObjectives)
        {
            objective.InitObjective();
        }
        Debug.Log("Objectives Initialized");
    }
}
