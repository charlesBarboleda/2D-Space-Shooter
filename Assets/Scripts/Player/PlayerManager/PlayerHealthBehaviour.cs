using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth = 100f;
    [SerializeField] public float healthRegenRate = 1f;
    [SerializeField] ParticleSystem _healingParticles;
    [SerializeField] GameObject _deathExplosionPrefab;
    SpriteRenderer _spriteRenderer;
    [SerializeField] bool _isDead;
    public bool isDead { get => _isDead; set => _isDead = value; }
    [SerializeField] List<string> _deathEffect;
    [SerializeField] string _deathExplosion;
    public List<string> deathEffect { get => _deathEffect; set => _deathEffect = value; }
    public string deathExplosion { get => _deathExplosion; set => _deathExplosion = value; }
    bool hasRevived = false;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RegenHealth();
        CapHealth();
    }
    void CapHealth()
    {
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        OnPlayerDamageCameraEffect.Instance.DamageEffect();
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            if (PlayerManager.Instance.PrestigeManager().chosenPrestige == PrestigeType.Lifewarden && !hasRevived)
            {
                Revive();
                hasRevived = true;
            }
            else
            {
                Die();
            }
        }
    }

    void Revive()
    {
        StartCoroutine(ReviveAnimation());
    }

    IEnumerator ReviveAnimation()
    {
        isDead = true;
        GameObject animation = ObjectPooler.Instance.SpawnFromPool("LifewardenPrestigeAnimation", transform.position, Quaternion.identity);
        animation.transform.SetParent(transform);
        float t = 0;
        while (t < 5)
        {
            t += Time.deltaTime;
            currentHealth = Mathf.Lerp(0, maxHealth, t / 5);
            yield return null;
        }
        isDead = false;
        animation.SetActive(false);
    }

    public void Die()
    {
        StartCoroutine(HandleDeath());
    }
    IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    void RegenHealth()
    {
        if (currentHealth < maxHealth && currentHealth > 0)
        {
            _healingParticles.gameObject.SetActive(true);
            currentHealth += healthRegenRate * Time.deltaTime;
        }
        else _healingParticles.gameObject.SetActive(false);
    }

    public void SetMaxHealth(float newMaxHealth) => maxHealth += newMaxHealth;
    public void SetCurrentHealth(float newCurrentHealth) => currentHealth += newCurrentHealth;
    public void SetHealthRegenRate(float newHealthRegenRate) => healthRegenRate += newHealthRegenRate;

    public IEnumerator HandleDeath()
    {
        isDead = true;
        _spriteRenderer.enabled = false;


        EventManager.GameOverEvent();
        yield return StartCoroutine(DeathAnimation());
    }

    public IEnumerator DeathAnimation()
    {
        GameObject deathEffect = Instantiate(_deathExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(deathEffect, 1f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
