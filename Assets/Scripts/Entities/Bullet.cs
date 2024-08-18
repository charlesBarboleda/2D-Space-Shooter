using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed { get; private set; }
    public float BulletDamage { get; private set; }
    public float BulletLifetime { get; private set; }

    private Rigidbody2D rb;


    private void OnEnable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(float speed, float damage, float lifetime, Vector3 direction)
    {
        this.BulletSpeed = speed;
        this.BulletDamage = damage;
        this.BulletLifetime = lifetime;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = direction.normalized * BulletSpeed;
        if (BulletLifetime > 0)
        {
            Invoke(nameof(Deactivate), BulletLifetime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && other.CompareTag("Player"))
        {
            damageable.TakeDamage(BulletDamage);
            gameObject.SetActive(false);
        }
    }
}
