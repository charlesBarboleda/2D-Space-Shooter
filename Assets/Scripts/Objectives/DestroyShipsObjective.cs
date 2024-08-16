using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/DestroyShipsObjective")]
public class DestroyShipsObjective : Objective
{

    public override void RegisterEventHandlers()
    {
        EventManager.OnShipDestroyed += OnShipDestroyed;
    }

    public override void UnregisterEventHandlers()
    {
        EventManager.OnShipDestroyed -= OnShipDestroyed;
    }

    private void OnShipDestroyed()
    {
        currentAmount++;
        if (currentAmount >= targetAmount)
        {
            isCompleted = true;
        }
    }
}
