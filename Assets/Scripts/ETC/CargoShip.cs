using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoShip : MonoBehaviour
{
    [SerializeField] GameObject cargoShield;
    [SerializeField] GameObject spawnAnimation;
    [SerializeField] GameObject deathAnimation;
    [SerializeField] float health = 100;
    [SerializeField] float shieldHealth = 100;

    void OnEnable()
    {
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
    }

    void OnDisable()
    {
        GameObject animation = Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetShieldHealth()
    {
        return shieldHealth;
    }

    public void SetHealth(float health)
    {
        this.health = health;

    }

    public void SetShieldHealth(float shieldHealth)
    {
        this.shieldHealth = shieldHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (shieldHealth > 0)
            {
                shieldHealth -= bullet.BulletDamage;
                if (shieldHealth <= 0)
                {
                    cargoShield.SetActive(false);
                }
            }
            else
            {
                health -= bullet.BulletDamage;
                if (health <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
            other.gameObject.SetActive(false);
        }
    }

}
