using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/DestroyShipsObjective")]
public class DestroyShipsObjective : Objective
{
    [SerializeField] private int targetDestroyAmount;
    private int shipsToDestroy;

    public override void Initialize()
    {
        shipsToDestroy = targetDestroyAmount;
        isCompleted = false;
    }

    public override void UpdateObjective()
    {
        if (isCompleted) return;

        if (shipsToDestroy <= 0 && !isCompleted)
        {
            shipsToDestroy = 0;
            CompleteObjective();
        }
    }

    public void ShipDestroyed()
    {
        if (isCompleted) return;

        shipsToDestroy--;
        UpdateObjective();
    }

    public int GetRemainingShips()
    {
        return shipsToDestroy;
    }
}
