using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [SerializeField] private List<ObjectiveBase> availableObjectives = new List<ObjectiveBase>();
    private List<ObjectiveBase> activeObjectives = new List<ObjectiveBase>();
    private UIManager uiManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (uiManager == null)
        {
            uiManager = UIManager.Instance;
        }
    }

    // Method to start objectives for a specific level
    public void StartObjectivesForLevel(Level level)
    {
        ClearActiveObjectives();
        List<ObjectiveBase> objectivesToAssign = level.GetLevelObjectives();

        foreach (var objective in objectivesToAssign)
        {
            ObjectiveBase objectiveInstance = Instantiate(objective);
            objectiveInstance.Initialize();
            activeObjectives.Add(objectiveInstance);
            uiManager.AddObjectiveToUI(objectiveInstance);
        }

        Debug.Log($"Objectives initialized for Level: {level.GetType().Name}");
    }

    // Update all active objectives
    void Update()
    {
        foreach (var objective in activeObjectives)
        {
            if (!objective.isObjectiveCompleted)
            {
                objective.UpdateObjective();
            }
        }
    }

    // Clear all active objectives and reset the UI
    public void ClearActiveObjectives()
    {
        foreach (var objective in activeObjectives)
        {
            objective.ResetObjective();
        }
        activeObjectives.Clear();
        uiManager.ClearObjectivesFromUI();
    }

    // Handle completion of an objective
    public void HandleObjectiveCompletion(ObjectiveBase objective)
    {
        if (objective.isObjectiveCompleted)
        {
            Debug.Log($"Objective Completed: {objective.objectiveName} - Reward: {objective.rewardPoints} points.");
            PlayerManager.Instance.SetCurrency(PlayerManager.Instance.Currency() + objective.rewardPoints);
            uiManager.MarkObjectiveAsComplete(objective);
        }
    }
    public ObjectiveBase GetObjectiveFromPool(string objectiveName)
    {
        ObjectiveBase foundObjective = availableObjectives.Find(obj => obj.objectiveName == objectiveName);

        if (foundObjective == null)
        {
            Debug.LogError($"Objective with name {objectiveName} not found in availableObjectives.");
        }

        return foundObjective;
    }
}
