using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrodeBullet : Bullet
{
    ParticleSystem _particleSystem;

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
        GameObject flash = ObjectPooler.Instance.SpawnFromPool("CorrodeBulletFlash", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        flash.SetActive(false);
    }
    protected override IEnumerator BulletOnHitEffect()
    {
        if (_collider2D != null) _collider2D.enabled = false;
        if (_rb != null) _rb.simulated = false;
        if (_rb != null) _rb.velocity = Vector2.zero;
        if (_particleSystem != null) _particleSystem.Stop();
        _bulletOnHitEffect = ObjectPooler.Instance.SpawnFromPool("CorrodeBulletOnHitEffect", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        _bulletOnHitEffect.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ThraxArmada") || collision.gameObject.CompareTag("Syndicates") || collision.gameObject.CompareTag("CrimsonFleet") || collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            EnemyDebuffs enemyDebuffs = collision.gameObject.GetComponent<EnemyDebuffs>();
            // Check if the enemy is not already corroded
            if (damageable != null)
            {
                // Apply Corrode Effect to Enemy

                GameObject CorrodeEffect = ObjectPooler.Instance.SpawnFromPool("CorrodeEffect", collision.transform.position, Quaternion.identity);
                enemyDebuffs.debuffsList["Corrode"] = true;
                CorrodeEffect.GetComponent<CorrosionEffect>().ApplyCorrode(collision.gameObject, BulletDamage);
                StartCoroutine(BulletOnHitEffect());
            }
        }
    }

}



