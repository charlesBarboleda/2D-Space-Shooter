using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static event System.Action OnShipDestroyed;
    public static event System.Action OnGameOver;
    public static event System.Action OnNextRound;

    public static void ShipDestroyed()
    {
        OnShipDestroyed?.Invoke();
    }

    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void NextRound()
    {
        OnNextRound?.Invoke();
    }
}
