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
    public PrestigeType chosenPrestige = PrestigeType.None;
    public ParticleSystem arrowEmission;
    [SerializeField] Sprite _plaguebringer;
    [SerializeField] GameObject plaguebringerBuff;
    [SerializeField] GameObject Buff;
    [SerializeField] AudioClip _onDebrisAudio;
    [SerializeField] AudioClip _onPowerUpAudio;


    private void Awake()
    {
        SetSingleton();
    }

    void OnEnable()
    {
        EventManager.OnPrestigeChange += ApplyPrestigeEffects;
    }

    void OnDisable()
    {
        EventManager.OnPrestigeChange -= ApplyPrestigeEffects;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PrestigeToPlaguebringer();
            Debug.Log("Prestiged to Plaguebringer");
        }
    }

    void Start()
    {
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
    public void PrestigeTo(PrestigeType prestige)
    {
        chosenPrestige = prestige;
        EventManager.PrestigeChangeEvent(prestige);
    }
    public void PrestigeToPlaguebringer()
    {
        StartCoroutine(PrestigeAnimation("PlaguebringerPrestigeAnimation", 1.3f));
    }

    IEnumerator PrestigeAnimation(string animationName, float animationDuration)
    {
        GameObject prestigeAnimation = ObjectPooler.Instance.SpawnFromPool(animationName, transform.position, Quaternion.identity);
        prestigeAnimation.transform.SetParent(transform);
        yield return new WaitForSeconds(2f);
        UIManager.Instance.PrestigeCrackAndShatter();
        yield return new WaitForSeconds(animationDuration);
        prestigeAnimation.SetActive(false);
        PrestigeTo(PrestigeType.Plaguebringer);
    }

    void ApplyPrestigeEffects(PrestigeType prestige)
    {
        switch (prestige)
        {
            case PrestigeType.None:
                break;
            case PrestigeType.Plaguebringer:
                plaguebringerBuff.SetActive(true);
                _spriteRenderer.sprite = _plaguebringer;
                _spriteRenderer.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                _weapon.weaponType = WeaponType.PlayerCorrodeBullet;
                _weapon.amountOfBullets = 1;
                _weapon.bulletLifetime += 5f;
                _weapon.bulletSpeed -= 10f;
                _weapon.fireRate = 3f;
                _pickUpBehaviour.PickUpRadius += 10f;
                _movement.SetMoveSpeed(-5f);
                _health.SetMaxHealth(250f);
                _health.SetCurrentHealth(250f);
                StartCoroutine(PlayerCorrosion());
                break;
        }
    }
    IEnumerator PlayerCorrosion()
    {
        while (chosenPrestige == PrestigeType.Plaguebringer)
        {
            GameObject corrode = ObjectPooler.Instance.SpawnFromPool("CorrodeEffect", transform.position, Quaternion.identity);
            corrode.GetComponent<CorrosionEffect>().ApplyCorrode(gameObject, _weapon.bulletDamage * 5);
            yield return new WaitForSeconds(5f);
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
    public PlayerComboManager ComboManager() => _combo;
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


