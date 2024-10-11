using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularBullet : Bullet
{
    SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        base.Awake();
    }

    protected override void OnEnable()
    {
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        base.OnEnable();
    }

    protected override IEnumerator BulletOnHitEffect()
    {
        if (_collider2D != null) _collider2D.enabled = false;
        if (_rb != null) _rb.velocity = Vector2.zero;
        if (_spriteRenderer != null) _spriteRenderer.enabled = false;
        _bulletOnHitEffect = ObjectPooler.Instance.SpawnFromPool("RegularBulletOnHitEffect", transform.position, Quaternion.identity);
        UIManager.Instance.CreateOnHitDamageText(Mathf.Round(BulletDamage).ToString(), transform.position);
        yield return new WaitForSeconds(0.3f);
        _bulletOnHitEffect.SetActive(false);
        Deactivate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("ThraxArmada") || collision.gameObject.CompareTag("Syndicates") || collision.gameObject.CompareTag("CrimsonFleet") || collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bullet hit layer check: " + collision.gameObject.name);
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(BulletDamage);
                StartCoroutine(BulletOnHitEffect());
                if (shouldIncreaseCombo)
                    ComboManager.Instance.IncreaseCombo();
            }
        }
    }
}


