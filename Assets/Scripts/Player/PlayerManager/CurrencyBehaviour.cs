using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrencyBehaviour : MonoBehaviour
{
    [SerializeField] public float currency;

    public void SetCurrency(float amount)
    {
        currency += amount;
        EventManager.CurrencyChangeEvent(currency);
    }

}
