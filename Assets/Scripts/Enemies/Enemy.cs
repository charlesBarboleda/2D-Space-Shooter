using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Faction))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] List<GameObject> _currencyPrefab;
    // ETC
    float _targetSwitchCooldown = 30f;
    Transform _currentTarget;
    float _lastTargetSwitchTime = 0f;

    // Animations & References

    Faction _faction;
    AudioSource _audioSource;
    [SerializeField] AudioClip _abilitySound;
    [SerializeField] AudioClip _spawnSound;
    [SerializeField] string _spawnAnimation;
    [SerializeField] string _deathExplosion;
    public string deathExplosion { get => _deathExplosion; set => _deathExplosion = value; }
    [SerializeField] List<string> _deathEffect;
    public List<string> deathEffect { get => _deathEffect; set => _deathEffect = value; }
    AbilityHolder _abilityHolder;
    SpriteRenderer _spriteRenderer;
    List<BoxCollider2D> _colliders = new List<BoxCollider2D>();
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
    public abstract void Attack();

    protected virtual void Start()
    {
        _faction = GetComponent<Faction>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colliders.AddRange(GetComponents<BoxCollider2D>());
        isDead = false;


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
            UseAbility(CheckForTargets()); // Uses the ability if the cooldown is 0
            if (_abilitySound != null) _audioSource.PlayOneShot(_abilitySound);
        }


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

    }

    protected virtual void Movement(Transform target)
    {
        Collider2D[] targetColliders = target.GetComponents<Collider2D>();
        if (targetColliders.Length > 0)
        {
            Vector3 closestPoint = GetClosestPoint(targetColliders, transform.position);
            float distance = Vector3.Distance(closestPoint, transform.position);

            if (distance > _stopDistance)
            {
                Vector3 direction = (closestPoint - transform.position).normalized;
                transform.position += direction * _speed * Time.deltaTime;
            }
            else
            {
                OrbitAround(target);
            }
        }
    }

    void OrbitAround(Transform target)
    {
        float direction = _rotateClockwise ? 1 : -1;
        transform.RotateAround(target.position, Vector3.forward, direction * _speed * Time.deltaTime);
    }

    protected virtual void Aim(Transform target)
    {
        Collider2D[] targetColliders = target.GetComponents<Collider2D>();
        if (targetColliders.Length > 0)
        {
            Vector3 closestPoint = GetClosestPoint(targetColliders, transform.position);
            Vector3 direction = closestPoint - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimOffset));
        }
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

    protected virtual Transform CheckForTargets()
    {
        // Return current target if it is valid and within the cooldown period
        if (_currentTarget != null && Time.time < _lastTargetSwitchTime + _targetSwitchCooldown)
        {

            return _currentTarget;
        }

        float detectionRadius = 50f; // Increased radius for testing
        LayerMask enemyLayerMask = LayerMask.GetMask("Syndicates") | LayerMask.GetMask("ThraxArmada") | LayerMask.GetMask("CrimsonFleet") | LayerMask.GetMask("Player");

        // Get all potential targets within the detection radius
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayerMask);

        Transform bestTarget = null;

        // Iterate through detected targets to find a valid one
        foreach (Collider2D targetCollider in hitTargets)
        {
            Faction targetFaction = targetCollider.GetComponent<Faction>();

            if (targetFaction != null)
            {


                // Check if the target's faction is different from the enemy's faction
                if (targetFaction.factionType != _faction.factionType)
                {
                    Debug.Log("Target Faction:" + targetFaction.factionType);
                    // Check if the target has a valid tag
                    if (targetCollider.CompareTag("EnemyDestroyable") ||
                        targetCollider.CompareTag("ThraxArmada") ||
                        targetCollider.CompareTag("CrimsonFleet") ||
                        targetCollider.CompareTag("Syndicates"))
                    {
                        bestTarget = targetCollider.transform;
                        Debug.Log("Best Target:" + bestTarget.name);
                        break; // Found a valid target, no need to continue checking
                    }

                }

            }
        }
        if (bestTarget != null)
        {
            _currentTarget = bestTarget;
            _lastTargetSwitchTime = Time.time;
        }
        else
        {
            _currentTarget = PlayerManager.Instance.transform;
        }

        return _currentTarget;

    }


    IEnumerator SpawnAnimation()
    {
        if (_spawnSound != null) _audioSource.PlayOneShot(_spawnSound);
        GameObject obj = ObjectPooler.Instance.SpawnFromPool(_spawnAnimation, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
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

    void UseAbility(Transform target)
    {
        foreach (Ability ability in _abilityHolder.abilities)
        {
            if (ability.cooldown > 0) ability.TriggerAbility(gameObject, target);
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
        _health += GameManager.Instance.Level() * 10f;

        _currencyDrop += GameManager.Instance.Level() * 0.5f;

        _speed += GameManager.Instance.Level() * 0.05f;

        transform.localScale += new Vector3(GameManager.Instance.Level() * 0.02f, GameManager.Instance.Level() * 0.02f, GameManager.Instance.Level() * 0.02f);

    }

    public virtual void Die()
    {
        // Hide the ship visually and start the death animation coroutine
        StartCoroutine(HandleDeath());
    }

    public virtual IEnumerator HandleDeath()
    {

        isDead = true;

        // Disable the turrets if there are any
        if (turretChildren.Count > 0) turretChildren.ForEach(child => child.SetActive(false));

        // Stops the exhaust particles
        exhaustChildren.ForEach(child => child.SetActive(false));

        // Disable all colliders
        _colliders.ForEach(collider => collider.enabled = false);

        // Hide the ship's sprite
        _spriteRenderer.enabled = false;

        // Shake the camera
        CameraShake.Instance.TriggerShake(_cameraShakeMagnitude, _cameraShakeDuration);

        // Notify Objectives Manager
        ObjectivesManager.Instance.DestroyShip();

        // Notify Event Manager
        EventManager.EnemyDestroyedEvent(gameObject);

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
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
    public Faction Faction { get => _faction; set => _faction = value; }
    public bool isDead { get => _isDead; set => _isDead = value; }
    public float CurrencyDrop { get => _currencyDrop; set => _currencyDrop = value; }
    public float Health { get => _health; set => _health = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float StopDistance { get => _stopDistance; set => _stopDistance = value; }


}
