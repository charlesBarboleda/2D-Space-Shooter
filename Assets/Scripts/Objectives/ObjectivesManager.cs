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

        for (int i = 0; i < amount; i++)
        {
            int randomIndex;
            Objective selectedObjective;

            // Ensure no duplicates by repeatedly selecting a random objective if it's already active
            do
            {
                randomIndex = Random.Range(0, objectives.Count);
                selectedObjective = objectives[randomIndex];
            } while (!activeObjectives.Contains(selectedObjective));

            activeObjectives.Add(selectedObjective);
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

        foreach (Objective objective in activeObjectives)
        {
            if (objective.IsActive)
            {
                if (objective is DestroyShipsTimed destroyShipsTimed)
                {
                    if (_activeObjectivesID.Contains(destroyShipsTimed.ObjectiveID))
                        destroyShipsTimed.RegisterKill();

                }
                if (objective is DestroyShooterBossObjective destroyShooterBoss)
                {
                    if (_activeObjectivesID.Contains(destroyShooterBoss.ObjectiveID))
                        destroyShooterBoss.RegisterKill();
                }
                if (objective is DestroyFormationTimed destroyFormationTimed)
                {
                    if (_activeObjectivesID.Contains(destroyFormationTimed.ObjectiveID))
                        destroyFormationTimed.RegisterKill();
                }
                if (objective is DestroySpawnerBossObjective destroySpawnerBoss)
                {
                    if (_activeObjectivesID.Contains(destroySpawnerBoss.ObjectiveID))
                        destroySpawnerBoss.RegisterKill();
                }
            }
        }
    }

}
