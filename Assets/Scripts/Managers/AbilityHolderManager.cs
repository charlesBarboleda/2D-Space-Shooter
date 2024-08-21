using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolderManager : MonoBehaviour
{
    public static AbilityHolderManager Instance;
    AbilityHolder abilityHolder;
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
        abilityHolder = GameManager.Instance.GetPlayer().GetComponent<AbilityHolder>();
    }

    public void UnlockSkill(Ability ability)
    {
        ability.isUnlocked = true;
        Debug.Log($"{ability.name} unlocked!");
    }
}
