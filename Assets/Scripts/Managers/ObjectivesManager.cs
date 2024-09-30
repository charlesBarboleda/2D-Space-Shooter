using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [SerializeField] private List<ObjectiveBase> specialObjectives = new List<ObjectiveBase>();
    [SerializeField] private List<ObjectiveBase> generalObjectives = new List<ObjectiveBase>();

    public List<ObjectiveBase> activeObjectives = new List<ObjectiveBase>();
    UIManager _UIManager;

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
        if (_UIManager == null)
        {
            _UIManager = UIManager.Instance;
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
            _UIManager.AddObjectiveToUI(objectiveInstance);
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
        _UIManager.ClearObjectivesFromUI();
    }

    public void UpdateObjectivesUI()
    {
        Debug.Log("UpdateObjectivesUI called from ObjectiveManager");
        foreach (var objective in activeObjectives)
        {
            UIManager.Instance.UpdateObjectiveUI(objective);
        }
    }

    public void HandleObjectiveCompletion(ObjectiveBase objective)
    {
        if (objective.isObjectiveCompleted)
        {
            Debug.Log($"Objective Completed: {objective.objectiveName} - Reward: {objective.rewardPoints} points.");
            PlayerManager.Instance.SetCurrency(PlayerManager.Instance.Currency() + objective.rewardPoints);
            UIManager.Instance.MarkObjectiveAsComplete(objective);
        }
    }

    public void HandleObjectiveFailure(ObjectiveBase objective)
    {
        if (objective.isObjectiveFailed)
        {
            Debug.Log($"Objective Failed: {objective.objectiveName}");
            UIManager.Instance.MarkObjectiveAsFailed(objective);
        }
    }
    public ObjectiveBase GetObjectiveFromPool(string objectiveName)
    {
        ObjectiveBase foundObjective = specialObjectives.Find(obj => obj.objectiveName == objectiveName);

        if (foundObjective == null)
        {
            Debug.LogError($"Objective with name {objectiveName} not found in specialObjectives.");
        }

        return foundObjective;
    }

    public ObjectiveBase GetRandomObjectiveFromPool()
    {
        return generalObjectives[Random.Range(0, generalObjectives.Count)];
    }
}
