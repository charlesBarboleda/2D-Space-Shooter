using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/DestroyShipsTimerObjective")]
public class DestroyShipsTimerObjective : Objective
{
    [SerializeField] private int targetDestroyAmount;
    [SerializeField] private float timeLimit;
    private int shipsToDestroy;
    private float remainingTime;

    public override void Initialize()
    {
        shipsToDestroy = targetDestroyAmount;
        remainingTime = timeLimit;
        isCompleted = false;
    }

    public override void UpdateObjective()
    {
        if (isCompleted) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            FailObjective();  // Optional: Implement a method to handle failing an objective
        }
        else if (shipsToDestroy <= 0 && !isCompleted)
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

    public float GetRemainingTime()
    {
        return remainingTime;
    }
}

