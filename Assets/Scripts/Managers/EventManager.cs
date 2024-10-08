using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{

    public static event Action<GameObject> OnEnemyDestroyed;
    public static event Action<FactionType> OnFactionInvasionWon;

    public static event Action OnGameOver;
    public static event Action OnNextLevel;
    public static event Action OnLevelStart;
    public static event Action OnLevelComplete;
    public static event Action<Ability> OnUltimateReady;
    public static event Action<Ability> OnUltimateUnready;

    public static event System.Action<float> OnCurrencyChange;

    public static void UltimateReadyEvent(Ability ability)
    {
        OnUltimateReady?.Invoke(ability);
    }

    public static void UltimateUnreadyEvent(Ability ability)
    {
        OnUltimateUnready?.Invoke(ability);
    }

    public static void CurrencyChangeEvent(float currency)
    {
        OnCurrencyChange?.Invoke(currency);
    }
    public static void EnemyShipDestroyedEvent(GameObject go)
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

    public static void FactionInvasionWonEvent(FactionType faction)
    {
        OnFactionInvasionWon?.Invoke(faction);
    }


}
