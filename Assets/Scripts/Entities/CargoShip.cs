using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoShip : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject spawnAnimation;
    [SerializeField] GameObject deathAnimation;
    [SerializeField] float _health = 1000;

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
        return _health;
    }

    public void SetHealth(float _health)
    {
        this._health = _health;

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

        _health -= damage;
        if (_health > 0)
        {
            StartCoroutine(FlashRed());
        }
        if (_health <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        CameraShake.Instance.TriggerShake(5, 0.3f);
        GameObject animation = Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
        gameObject.SetActive(false);
    }
}
