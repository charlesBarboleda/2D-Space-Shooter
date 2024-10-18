using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentCurrencyManager : MonoBehaviour
{
    float _permanentCurrency;

    void Start()
    {
        _permanentCurrency = 0;
    }

    public float CurrencyCalculator(int amountOfKills, int levelsSurvived, float objectivesCompleted, float timeSurvived)
    {
        return (amountOfKills * 10) + (levelsSurvived * 100) + (objectivesCompleted * 500) + (timeSurvived * 2);
    }


}
