using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public float pickUpRadius;
    public static event Action OnCurrencyChange;
    public HealthBar healthBar;
    SpriteRenderer spriteRenderer;
    public Weapon weapon;
    public PlayerController playerController;

    public float playerHealth = 100f;

    public float currency = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickUpRadius, LayerMask.GetMask("Debris"));

        // Iterate over each collider and trigger attraction
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Debris"))
            {
                Debug.Log("Debris detected: " + hit.name);
                hit.GetComponent<CurrencyDrop>().isAttracted = true;
            }
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        StartCoroutine(FlashRed());
        healthBar.SetHealth();
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddCurrency(float currency)
    {
        this.currency += currency;
        OnCurrencyChange?.Invoke();
    }

    public void RemoveCurrency(float currency)
    {
        this.currency -= currency;

    }

    public void AddHealth(float health)
    {
        playerHealth += health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(other.gameObject.GetComponent<Bullet>().BulletDamage);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Nuke"))
        {
            TakeDamage(1000);
        }

    }
}
