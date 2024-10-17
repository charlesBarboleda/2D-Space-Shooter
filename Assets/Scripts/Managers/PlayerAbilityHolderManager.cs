using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityHolderManager : MonoBehaviour
{
    public static PlayerAbilityHolderManager Instance;
    AbilityHolder _abilityHolder;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }
    void Start()
    {
        _abilityHolder = PlayerManager.GetInstance().AbilityHolder();
    }


}
