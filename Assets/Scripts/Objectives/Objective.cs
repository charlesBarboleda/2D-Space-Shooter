using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject, IObjective
{
    [Header("Objective Details")]
    public string description;
    public bool isCompleted { get; set; }
    public int objectiveReward;

    [Header("Optional Objective Details")]
    public int targetAmount;
    public int currentAmount;


    public abstract void RegisterEventHandlers();
    public abstract void UnregisterEventHandlers();

}
