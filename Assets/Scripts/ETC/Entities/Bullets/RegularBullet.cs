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
        StartCoroutine(BulletFlashEffect());
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

    IEnumerator BulletFlashEffect()
    {
        yield return new WaitForEndOfFrame();
        GameObject flash = ObjectPooler.Instance.SpawnFromPool("RegularBulletFlash", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        flash.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ThraxArmada") || collision.gameObject.CompareTag("Syndicates") || collision.gameObject.CompareTag("CrimsonFleet") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Asteroid"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(BulletDamage);
                if (gameObject.CompareTag("PlayerBullet"))
                {
                    EventManager.BulletDamageEvent(BulletDamage);
                    EventManager.PlayerDamageDealtEvent(BulletDamage);
                    if (shouldIncreaseCombo)
                        ComboManager.Instance.IncreaseCombo();
                }
                StartCoroutine(BulletOnHitEffect());
            }
        }
    }
}


