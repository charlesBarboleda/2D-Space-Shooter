using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyShipsTimed", menuName = "Objectives/DestroyShipsTimed", order = 1)]
public class DestroyShipsTimed : Objective
{
    [SerializeField] float timeToDestroy;
    [SerializeField] float elapsedTime;
    [SerializeField] int requiredKills;
    [SerializeField] int currentKills;


    public override void InitObjective()
    {
        elapsedTime = timeToDestroy;
        currentKills = requiredKills;
        SetIsCompleted(false);
        SetIsActive(true);
        SetIsFailed(false);

    }
    public override void UpdateObjective()
    {
        if (GetIsCompleted()) return;

        elapsedTime -= Time.deltaTime;

        if (elapsedTime <= 0) MarkObjectiveFailed();
        if (currentKills == 0 && elapsedTime > 0)
        {
            currentKills = 0;
            CompleteObjective();
        }

        SetObjectiveDescription($"Destroy {currentKills} ships in {elapsedTime:F0} seconds");


    }
    public override void FailedObjective()
    {
        MarkObjectiveFailed();
    }
    public override void CompleteObjective()
    {
        if (currentKills == 0)
        {
            MarkObjectiveCompleted();
        }
    }

    public void SetTimeToDestroy(float time)
    {
        timeToDestroy = time;
    }
    public void SetRequiredKills(int kills)
    {
        requiredKills = kills;
    }

    public float GetTimeToDestroy()
    {
        return timeToDestroy;
    }


    public float GetRequiredKills()
    {
        return requiredKills;
    }
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public int GetCurrentKills()
    {
        return currentKills;
    }
    public void SetCurrentKills(int kills)
    {
        currentKills = kills;
    }

}
