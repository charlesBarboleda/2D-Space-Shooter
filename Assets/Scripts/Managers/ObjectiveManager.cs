using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;
    public List<Objective> activeObjectives;

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
        UpdateObjectives();
    }


    void Start()
    {
        foreach (Objective objective in activeObjectives)
        {
            objective.RegisterEventHandlers();
        }
    }

    void OnDestroy()
    {
        foreach (Objective objective in activeObjectives)
        {
            objective.UnregisterEventHandlers();
        }
    }

    public void UpdateObjectives()
    {
        UIManager.Instance.UpdateObjectivesUI(activeObjectives);
    }
}
