using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManagerLegacy : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject deathEffectPrefab;
    public static PlayerManagerLegacy Instance;
    public AbilityHolder abilityHolder;
    public PlayerMovementBehaviour playerMovementBehaviour;

    public float pickUpRadius;
    public static event Action OnCurrencyChange;
    SpriteRenderer spriteRenderer;
    public Weapon weapon;
    public HealthBar healthBar;
    public float healthRegen = 0f;

    public float playerHealth = 100f;
    public float maxHealth = 100f;

    public float currency = 0f;
    [SerializeField] ParticleSystem _healingParticles;

    public bool isDead { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public List<string> deathEffect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string deathExplosion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        playerMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        abilityHolder = GetComponent<AbilityHolder>();
    }
    void Update()
    {

        RegenHealth();
    }

    void FixedUpdate()
    {
        PickUpLogic();
    }


    void RegenHealth()
    {
        if (playerHealth < maxHealth && playerHealth > 0)
        {
            _healingParticles.Play();
            playerHealth += healthRegen * Time.deltaTime;
        }
        else _healingParticles.Stop();
    }

    void PickUpLogic()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickUpRadius, LayerMask.GetMask("Debris"));

        // Iterate over each collider and trigger attraction
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Debris"))
            {
                hit.GetComponent<Debris>().isAttracted = true;
            }
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        StartCoroutine(FlashRed());

        if (playerHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(deathEffect, 1f);
        EventManager.GameOverEvent();
        Destroy(gameObject);
    }

    public void AddCurrency(float currency)
    {
        this.currency += currency;
        OnCurrencyChange?.Invoke();
    }
    public void RemoveCurrency(float currency)
    {
        this.currency -= currency;
        OnCurrencyChange?.Invoke();
    }

    public static PlayerManagerLegacy Player()
    {
        return Instance;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(other.gameObject.GetComponent<Bullet>().BulletDamage);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Nuke"))
        {
            TakeDamage(1000);
        }

    }

    public IEnumerator HandleDeath()
    {
        throw new NotImplementedException();
    }

    public IEnumerator DeathAnimation()
    {
        throw new NotImplementedException();
    }
}
