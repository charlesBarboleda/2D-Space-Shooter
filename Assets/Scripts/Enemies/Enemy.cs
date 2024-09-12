using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Faction))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CompositeCollider2D))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] List<GameObject> _currencyPrefab;
    // ETC
    float _checkForTargetsInterval = 0.5f;
    float _movementInterval = 0.1f;
    float _abilityInterval = 1f;
    Coroutine _checkForTargetsCoroutine;
    Coroutine _movementCoroutine;
    Coroutine _abilityCoroutine;
    Transform _currentTarget;
    Vector3 _cachedDirection;
    float _cachedDistance;


    // Animations & References

    Faction _faction;
    AudioSource _audioSource;
    [SerializeField] GameObject _buffedParticles;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] AudioClip _abilitySound;
    [SerializeField] AudioClip _spawnSound;
    [SerializeField] string _spawnAnimation;
    [SerializeField] string _deathExplosion;
    public string deathExplosion { get => _deathExplosion; set => _deathExplosion = value; }
    [SerializeField] List<string> _deathEffect;
    public List<string> deathEffect { get => _deathEffect; set => _deathEffect = value; }
    AbilityHolder _abilityHolder;
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidbody2D;
    List<Collider2D> _colliders = new List<Collider2D>();
    // Stats
    [SerializeField] float _aimOffset;
    [SerializeField] bool _shouldRotate;
    [SerializeField] float _health;
    [SerializeField] float _currencyDrop;
    [SerializeField] float _speed;
    [SerializeField] float _stopDistance;
    [SerializeField] bool _isDead;


    bool _rotateClockwise = false;
    public List<GameObject> exhaustChildren = new List<GameObject>();
    public List<GameObject> turretChildren = new List<GameObject>();



    // Camera Shake
    [SerializeField] float _cameraShakeMagnitude;
    [SerializeField] float _cameraShakeDuration;
    protected abstract void Attack();

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _abilityHolder = GetComponent<AbilityHolder>();
        _faction = GetComponent<Faction>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colliders.AddRange(GetComponents<Collider2D>());
        isDead = false;
        _faction.AddAllyFaction(_faction.factionType);
        _rigidbody2D.simulated = true;

        foreach (Collider2D collider in _colliders)
        {
            collider.usedByComposite = true;
        }


    }

    protected virtual void Update()
    {
        if (isDead) return;
        if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
        {
            _currentTarget = CheckForTargets();
        }
        if (_shouldRotate) Aim(_currentTarget);
        Movement(_currentTarget);

        if (_abilityHolder != null)
        {
            UseAbilities(CheckForTargets()); // Uses the ability if the cooldown is 0
            if (_abilitySound != null) _audioSource.PlayOneShot(_abilitySound);
        }


    }
    public virtual void UnBuffedState()
    {
        _buffedParticles.SetActive(false);
        Health = Health / 1.5f;
        Speed = Speed / 1.5f;
    }
    public virtual void BuffedState()
    {
        _buffedParticles.SetActive(true);
        Health = Health * 1.5f;
        Speed = Speed * 1.5f;

    }

    protected virtual void OnEnable()
    {

        if (turretChildren.Count > 0) turretChildren.ForEach(child => child.SetActive(true));
        if (exhaustChildren.Count > 0) exhaustChildren.ForEach(child => child.SetActive(true));
        if (_colliders.Count > 0) _colliders.ForEach(collider => collider.enabled = true);
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        if (isDead) isDead = false;

        IncreaseStatsPerLevel();
        StartCoroutine(StartSpawnAnimationWithDelay());


        // Randomly choose rotation direction
        if (Random.value < 0.5) _rotateClockwise = true;
        else _rotateClockwise = false;

        _checkForTargetsCoroutine = StartCoroutine(CheckForTargetsRoutine());
        _movementCoroutine = StartCoroutine(MovementRoutine());
        _abilityCoroutine = StartCoroutine(AbilityRoutine());

    }

    protected virtual void OnDisable()
    {
        if (_checkForTargetsCoroutine != null) StopCoroutine(_checkForTargetsCoroutine);
        if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
        if (_abilityCoroutine != null) StopCoroutine(_abilityCoroutine);
    }

    IEnumerator CheckForTargetsRoutine()
    {
        while (!isDead)
        {
            if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
                _currentTarget = CheckForTargets();

            yield return new WaitForSeconds(_checkForTargetsInterval);
        }
    }

    IEnumerator MovementRoutine()
    {
        while (!isDead)
        {
            Movement(_currentTarget);
            yield return new WaitForSeconds(_movementInterval);
        }
    }

    IEnumerator AbilityRoutine()
    {
        while (!isDead && _abilityHolder != null)
        {
            UseAbilities(_currentTarget);
            yield return new WaitForSeconds(_abilityInterval);
        }
    }

    protected virtual void Movement(Transform target)
    {
        if (target == null) return;

        _cachedDirection = (target.position - transform.position).normalized;
        _cachedDistance = Vector3.Distance(transform.position, target.position);

        if (_cachedDistance > _stopDistance)
        {
            transform.position += _cachedDirection * _speed * Time.deltaTime;
        }
        else
        {
            Orbit(target);
        }
    }

    private void Orbit(Transform target)
    {
        float rotationDirection = _rotateClockwise ? 1 : -1;
        transform.RotateAround(target.position, Vector3.forward, rotationDirection * _speed * Time.deltaTime);
    }

    protected virtual void Aim(Transform target)
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimOffset));
    }

    protected virtual Transform CheckForTargets()
    {
        float detectionRadius = 50f;
        LayerMask targetLayerMask = LayerMask.GetMask("Syndicates", "ThraxArmada", "CrimsonFleet", "Player");
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayerMask);

        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D targetCollider in hitTargets)
        {
            Faction targetFaction = targetCollider.GetComponent<Faction>();
            if (targetFaction != null && _faction.IsHostileTo(targetFaction.factionType))
            {
                float distance = Vector3.Distance(transform.position, targetCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTarget = targetCollider.transform;
                    Debug.Log("Best Target:" + bestTarget.name + "Faction: " + targetFaction.factionType);
                }
            }
        }

        return bestTarget ?? PlayerManager.Instance.transform;  // Fallback to player if no other targets
    }
    protected Vector2 GetClosestPoint(Collider2D[] colliders, Vector3 fromPosition)
    {
        Vector3 closestPoint = Vector3.zero;
        float minDistance = Mathf.Infinity;

        foreach (var collider in colliders)
        {
            Vector3 point = collider.ClosestPoint(fromPosition);
            float distance = Vector3.Distance(fromPosition, point);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    IEnumerator SpawnAnimation()
    {
        if (_spawnSound != null) _audioSource.PlayOneShot(_spawnSound);
        GameObject obj = ObjectPooler.Instance.SpawnFromPool(_spawnAnimation, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        obj.SetActive(false);
    }
    IEnumerator StartSpawnAnimationWithDelay()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SpawnAnimation());
    }

    IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    void UseAbilities(Transform target)
    {
        foreach (Ability ability in _abilityHolder.abilities)
        {
            if (ability.currentCooldown >= ability.cooldown) ability.TriggerAbility(gameObject, target);
        }
    }


    public void TakeDamage(float damage)
    {
        if (isDead) return;
        _health -= damage;
        if (_health > 0)
        {
            StartCoroutine(FlashRed());
        }
        if (_health <= 0)
        {
            Die();
        }
    }



    public virtual void IncreaseStatsPerLevel()
    {
        Health += GameManager.Instance.Level * 5f;

        CurrencyDrop += GameManager.Instance.Level * 0.5f;

        Speed += GameManager.Instance.Level * 0.05f;

        transform.localScale += new Vector3(GameManager.Instance.Level * 0.01f, GameManager.Instance.Level * 0.01f, GameManager.Instance.Level * 0.01f);

    }

    public virtual void Die()
    {
        // Hide the ship visually and start the death animation coroutine
        StartCoroutine(HandleDeath());
    }

    public virtual IEnumerator HandleDeath()
    {

        isDead = true;
        // Play the death sound
        if (_audioSource != null) _audioSource.PlayOneShot(_deathSound);

        // Disable the turrets if there are any
        if (turretChildren.Count > 0) turretChildren.ForEach(child => child.SetActive(false));

        // Stops the exhaust particles
        if (exhaustChildren.Count > 0) exhaustChildren.ForEach(child => child.SetActive(false));

        // Disable all colliders
        if (_colliders.Count > 0) _colliders.ForEach(collider => collider.enabled = false);

        // Hide the ship's sprite
        if (_spriteRenderer != null) _spriteRenderer.enabled = false;

        // Shake the camera
        CameraShake.Instance.TriggerShake(_cameraShakeMagnitude, _cameraShakeDuration);

        // Notify Event Manager
        EventManager.EnemyDestroyedEvent(gameObject, _faction);

        // Create the debris
        GameObject currency = Instantiate(_currencyPrefab[Random.Range(0, _currencyPrefab.Count)], transform.position, transform.rotation);
        currency.GetComponent<Debris>().SetCurrency(_currencyDrop);

        // Wait for the death animation to complete
        yield return StartCoroutine(DeathAnimation());

        // After the animation is done, deactivate the entire GameObject
        gameObject.SetActive(false);

    }

    public IEnumerator DeathAnimation()
    {
        GameObject exp2 = ObjectPooler.Instance.SpawnFromPool(deathEffect[Random.Range(0, deathEffect.Count)], transform.position, Quaternion.identity);
        GameObject exp = ObjectPooler.Instance.SpawnFromPool(deathExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f); // Wait for the animation to finish
        exp.SetActive(false);
        exp2.SetActive(false);
    }


    /// <summary>
    /// Getters and Setters
    /// </summary>
    /// 
    public List<Collider2D> Colliders { get => _colliders; set => _colliders = value; }
    public SpriteRenderer SpriteRenderer { get => _spriteRenderer; set => _spriteRenderer = value; }
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
    public Faction Faction { get => _faction; set => _faction = value; }
    public List<GameObject> CurrencyPrefab { get => _currencyPrefab; set => _currencyPrefab = value; }
    public float CameraShakeMagnitude { get => _cameraShakeMagnitude; set => _cameraShakeMagnitude = value; }
    public float CameraShakeDuration { get => _cameraShakeDuration; set => _cameraShakeDuration = value; }
    public bool isDead { get => _isDead; set => _isDead = value; }
    public float CurrencyDrop { get => _currencyDrop; set => _currencyDrop = value; }
    public float Health { get => _health; set => _health = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float StopDistance { get => _stopDistance; set => _stopDistance = value; }


}
