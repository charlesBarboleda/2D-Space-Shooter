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
        EventManager.OnRoundStart += StartObjectives;
    }

    void OnDisable()
    {
        EventManager.OnEnemyDestroyed -= OnEnemyDestroyed;
        EventManager.OnRoundStart -= StartObjectives;

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

    public void SetEarlyObjectives()
    {
        SetActiveObjectives(earlyObjectives, Random.Range(1, 3));
        Debug.Log("Early Objectives: " + activeObjectives.Count);
    }

    public void SetMidObjectives()
    {
        SetActiveObjectives(midObjectives, Random.Range(1, 3));
    }

    public void SetLateObjectives()
    {
        SetActiveObjectives(lateObjectives, Random.Range(1, 4));
    }

    void StartObjectives()
    {
        ObjectivesUIManager.Instance.InitializeUI(activeObjectives); // Initialize UI with the objectives
        foreach (Objective objective in activeObjectives)
        {
            objective.InitObjective();
            Debug.Log("Started objective: " + objective.name);
        }
    }

    public void SetActiveObjectives(List<Objective> objectives, int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            int randomIndex;
            Objective selectedObjective;

            // Ensure no duplicates by repeatedly selecting a random objective if it's already active
            do
            {
                randomIndex = Random.Range(0, objectives.Count);
                selectedObjective = objectives[randomIndex];
            } while (activeObjectives.Contains(selectedObjective));

            activeObjectives.Add(selectedObjective);
        }
    }

    public void RemoveAllObjectives()
    {
        activeObjectives.Clear();
        ObjectivesUIManager.Instance.ClearObjectivesUI();
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

}
