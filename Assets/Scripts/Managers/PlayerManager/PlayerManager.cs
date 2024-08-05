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


    public float playerHealth = 100f;

    public void TakeDamage(float damage)
    {
        if (playerShield.shield.activeSelf)
        {
            return;
        }
        playerHealth -= damage;
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
