using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class PlayerManager : MonoBehaviour, ITargetable
{
    public static PlayerManager Instance { get; private set; }

    PlayerHealthBehaviour _health;
    PlayerCurrencyBehaviour _currency;
    PlayerMovementBehaviour _movement;
    AudioSource _audioSource;
    AbilityHolder _abilityHolder;
    PickUpBehaviour _pickUpBehaviour;
    PowerUpBehaviour _powerUpBehaviour;
    Faction _faction;
    Weapon _weapon;
    [SerializeField] AudioClip _onDebrisAudio;
    [SerializeField] AudioClip _onPowerUpAudio;


    private void Awake()
    {
        SetSingleton();
    }

    void Start()
    {
        _faction = GetComponent<Faction>();
        _audioSource = GetComponent<AudioSource>();
        _powerUpBehaviour = GetComponent<PowerUpBehaviour>();
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Debris") || other.CompareTag("PowerUp"))
        {
            IPickable iPickable = other.GetComponent<IPickable>();
            if (iPickable != null)
            {
                iPickable.OnPickUp();
                if (other.CompareTag("Debris"))
                {
                    _audioSource.PlayOneShot(_onDebrisAudio);
                }
                else if (other.CompareTag("PowerUp"))
                {
                    _audioSource.PlayOneShot(_onPowerUpAudio);
                    _powerUpBehaviour.AddPowerUp(other.GetComponent<PowerUp>());
                }
            }
        }
    }

    /// <summary>
    /// Getters and Setters
    /// </summary>
    public static PlayerManager GetInstance() => Instance;
    public AbilityHolder AbilityHolder() => _abilityHolder;
    public Weapon Weapon() => _weapon;
    public PickUpBehaviour PickUpBehaviour() => _pickUpBehaviour;
    public PowerUpBehaviour PowerUpBehaviour() => _powerUpBehaviour;

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

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool IsAlive()
    {
        return !_health.isDead;
    }

    public FactionType GetFactionType()
    {
        return _faction.factionType;
    }
    #endregion

    #region PickUp Management
    public float PickUpRadius { get => _pickUpBehaviour.PickUpRadius; set => _pickUpBehaviour.PickUpRadius = value; }
    #endregion

}


