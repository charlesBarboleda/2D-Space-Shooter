using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyBehaviour
{
    public float currency { get; private set; }

    public CurrencyBehaviour(float startingCurrency)
    {
        currency = startingCurrency;
    }
    void SetCurrency(float amount)
    {
        currency = amount;
    }


}
