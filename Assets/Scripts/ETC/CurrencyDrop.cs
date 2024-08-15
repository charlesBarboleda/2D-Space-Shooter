using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDrop : MonoBehaviour
{
    float currencyWorth;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            player.AddCurrency(currencyWorth);
            Destroy(gameObject);
        }
    }

    public void SetCurrency(float currency)
    {
        this.currencyWorth = currency;
    }
}
