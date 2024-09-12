using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Faction))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] List<GameObject> _currencyPrefab;
    // ETC
    float _checkForTargetsInterval = 5f;
    Coroutine _checkForTargetsCoroutine;
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
    BoxCollider2D[] _boxColliders;
    // Stats
    [SerializeField] float _aimOffset;
    [SerializeField] bool _shouldRotate;
    [SerializeField] float _health;
    [SerializeField] float _currencyDrop;
    [SerializeField] float _speed;
    [SerializeField] float _stopDistance;
    [SerializeField] bool _isDead;
    [SerializeField] float _aimRange;

    bool _rotateClockwise = false;
    public List<GameObject> exhaustChildren = new List<GameObject>();
    public List<GameObject> turretChildren = new List<GameObject>();
    [SerializeField] float separationRadius = 2f;  // Radius for separation behavior
    [SerializeField] float separationWeight = 1.5f;   // Strength of the repulsion force



    // Camera Shake
    [SerializeField] float _cameraShakeMagnitude;
    [SerializeField] float _cameraShakeDuration;
    protected abstract void Attack();

    protected virtual void Awake()
    {

        _abilityHolder = GetComponent<AbilityHolder>();
        _faction = GetComponent<Faction>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        isDead = false;
        _boxColliders = GetComponents<BoxCollider2D>();
        _faction.AddAllyFaction(_faction.factionType);


    }

    protected virtual void Update()
    {
        if (isDead) return;
        if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy) _currentTarget = CheckForTargets();
        if (_shouldRotate) Aim(_currentTarget);
        Movement(_currentTarget);

        if (_abilityHolder != null)
        {
            UseAbilities(_currentTarget); // Uses the ability if the cooldown is 0
            if (_abilitySound != null) _audioSource.PlayOneShot(_abilitySound);
        }
        if (CurrentTarget != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, CurrentTarget.position);
            if (distanceToTarget < AimRange)
            {
                // Check if the target is the one we should shoot at
                if (IsTargetInRange(CurrentTarget))
                {
                    Attack();
                }
            }
        }


    }

    protected virtual Vector3 CalculateSeparation()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Syndicates", "ThraxArmada", "CrimsonFleet");
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius, enemyLayer);
        Vector3 separationForce = Vector3.zero;
        int count = 0;

        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy != null && enemy.transform != transform)
            {
                Vector3 directionToEnemy = transform.position - enemy.transform.position;
                separationForce += directionToEnemy.normalized / directionToEnemy.magnitude; // Inverse weighted by distance
                count++;
            }
        }

        if (count > 0)
        {
            separationForce /= count; // Average the forces from nearby enemies
            separationForce = separationForce.normalized * separationWeight; // Normalize and apply weight
        }

        return separationForce;
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

    public bool IsTargetInRange(Transform target)
    {
        // Calculate edge distance
        if (target.TryGetComponent<Collider2D>(out Collider2D targetCollider))
        {
            Vector2 closestPointToTarget = targetCollider.ClosestPoint(transform.position);
            float distanceToTargetEdge = Vector2.Distance(transform.position, closestPointToTarget);
            return distanceToTargetEdge < _aimRange;
        }
        return false;
    }


    protected virtual void OnEnable()
    {

        if (turretChildren.Count > 0) turretChildren.ForEach(child => child.SetActive(true));
        if (exhaustChildren.Count > 0) exhaustChildren.ForEach(child => child.SetActive(true));
        if (_boxColliders != null) _boxColliders.ForEach(collider => collider.enabled = true);
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        if (isDead) isDead = false;

        IncreaseStatsPerLevel();
        StartCoroutine(StartSpawnAnimationWithDelay());


        // Randomly choose rotation direction
        if (Random.value < 0.5) _rotateClockwise = true;
        else _rotateClockwise = false;

        _checkForTargetsCoroutine = StartCoroutine(CheckForTargetsRoutine());

    }

    protected virtual void OnDisable()
    {
        if (_checkForTargetsCoroutine != null) StopCoroutine(_checkForTargetsCoroutine);
    }

    IEnumerator CheckForTargetsRoutine()
    {
        while (!isDead)
        {
            _currentTarget = CheckForTargets();
            yield return new WaitForSeconds(_checkForTargetsInterval);
        }
    }




    protected virtual void Movement(Transform target)
    {
        if (target == null) return;

        // Get the target's closest point using colliders
        Collider2D targetCollider = target.GetComponent<Collider2D>();
        if (targetCollider != null)
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
            _cachedDirection = (closestPoint - transform.position).normalized;
            _cachedDistance = Vector3.Distance(transform.position, closestPoint);
        }
        else
        {
            // Fallback if no collider is found
            _cachedDirection = (target.position - transform.position).normalized;
            _cachedDistance = Vector3.Distance(transform.position, target.position);
        }

        // Get separation force to avoid collisions with other enemies
        Vector3 separationForce = CalculateSeparation();

        if (_cachedDistance > _stopDistance)
        {
            Vector3 finalDirection = (_cachedDirection + separationForce).normalized;
            transform.position += finalDirection * _speed * Time.deltaTime;
        }
        else
        {
            Orbit(target);  // Orbit when close enough
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

        // Get the target's closest point using colliders
        Collider2D targetCollider = target.GetComponent<Collider2D>();
        if (targetCollider != null)
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
            Vector3 direction = closestPoint - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimOffset));
        }
        else
        {
            // Fallback if no collider is found
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimOffset));
        }
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
            Debug.Log("Target: " + targetCollider.name);
            Faction targetFaction = targetCollider.GetComponent<Faction>();
            if (targetFaction != null && _faction.IsHostileTo(targetFaction.factionType))
            {
                Debug.Log("Target Faction: " + targetFaction.factionType);
                float distance = Vector3.Distance(transform.position, targetCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTarget = targetCollider.transform;
                    Debug.Log("Best Target: " + targetCollider.name);

                }
            }
        }


        return bestTarget ?? PlayerManager.Instance.transform;  // Fallback to player if no other targets
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
        if (_boxColliders != null) _boxColliders.ForEach(collider => collider.enabled = false);

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
    public Transform CurrentTarget { get => _currentTarget; set => _currentTarget = value; }
    public BoxCollider2D[] BoxColliders { get => _boxColliders; set => _boxColliders = value; }
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
    public float AimRange { get => _aimRange; set => _aimRange = value; }


}
