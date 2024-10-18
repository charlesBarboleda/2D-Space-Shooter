using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] float _currentHealth;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _currencyDrop;
    [SerializeField] List<string> _currencyPrefab;
    [SerializeField] List<GameObject> exhaustChildren;
    [SerializeField] List<GameObject> turretChildren;

    [SerializeField] AudioClip _deathSound;
    [SerializeField] string _deathExplosion;

    [SerializeField] List<string> _deathEffect;

    AttackManager _attackManager;
    SpriteRenderer _spriteRenderer;
    AudioSource _audioSource;
    Collider2D[] _colliders;
    Rigidbody2D _rigidbody;
    Faction _faction;
    public enum ShakeType { None, Small, Mid, Large }
    [SerializeField] ShakeType shakeType;
    public bool isDead { get; set; }


    void Awake()
    {
        _attackManager = GetComponent<AttackManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody != null) _rigidbody.gravityScale = 0;
        _faction = GetComponent<Faction>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _colliders = GetComponents<Collider2D>();
        _currentHealth = _maxHealth;
    }

    void OnEnable()
    {
        if (turretChildren.Count > 0) turretChildren.ForEach(child => child.SetActive(true));
        if (exhaustChildren.Count > 0) exhaustChildren.ForEach(child => child.SetActive(true));
        if (_colliders != null) _colliders.ForEach(collider => collider.enabled = true);
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        if (_rigidbody != null) _rigidbody.simulated = true;
        if (isDead) isDead = false;
        if (_attackManager != null) _attackManager.IsSilenced = false;
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        _currentHealth -= damage;
        if (_currentHealth > 0)
        {
            StartCoroutine(FlashRed());
        }
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    public virtual void Die()
    {
        // Hide the ship visually and start the death animation coroutine
        StartCoroutine(HandleDeath());
    }

    public IEnumerator HandleDeath()
    {

        isDead = true;
        // Play the death sound
        if (_audioSource != null) _audioSource.PlayOneShot(_deathSound);

        // Disable the turrets if there are any
        if (turretChildren.Count > 0) turretChildren.ForEach(child => child.SetActive(false));

        // Stops the exhaust particles
        if (exhaustChildren.Count > 0) exhaustChildren.ForEach(child => child.SetActive(false));

        // Disable all colliders
        if (_colliders != null)
            foreach (var collider in _colliders)
                collider.enabled = false;

        // Hide the ship's sprite
        if (_spriteRenderer != null) _spriteRenderer.enabled = false;

        // Remove the rigidbody physics calculations
        if (_rigidbody != null)
        {
            _rigidbody.simulated = false;
        }
        // Disable attacking and all the attacking particles
        if (_attackManager != null)
        {
            _attackManager.IsSilenced = true;
        }

        // Shake the camera
        switch (shakeType)
        {
            case ShakeType.None:
                break;
            case ShakeType.Small:
                CameraFollowBehaviour.Instance.ShakePlayerCamera(6f, 5.0f, 0.25f); ;
                break;
            case ShakeType.Mid:
                CameraFollowBehaviour.Instance.ShakePlayerCamera(10f, 5.0f, 1f);
                break;
            case ShakeType.Large:
                CameraFollowBehaviour.Instance.ShakePlayerCamera(16f, 5.0f, 2f);
                break;
        }

        // Notify Event Manager
        TryGetComponent(out Enemy enemy);
        if (enemy != null)
        {
            EventManager.EnemyShipDestroyedEvent(gameObject);
        }

        // Create the debris
        if (_currencyPrefab.Count > 0)
        {

            GameObject currency = ObjectPooler.Instance.SpawnFromPool(_currencyPrefab[Random.Range(0, _currencyPrefab.Count)], transform.position, transform.rotation);
            currency.GetComponent<Debris>().SetCurrency(_currencyDrop);

        }

        // Wait for the death animation to complete
        yield return StartCoroutine(DeathAnimation());

        // After the animation is done, deactivate the entire GameObject
        gameObject.SetActive(false);

    }
    public IEnumerator DeathAnimation()
    {
        GameObject exp = ObjectPooler.Instance.SpawnFromPool(_deathExplosion, transform.position, Quaternion.identity);
        GameObject exp2 = ObjectPooler.Instance.SpawnFromPool(_deathEffect[Random.Range(0, _deathEffect.Count)], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f); // Wait for the animation to finish
        exp.SetActive(false);
        exp2.SetActive(false);
    }

    /// <summary>
    /// Getters and Setters
    /// </summary>

    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float CurrencyDrop { get => _currencyDrop; set => _currencyDrop = value; }
    public Collider2D[] Colliders { get => _colliders; }
    public SpriteRenderer SpriteRenderer { get => _spriteRenderer; }
    public Rigidbody2D Rigidbody { get => _rigidbody; }
    public List<GameObject> ExhaustChildren { get => exhaustChildren; }
    public List<string> CurrencyPrefab { get => _currencyPrefab; }
    public string deathExplosion { get => _deathExplosion; set => _deathExplosion = value; }
    public List<string> deathEffect { get => _deathEffect; set => _deathEffect = value; }

}
