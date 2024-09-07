using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] float _duration = 30f;
    public float duration { get => _duration; set => _duration = value; }
    bool _isActive = false;
    public bool isActive { get => _isActive; set => _isActive = value; }


    protected abstract void Effect();
    public abstract void DeactivateEffect();

}
