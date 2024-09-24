using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyShield : MonoBehaviour, IDamageable
{
    Transform _shieldOwner;
    [SerializeField] float _maxShield = 100f;
    [SerializeField] float _currentShield;
    [SerializeField] float _shieldRegenRate = 100f;
    [SerializeField] float _size;
    SpriteRenderer spriteRenderer;
    Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

    }
    void OnEnable()
    {
        Debug.Log($"Shield owner: {_shieldOwner.name}, Max Shield: {_maxShield}, Shield Regen: {_shieldRegenRate}");
    }
    void Update()
    {
        RegenShield();
        // Update size from zero to max size
        transform.localScale = new Vector3(_size, _size, 1);


    }

    public void TakeDamage(float damage)
    {
        _currentShield -= damage;
        StartCoroutine(FlashRed());
        if (_currentShield < 0)
        {
            _currentShield = 0;
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    public void RegenShield()
    {
        _currentShield += _shieldRegenRate * Time.deltaTime;
        if (_currentShield > _maxShield)
        {
            _currentShield = _maxShield;
        }
    }

    public void ResetStats()
    {
        _currentShield = _maxShield;
        transform.localScale = new Vector3(_size, _size, 1);
    }

    public void Die()
    {
        StartCoroutine(HandleDeath());
    }

    public IEnumerator HandleDeath()
    {

        yield return DeathAnimation();
        gameObject.SetActive(false);
    }

    public IEnumerator DeathAnimation()
    {
        yield return null;
    }

    public float CurrentShield { get => _currentShield; set => _currentShield = value; }
    public float MaxShield { get => _maxShield; set => _maxShield = value; }
    public float ShieldRegenRate { get => _shieldRegenRate; set => _shieldRegenRate = value; }
    public float Size { get => _size; set => _size = value; }
    public bool isDead { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public List<string> deathEffect { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string deathExplosion { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Transform ShieldOwner { get => _shieldOwner; set => _shieldOwner = value; }
}
