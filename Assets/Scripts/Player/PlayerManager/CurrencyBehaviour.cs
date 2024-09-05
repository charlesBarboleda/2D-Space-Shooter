using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrencyBehaviour : MonoBehaviour
{
    [SerializeField] public float currency { get; private set; } = 0f;

    public void SetCurrency(float amount)
    {
        currency = amount;
        EventManager.CurrencyChangeEvent(currency);
    }

}
