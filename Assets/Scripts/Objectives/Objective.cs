using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective
{

    public ObjectiveState State { get; private set; } = ObjectiveState.InActive;
    public int reward { get; private set; }

    public event Action<ObjectiveState> OnStateChanged;

    public void StartObjective()
    {
        if (State == ObjectiveState.InActive)
        {
            State = ObjectiveState.Active;
            OnStateChanged?.Invoke(State); // Notify subscribers
            OnStart();
        }
    }

    public void CompleteObjective()
    {
        if (State == ObjectiveState.Active)
        {
            State = ObjectiveState.Completed;
            OnStateChanged?.Invoke(State); // Notify subscribers
            OnComplete();
        }
    }

    public void FailObjective()
    {
        if (State == ObjectiveState.Active)
        {
            State = ObjectiveState.Failed;
            OnStateChanged?.Invoke(State); // Notify subscribers
            OnFail();
        }
    }
    public void SetReward(int reward)
    {
        this.reward = reward;
    }

    protected abstract void OnStart();
    protected abstract void OnComplete();
    protected abstract void OnFail();
}

