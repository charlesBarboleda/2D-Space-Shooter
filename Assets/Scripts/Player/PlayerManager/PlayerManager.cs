using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using DG.Tweening;

[DefaultExecutionOrder(-99)]
public class PlayerManager : MonoBehaviour, ITargetable
{
    public static PlayerManager Instance { get; private set; }

    PrestigeManager _prestigeManager;
    SpriteRenderer _spriteRenderer;
    PlayerHealthBehaviour _health;
    PlayerCurrencyBehaviour _currency;
    PlayerMovementBehaviour _movement;
    PlayerComboManager _combo;
    AudioSource _audioSource;
    AbilityHolder _abilityHolder;
    PickUpBehaviour _pickUpBehaviour;
    PowerUpBehaviour _powerUpBehaviour;

    Faction _faction;
    Weapon _weapon;
    public ParticleSystem arrowEmission;

    [SerializeField] GameObject Buff;
    [SerializeField] AudioClip _onDebrisAudio;
    [SerializeField] AudioClip _onPowerUpAudio;


    private void Awake()
    {
        SetSingleton();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _prestigeManager.PrestigeToSunlancer();
        }
    }

    void Start()
    {
        _prestigeManager = GetComponent<PrestigeManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _faction = GetComponent<Faction>();
        _combo = GetComponent<PlayerComboManager>();
        _audioSource = GetComponent<AudioSource>();
        _powerUpBehaviour = GetComponent<PowerUpBehaviour>();
        _pickUpBehaviour = GetComponent<PickUpBehaviour>();
        _abilityHolder = GetComponent<AbilityHolder>();
        _weapon = GetComponent<Weapon>();
        _movement = GetComponent<PlayerMovementBehaviour>();
        _health = GetComponent<PlayerHealthBehaviour>();
        _currency = GetComponent<PlayerCurrencyBehaviour>();
        ApplyPermanentUpgrades();
    }

    void ApplyPermanentUpgrades()
    {
        _health.currentHealth = PlayerPrefs.GetFloat("Health", 100);
        _health.maxHealth = PlayerPrefs.GetFloat("Health", 100);
        _movement.moveSpeed = PlayerPrefs.GetFloat("Speed", 15);
        _pickUpBehaviour.PickUpRadius = PlayerPrefs.GetFloat("PickUpRadius", 5);
        _weapon.bulletDamage = PlayerPrefs.GetFloat("BulletDamage", 50);
        _weapon.bulletSpeed = PlayerPrefs.GetFloat("BulletSpeed", 30);
        _weapon.fireRate = PlayerPrefs.GetFloat("FireRate", 1);
        _weapon.bulletLifetime = PlayerPrefs.GetFloat("BulletLifetime", 3);



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

    public void ActivateBuffAnimations()
    {
        Buff.SetActive(true);
    }

    public void DeactivateBuffAnimations()
    {
        var emission = arrowEmission.emission;
        emission.rateOverTime = 5;
        Buff.SetActive(false);
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
    public PlayerHealthBehaviour Health() => _health;
    public PlayerComboManager ComboManager() => _combo;
    public PickUpBehaviour PickUpBehaviour() => _pickUpBehaviour;
    public PowerUpBehaviour PowerUpBehaviour() => _powerUpBehaviour;
    public PrestigeManager PrestigeManager() => _prestigeManager;
    public PlayerMovementBehaviour Movement() => _movement;

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


