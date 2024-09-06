using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour, IPickable
{
    public bool isAttracted { get => isAttracted; set => isAttracted = value; }
    public float maxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float currencyWorth;

    void FixedUpdate()
    {

        if (isAttracted)
        {
            MoveTowardsPlayer();
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            player.SetCurrency(player.Currency() + currencyWorth);
            Destroy(gameObject);

        }
    }

    public void SetCurrency(float currency)
    {
        this.currencyWorth = currency;
    }

    public void MoveTowardsPlayer()
    {
        Vector2 playerPosition = PlayerManager.GetInstance().transform.position;
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, maxSpeed * Time.deltaTime);
    }

    public void Effect()
    {

    }
}
