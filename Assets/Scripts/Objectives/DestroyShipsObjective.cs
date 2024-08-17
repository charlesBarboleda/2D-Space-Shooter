using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyShipsObjective : Objective
{

    private int enemiesToDestroy;
    private int enemiesDestroyed;

    public DestroyShipsObjective(int enemiesToDestroy)
    {
        this.enemiesToDestroy = enemiesToDestroy;
    }

    protected override void OnStart()
    {
        Debug.Log("Kill Enemies Objective Started");
    }

    protected override void OnComplete()
    {
        GameManager.Instance.GetPlayer().AddCurrency(reward);
        Debug.Log("Kill Enemies Objective Completed");
    }

    protected override void OnFail()
    {
        Debug.Log("Kill Enemies Objective Failed");
    }

    public void OnEnemyKilled()
    {
        if (State == ObjectiveState.Active)
        {
            enemiesDestroyed++;
            if (enemiesDestroyed >= enemiesToDestroy)
            {
                CompleteObjective();
            }
        }
    }
}
