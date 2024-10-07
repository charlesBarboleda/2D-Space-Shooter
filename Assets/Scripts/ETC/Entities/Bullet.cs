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
    GameObject _bulletOnHitEffect;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;

    Rigidbody2D _rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 position = transform.position;
        position.z = 0; // Locks the Z position to 0
        transform.position = position;
    }
    void OnEnable()
    {
        if (_bulletOnHitEffect != null) _bulletOnHitEffect.SetActive(false);
        if (_boxCollider2D != null) _boxCollider2D.enabled = true;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        if (_rb != null)
        {
            _rb.simulated = true;
            _rb.velocity = Vector2.zero;
        }

    }

    public void Initialize(float speed, float damage, float lifetime, Vector3 direction)
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

    private void Deactivate()
    {
        _rb.velocity = Vector2.zero;
        _rb.simulated = false;
        if (_boxCollider2D != null) _boxCollider2D.enabled = false;
        if (_spriteRenderer != null) _spriteRenderer.enabled = false;

        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CrimsonFleet") || other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates"))
        {

            StartCoroutine(BulletOnHitEffect());
            other.GetComponent<IDamageable>()?.TakeDamage(BulletDamage);
            ComboManager.Instance.IncreaseCombo();

        }
        if (other.CompareTag("Player") || other.CompareTag("EnemyDestroyable"))
        {
            StartCoroutine(BulletOnHitEffect());
            other.GetComponent<IDamageable>()?.TakeDamage(BulletDamage);

        }
    }

    IEnumerator BulletOnHitEffect()
    {


        _spriteRenderer.enabled = false;
        _boxCollider2D.enabled = false;
        _bulletOnHitEffect = ObjectPooler.Instance.SpawnFromPool("BulletHitEffect", transform.position, Quaternion.identity);

        UIManager.Instance.CreateOnHitDamageText(Mathf.Round(BulletDamage).ToString(), transform.position);
        yield return new WaitForSeconds(0.1f);
        _bulletOnHitEffect.SetActive(false);
        Deactivate();
    }





}

