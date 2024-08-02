using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager Instance { get; private set; }
    public static EventManager GetInstance() { return Instance; }

    void Awake()
    {
        SetSingleton();
    }
    void SetSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
