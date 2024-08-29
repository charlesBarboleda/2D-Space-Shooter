using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject spawnAnimation;
    [SerializeField] GameObject deathAnimation;
    [SerializeField] float health = 1000f;

    void OnEnable()
    {
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
    }

    public void TeleportAway()
    {
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
        gameObject.SetActive(false);
    }


    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float health)
    {
        this.health = health;

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
                TakeDamage(bullet.BulletDamage);
                StartCoroutine(FlashRed());
            }
            other.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            StartCoroutine(FlashRed());
        }
        if (health <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        CameraShake.Instance.TriggerShake(5, 0.3f);
        GameManager.Instance.enemies.Remove(gameObject);
        GameObject animation = Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
        gameObject.SetActive(false);
    }
}
