using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoShip : MonoBehaviour
{
    [SerializeField] GameObject spawnAnimation;
    [SerializeField] GameObject deathAnimation;
    [SerializeField] float health = 1000;
    [SerializeField] float shieldHealth = 1000;

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

    IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            {
                health -= bullet.BulletDamage;
                StartCoroutine(FlashRed());
                if (health <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
            other.gameObject.SetActive(false);
        }
    }

}
