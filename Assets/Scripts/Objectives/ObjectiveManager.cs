using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> activeObjectives = new();

    void Start()
    {
        EventManager.OnShipDestroyed += HandleShipDestroyed;

    }

    void Update()
    {
        foreach (var objective in activeObjectives)
        {
            objective.UpdateObjective();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartObjectives();
        }
    }

    public void StartObjectives()
    {
        foreach (var objective in activeObjectives)
        {
            objective.Initialize();
        }
    }

    void OnDestroy()
    {
        EventManager.OnShipDestroyed -= HandleShipDestroyed;
    }

    public void AddObjective(Objective objective)
    {
        activeObjectives.Add(objective);
        objective.Initialize();
    }

    public void RemoveObjective(Objective objective)
    {
        if (activeObjectives.Contains(objective))
        {
            activeObjectives.Remove(objective);
        }
    }

    private void HandleShipDestroyed()
    {
        foreach (var objective in activeObjectives)
        {
            if (objective is DestroyShipsObjective destroyShipsObjective)
            {
                destroyShipsObjective.ShipDestroyed();
            }
            else if (objective is DestroyShipsTimerObjective destroyShipsTimerObjective)
            {
                destroyShipsTimerObjective.ShipDestroyed();
            }
        }
    }
}
