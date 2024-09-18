using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static event Action<GameObject> OnEnemyDestroyed;

    public static event System.Action OnGameOver;
    public static event System.Action OnNextLevel;
    public static event System.Action OnLevelStart;
    public static event System.Action OnLevelComplete;

    public static event System.Action<float> OnCurrencyChange;


    public static void CurrencyChangeEvent(float currency)
    {
        OnCurrencyChange?.Invoke(currency);
    }
    public static void AnyShipDestroyedEvent(GameObject go)
    {
        OnEnemyDestroyed?.Invoke(go);
    }
    public static void GameOverEvent()
    {
        OnGameOver?.Invoke();
    }

    public static void NextLevelEvent()
    {
        OnNextLevel?.Invoke();
    }

    public static void LevelStartEvent()
    {
        OnLevelStart?.Invoke();
    }

    public static void LevelCompleteEvent()
    {
        OnLevelComplete?.Invoke();
    }


}
