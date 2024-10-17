using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed { get; private set; }
    public float BulletDamage { get; private set; }
    public float BulletLifetime { get; set; } = 5f;
    public bool shouldIncreaseCombo = false;
    protected GameObject _bulletOnHitEffect;
    protected Collider2D _collider2D;

    protected Rigidbody2D _rb;

    protected virtual void Awake()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        Vector3 position = transform.position;
        position.z = 0; // Locks the Z position to 0
        transform.position = position;
    }
    protected virtual void OnEnable()
    {
        if (_bulletOnHitEffect != null) _bulletOnHitEffect.SetActive(false);
        if (_collider2D != null) _collider2D.enabled = true;
        if (_rb != null)
        {
            _rb.simulated = true;
            _rb.velocity = Vector2.zero;
        }

    }

    public virtual void Initialize(float speed, float damage, float lifetime, Vector3 direction)
    {
        BulletSpeed = speed;
        BulletDamage = damage;
        BulletLifetime = lifetime;

        _rb.velocity = direction.normalized * BulletSpeed;

        if (BulletLifetime > 0)
        {
            Invoke(nameof(Deactivate), BulletLifetime);
        }
    }

    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }


    protected virtual IEnumerator BulletOnHitEffect()
    {
        yield return null;
    }




}

