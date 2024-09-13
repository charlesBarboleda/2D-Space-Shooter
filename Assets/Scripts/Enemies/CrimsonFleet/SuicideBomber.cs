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
        StartCoroutine(Explode());
        Debug.Log("Exploding");
    }
    IEnumerator Glow()
    {
        SpriteRenderer.material = _glowMaterial;
        yield return new WaitForSeconds(_glowLengthRate);
        SpriteRenderer.material = _regularMaterial;
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
                Debug.Log("Playing explosion sound.");
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
        isDead = true;

        // Stops the exhaust particles
        exhaustChildren.ForEach(child => child.SetActive(false));

        // Disable all colliders
        BoxColliders.ForEach(collider => collider.enabled = false);

        // Hide the ship's sprite
        SpriteRenderer.enabled = false;

        // Shake the camera
        CameraShake.Instance.TriggerShake(CameraShakeMagnitude, CameraShakeDuration);

        // Notify Objectives Manager
        // ObjectivesManager.Instance.DestroyCrimsonShipsTimed();

        // Notify Event Manager
        EventManager.EnemyDestroyedEvent(gameObject, Faction);

        // Create the debris
        GameObject currency = Instantiate(CurrencyPrefab[Random.Range(0, CurrencyPrefab.Count)], transform.position, transform.rotation);
        currency.GetComponent<Debris>().SetCurrency(CurrencyDrop);
        yield return StartCoroutine(ExplosionAnimation());
        gameObject.SetActive(false);
    }

    public override void IncreaseStatsPerLevel()
    {
        base.IncreaseStatsPerLevel();
        _explosionDamage += GameManager.Instance.Level * 1f;
        _explosionRadius += GameManager.Instance.Level * 0.1f;
    }

    /// <summary>
    /// Getters and Setters
    /// </summary>
    public float ExplosionRadius { get => _explosionRadius; set => _explosionRadius = value; }
    public float ExplosionDamage { get => _explosionDamage; set => _explosionDamage = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }

}
