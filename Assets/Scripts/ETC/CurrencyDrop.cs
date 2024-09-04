using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDrop : MonoBehaviour
{
    float currencyWorth;

    public bool isAttracted;
    public float maxSpeed = 100f;

    void FixedUpdate()
    {

        if (isAttracted)
        {
            MoveTowardsPlayer();
        }
    }
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

    private void MoveTowardsPlayer()
    {
        Vector2 playerPosition = PlayerManager.Player().transform.position;
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, maxSpeed * Time.deltaTime);
    }
}
