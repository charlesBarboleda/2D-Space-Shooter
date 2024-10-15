using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechBullet : Bullet
{
    ParticleSystem _particleSystem;
    public float leechAmount = 0.5f;

    protected override void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        base.Awake();
    }

    protected override void OnEnable()
    {
        if (_particleSystem != null) _particleSystem.Play();
        base.OnEnable();
        StartCoroutine(BulletFlashEffect());
    }
    IEnumerator BulletFlashEffect()
    {
        yield return new WaitForEndOfFrame();
        GameObject flash = ObjectPooler.Instance.SpawnFromPool("LeechBulletFlash", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        flash.SetActive(false);
    }
    protected override IEnumerator BulletOnHitEffect()
    {
        if (_collider2D != null) _collider2D.enabled = false;
        if (_rb != null) _rb.velocity = Vector2.zero;
        if (_particleSystem != null) _particleSystem.Play();
        _bulletOnHitEffect = ObjectPooler.Instance.SpawnFromPool("LeechBulletOnHitEffect", transform.position, Quaternion.identity);
        UIManager.Instance.CreateOnHitDamageText(Mathf.Round(BulletDamage).ToString(), transform.position);
        yield return new WaitForSeconds(0.3f);
        _bulletOnHitEffect.SetActive(false);
        Deactivate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("LeechBullet Collision: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("ThraxArmada") || collision.gameObject.CompareTag("Syndicates") || collision.gameObject.CompareTag("CrimsonFleet") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("LeechBullet Collision with enemy: " + collision.gameObject.name);
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(BulletDamage);
                // Increase player health by 1% of the bullet damage by leeching
                Leech();
                StartCoroutine(BulletOnHitEffect());
                if (shouldIncreaseCombo)
                    ComboManager.Instance.IncreaseCombo();
            }
        }
    }

    void Leech()
    {
        PlayerManager.Instance.SetCurrentHealth(BulletDamage * leechAmount);

    }
}



