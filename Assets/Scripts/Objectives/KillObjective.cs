using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/KillObjective")]
public class KillObjective : Objective
{
    public int requiredKills;
    private int currentKills;

    public void RegisterKill()
    {
        currentKills++;
        CheckCompletion();
    }

    public override void CheckCompletion()
    {
        if (currentKills >= requiredKills)
        {
            CompleteObjective();

        }
    }
}
