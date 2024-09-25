using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityUtils;

public class SuicideBomber : Enemy
{
    [SerializeField] float _explosionRadius = 5f;
    [SerializeField] float _explosionDamage = 100f;
    [SerializeField] float _glowLengthRate = 0.5f;
    [SerializeField] float _glowTickRate = 2f;

    [SerializeField] Material _regularMaterial;

    [SerializeField] Material _glowMaterial;
    [SerializeField] float _attackRange;
    [SerializeField] string _explosionEffect;
    [SerializeField] AudioClip _explosionSound;

    protected override void Update()
    {
        base.Update();
        if (Time.time % _glowTickRate < 0.1f)
        {
            StartCoroutine(Glow());
        }

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Health.SpriteRenderer.enabled = true;
        Health.Colliders.ForEach(collider => collider.enabled = true);
        Health.Rigidbody.simulated = true;
    }
    void OnDisable()
    {
        StopAllCoroutines();

    }

    public override void BuffedState()
    {
        base.BuffedState();
        _explosionRadius = _explosionRadius * 1.5f;
        _explosionDamage = _explosionDamage * 1.5f;
    }

    public override void UnBuffedState()
    {
        base.UnBuffedState();
        _explosionRadius = _explosionRadius / 1.5f;
        _explosionDamage = _explosionDamage / 1.5f;
    }


    protected override void Attack()
    {
        if (Health.isDead) return;
        StartCoroutine(Explode());
    }
    IEnumerator Glow()
    {
        Health.SpriteRenderer.material = _glowMaterial;
        yield return new WaitForSeconds(_glowLengthRate);
        Health.SpriteRenderer.material = _regularMaterial;
    }

    IEnumerator ExplosionAnimation()
    {
        if (_explosionEffect != null)
        {
            GameObject explosion = ObjectPooler.Instance.SpawnFromPool(_explosionEffect, transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(_explosionRadius / 3, _explosionRadius / 3, 1);
            if (_explosionSound != null)
            {
                AudioSource.PlayOneShot(_explosionSound);
            }
            yield return new WaitForSeconds(1f);
            explosion.SetActive(false);

        }
    }

    IEnumerator Explode()
    {
        LayerMask layerMasks = LayerMask.GetMask("Player", "EnemyDestroyable", "Syndicates", "ThraxArmada");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, layerMasks);

        foreach (Collider2D hit in colliders)
        {
            if (hit.CompareTag("Player") || hit.CompareTag("EnemyDestroyable") || hit.CompareTag("Syndicates") || hit.CompareTag("ThraxArmada"))
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(_explosionDamage);
                }
            }
        }
        Health.isDead = true;

        // Stops the exhaust particles
        Health.ExhaustChildren.ForEach(child => child.SetActive(false));

        // Disable all colliders
        foreach (var collider in Health.Colliders)
        {
            collider.enabled = false;
        }

        // Disable the rigidbody
        Health.Rigidbody.simulated = false;

        // Hide the ship's sprite
        Health.SpriteRenderer.enabled = false;

        // Shake the camera
        CameraShake.Instance.TriggerShakeMid(0.1f);

        //Notify Objectives Manager
        // ObjectivesManager.Instance.DestroyCrimsonShipsTimed();

        // Notify Event Manager
        EventManager.EnemyShipDestroyedEvent(EnemyID, gameObject);

        // Create the debris
        GameObject currency = ObjectPooler.Instance.SpawnFromPool(Health.CurrencyPrefab[Random.Range(0, Health.CurrencyPrefab.Count)], transform.position, transform.rotation);
        currency.GetComponent<Debris>().SetCurrency(Health.CurrencyDrop);

        yield return StartCoroutine(ExplosionAnimation());
        gameObject.SetActive(false);
    }

    public override void IncreaseStatsPerLevel()
    {
        base.IncreaseStatsPerLevel();
        _explosionDamage += LevelManager.Instance.CurrentLevelIndex * 1f;
    }

    /// <summary>
    /// Getters and Setters
    /// </summary>
    public float ExplosionDamage { get => _explosionDamage; set => _explosionDamage = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }

}
