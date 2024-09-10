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

    void OnEnable()
    {
        EventManager.OnEnemyDestroyed += OnEnemyDestroyed;
    }

    void OnDisable()
    {
        EventManager.OnEnemyDestroyed -= OnEnemyDestroyed;

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
        if (activeObjectives.Contains(newObjective)) return;
        activeObjectives.Add(newObjective);
        newObjective.InitObjective(); // Initialize the new objective
        ObjectivesUIManager.Instance.AddObjectiveUI(newObjective); // Update UI with the new objective
    }
    public void RemoveAllObjectives()
    {
        activeObjectives.Clear();
    }


    void OnEnemyDestroyed(GameObject enemy, Faction faction)
    {
        foreach (Objective objective in activeObjectives)
        {
            // Handle destroyFormationTimed objectives
            if (objective is DestroyFormationTimed destroyFormationTimed && faction.factionType == FactionType.CrimsonFleet)
            {
                destroyFormationTimed.CurrentKills--;
            }

            // Handle other objectives (e.g., DestroyShipsTimed)
            else if (objective is DestroyShipsTimed destroyShipsTimed)
            {
                destroyShipsTimed.SetCurrentKills(destroyShipsTimed.GetCurrentKills() - 1);
            }
        }
    }


    public void DestroyBoss()
    {
        foreach (Objective objective in activeObjectives)
        {
            if (objective is DestroySpawnerBossObjective destroySpawnerBossObjective)
            {
                destroySpawnerBossObjective.SetCurrentKills(destroySpawnerBossObjective.GetCurrentKills() - 1);
            }
        }
        foreach (Objective objective in activeObjectives)
        {
            if (objective is DestroyShooterBossObjective destroyShooterBossObjective)
            {
                destroyShooterBossObjective.SetCurrentKills(destroyShooterBossObjective.GetCurrentKills() - 1);
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
    }
}
