using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBehaviour : IDamageable
{
    public float maxHealth { get; private set; }
    public float currentHealth { get; private set; }
    public float regenRate { get; private set; }
    readonly HealthBar _healthBar;
    readonly ParticleSystem _healingParticles;
    readonly GameObject _deathEffectPrefab;
    readonly CoroutineRunner _coroutineRunner;
    SpriteRenderer _spriteRenderer;

    public HealthBehaviour(float maxHealth, float regenRate, HealthBar healthBar, ParticleSystem healingParticles, GameObject deathEffectPrefab, SpriteRenderer spriteRenderer, CoroutineRunner coroutineRunner)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.regenRate = regenRate;
        _healthBar = healthBar;
        _healingParticles = healingParticles;
        _deathEffectPrefab = deathEffectPrefab;
        _spriteRenderer = spriteRenderer;
        _coroutineRunner = coroutineRunner;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        _coroutineRunner.StartCoroutine(FlashRed());
        _healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject deathEffect = Object.Instantiate(_deathEffectPrefab, _spriteRenderer.transform.position, Quaternion.identity);
        Object.Destroy(deathEffect, 1f);
        EventManager.GameOverEvent();
        Object.Destroy(_spriteRenderer.gameObject);
    }

    public void SetRegenHealth(float amount)
    {
        regenRate = amount;
    }

    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
    }

    public void SetCurrentHealth(float amount)
    {
        currentHealth = amount;
    }

    public void RegenHealth()
    {
        if (currentHealth < maxHealth && currentHealth > 0)
        {
            _healingParticles.Play();
            currentHealth += regenRate * Time.deltaTime;
            UpdateHealthBar();
        }
        else _healingParticles.Stop();
    }

    void UpdateHealthBar()
    {
        _healthBar.SetHealth(currentHealth, maxHealth);
    }

    IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

}

