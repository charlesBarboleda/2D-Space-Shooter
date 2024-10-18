using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBullet : Bullet
{
    ParticleSystem _particleSystem;
    public int amountOfReflections = 2;

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
        GameObject flash = ObjectPooler.Instance.SpawnFromPool("ReflectBulletFlash", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        flash.SetActive(false);
    }

    protected override IEnumerator BulletOnHitEffect()
    {
        _bulletOnHitEffect = ObjectPooler.Instance.SpawnFromPool("ReflectBulletOnHitEffect", transform.position, Quaternion.identity);
        UIManager.Instance.CreateOnHitDamageText(Mathf.Round(BulletDamage).ToString(), transform.position);
        yield return new WaitForSeconds(0.3f);
        _bulletOnHitEffect.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ThraxArmada") || collision.gameObject.CompareTag("Syndicates") || collision.gameObject.CompareTag("CrimsonFleet") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Asteroid"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(BulletDamage);
                StartCoroutine(BulletOnHitEffect());
                if (gameObject.CompareTag("PlayerBullet"))
                {
                    EventManager.BulletDamageEvent(BulletDamage);
                    EventManager.PlayerDamageDealtEvent(BulletDamage);
                    if (shouldIncreaseCombo)
                        ComboManager.Instance.IncreaseCombo();
                }
                // Reflect the bullet
                if (amountOfReflections > 0)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, _rb.velocity.normalized);
                    if (hit.collider != null)
                    {
                        // Reflect the bullet and add randomization to the angle
                        Vector2 direction = Vector2.Reflect(_rb.velocity.normalized, hit.normal);

                        // Randomize the angle slightly by rotating the direction vector
                        float randomAngle = Random.Range(-90f, 90f); // You can adjust the angle range
                        direction = Quaternion.Euler(0, 0, randomAngle) * direction;

                        _rb.velocity = direction * BulletSpeed;
                        amountOfReflections--;
                    }
                }
            }
        }
    }
}



