using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public float cooldown;
    public abstract void UseAbility(GameObject owner, Transform target);
}
