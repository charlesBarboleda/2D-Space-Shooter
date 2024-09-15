using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;

    List<string> _activeObjectivesID = new List<string>();
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

        EventManager.OnRoundStart += StartObjectives;
        EventManager.OnEnemyDestroyed += OnAnyEnemyDestroyed;


    }

    void OnDisable()
    {
        EventManager.OnRoundStart -= StartObjectives;
        EventManager.OnEnemyDestroyed -= OnAnyEnemyDestroyed;



    }

    void Update()
    {
        if (activeObjectives.Count == 0) return;
        foreach (Objective objective in activeObjectives)
        {
            if (objective.IsActive)
            {
                objective.UpdateObjective();
                ObjectivesUIManager.Instance.UpdateObjectiveUI(objective); // Update UI when objectives change
            }
        }

    }

    public void SetObjectives(string level, int amount)
    {
        switch (level)
        {
            case "Early":
                SetActiveObjectives(earlyObjectives, Random.Range(1, amount + 1));
                break;
            case "Mid":
                SetActiveObjectives(midObjectives, Random.Range(1, amount + 1));
                break;
            case "Late":
                SetActiveObjectives(lateObjectives, Random.Range(1, amount + 1));
                break;
        }

    }


    void StartObjectives()
    {
        ObjectivesUIManager.Instance.InitializeUI(activeObjectives); // Initialize UI with the objectives
        foreach (Objective objective in activeObjectives)
        {
            objective.InitObjective();
            _activeObjectivesID.Add(objective.ObjectiveID);
        }



    }

    public void SetActiveObjectives(List<Objective> objectives, int amount)
    {

        int attempts = 0; // To prevent infinite loops in case there are not enough valid objectives.

        while (activeObjectives.Count < amount && attempts < objectives.Count * 2) // Safety check to avoid too many iterations.
        {
            int randomIndex = Random.Range(0, objectives.Count);
            Objective selectedObjective = objectives[randomIndex];

            // Check if the selected objective type is not already in the active objectives list.
            if (!activeObjectives.Exists(x => x.GetType() == selectedObjective.GetType()))
            {
                activeObjectives.Add(selectedObjective);
            }

            attempts++; // Increase attempt counter
        }

    }

    public void RemoveAllObjectives()
    {
        activeObjectives.Clear();
        ObjectivesUIManager.Instance.ClearObjectivesUI();
    }


    void OnAnyEnemyDestroyed(GameObject enemy)
    {
        if (activeObjectives.Count == 0) return;
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        foreach (Objective objective in activeObjectives)
        {
            if (objective.IsActive && _activeObjectivesID.Contains(objective.ObjectiveID))
            {
                // Call RegisterKill only for the objective that matches the ID
                if (objective is DestroyShipsTimed destroyShipsTimed)
                {
                    destroyShipsTimed.RegisterKill();
                }
                else if (objective is DestroyShooterBossObjective destroyShooterBoss && enemyScript.EnemyID == objective.ObjectiveID)
                {
                    destroyShooterBoss.RegisterKill();
                }
                else if (objective is DestroyFormationTimed destroyFormationTimed && enemyScript.EnemyID == objective.ObjectiveID)
                {
                    destroyFormationTimed.RegisterKill();
                }
                else if (objective is DestroySpawnerBossObjective destroySpawnerBoss && enemyScript.EnemyID == objective.ObjectiveID)
                {
                    destroySpawnerBoss.RegisterKill();
                }
            }
        }
    }
}
