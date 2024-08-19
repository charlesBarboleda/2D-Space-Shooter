using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static event Action<GameObject> OnEnemyDestroyed;
    public static event System.Action OnGameOver;
    public static event System.Action OnNextRound;
    public static event System.Action OnRoundStart;



    public static void EnemyDestroyedEvent(GameObject enemy)
    {
        OnEnemyDestroyed?.Invoke(enemy);
    }
    public static void GameOverEvent()
    {
        OnGameOver?.Invoke();
    }

    public static void NextRoundEvent()
    {
        OnNextRound?.Invoke();
    }

    public static void RoundStartEvent()
    {
        OnRoundStart?.Invoke();
    }


}
