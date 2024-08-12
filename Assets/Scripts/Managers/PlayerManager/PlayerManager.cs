using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public PlayerShield playerShield;
    public Laser laser;

    public Weapon weapon;
    public Turret turret;

    public HealthBar healthBar;
    public Teleport playerTeleport;
    private SpriteRenderer spriteRenderer;

    public float playerHealth = 100f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public void TakeDamage(float damage)
    {
        if (playerShield.shield.activeSelf)
        {
            return;
        }
        playerHealth -= damage;
        StartCoroutine(FlashRed());
        healthBar.SetHealth();
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }
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
