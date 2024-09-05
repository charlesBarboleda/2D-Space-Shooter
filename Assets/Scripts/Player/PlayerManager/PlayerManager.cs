using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    PlayerHealthBehaviour _health;
    PlayerCurrencyBehaviour _currency;
    PlayerMovementBehaviour _movement;
    AbilityHolder _abilityHolder;
    PickUpBehaviour _pickUpBehaviour;
    Weapon _weapon;

    private void Awake()
    {
        SetSingleton();
    }

    void Start()
    {
        _pickUpBehaviour = GetComponent<PickUpBehaviour>();
        _abilityHolder = GetComponent<AbilityHolder>();
        _weapon = GetComponent<Weapon>();
        _movement = GetComponent<PlayerMovementBehaviour>();
        _health = GetComponent<PlayerHealthBehaviour>();
        _currency = GetComponent<PlayerCurrencyBehaviour>();
    }

    void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Getters and Setters
    /// </summary>
    public static PlayerManager GetInstance() => Instance;
    public AbilityHolder AbilityHolder() => _abilityHolder;
    public Weapon Weapon() => _weapon;
    public PickUpBehaviour PickUpBehaviour() => _pickUpBehaviour;

    #region Movement Management
    public void SetMoveSpeed(float newMoveSpeed) => _movement.SetMoveSpeed(newMoveSpeed);
    public float MoveSpeed() => _movement.MoveSpeed();
    #endregion 

    #region Health Management
    public void SetMaxHealth(float newMaxHealth) => _health.SetMaxHealth(newMaxHealth);
    public float MaxHealth() => _health.maxHealth;

    public void SetCurrentHealth(float newCurrentHealth) => _health.SetCurrentHealth(newCurrentHealth);
    public float CurrentHealth() => _health.currentHealth;
    public void SetHealthRegenRate(float newHealthRegenRate) => _health.SetHealthRegenRate(newHealthRegenRate);
    public float HealthRegenRate() => _health.healthRegenRate;
    #endregion

    #region Currency Management
    public float Currency() => _currency.currency;
    public void SetCurrency(float amount) => _currency.SetCurrency(amount);
    #endregion

    #region PickUp Management
    public float PickUpRadius() => _pickUpBehaviour.PickUpRadius();
    public void SetPickUpRadius(float newPickUpRadius) => _pickUpBehaviour.SetPickUpRadius(newPickUpRadius);
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            _health.TakeDamage(other.gameObject.GetComponent<Bullet>().BulletDamage);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Nuke"))
        {
            _health.TakeDamage(1000);
        }

    }
}

