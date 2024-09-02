using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] List<GameObject> _currencyPrefab;

    // Animations & References
    public AudioSource audioSource;
    [SerializeField] AudioClip _abilitySound;
    [SerializeField] AudioClip _spawnSound;
    [SerializeField] string _spawnAnimation;
    [SerializeField] string _deathExplosion;
    [SerializeField]
    AbilityHolder _abilityHolder;
    SpriteRenderer _spriteRenderer;
    List<BoxCollider2D> _colliders = new List<BoxCollider2D>();
    // Stats
    [SerializeField] bool _shouldRotate;
    [SerializeField] float _health;
    [SerializeField] float _currencyDrop;
    [SerializeField] float _speed;
    [SerializeField] float _stopDistance;
    public bool isDead;
    bool _rotateClockwise = false;
    List<GameObject> exhaustChildren = new List<GameObject>();


    // Camera Shake
    [SerializeField] float _cameraShakeMagnitude;
    [SerializeField] float _cameraShakeDuration;
    public abstract void Attack();

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colliders.AddRange(GetComponents<BoxCollider2D>());
        exhaustChildren.AddRange(GameObject.FindGameObjectsWithTag("Exhaust"));


    }

    public virtual void Update()
    {

        if (_shouldRotate) Aim(CheckForTargets());
        Movement(CheckForTargets());

        if (_abilityHolder != null)
        {
            UseAbility(CheckForTargets()); // Uses the ability if the cooldown is 0
            if (_abilitySound != null) audioSource.PlayOneShot(_abilitySound);
        }

    }

    public virtual void OnEnable()
    {
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

    public virtual void OnDisable()
    {
        if (!isDead)
        {
            GameManager.Instance.RemoveEnemy(gameObject);
        }
    }

    public virtual void Movement(Transform target)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > _stopDistance)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;
        }
        else
        {
            OrbitAround(target);
        }
    }

    private void OrbitAround(Transform target)
    {
        float direction = _rotateClockwise ? 1 : -1;
        transform.RotateAround(target.position, Vector3.forward, direction * _speed * Time.deltaTime);
    }

    public void Aim(Transform target)
    {
        Vector3 targetAim = target.transform.position;
        Vector3 direction = targetAim - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270f));
    }

    IEnumerator SpawnAnimation()
    {
        audioSource.PlayOneShot(_spawnSound);
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

        _health -= damage;
        if (_health > 0)
        {
            Debug.Log("Enemy Health: " + _health);
            StartCoroutine(FlashRed());
        }
        if (_health <= 0)
        {
            Destroy();
        }
    }

    public virtual Transform CheckForTargets()
    {
        // Check for enemies using circle raycast
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, 200f, LayerMask.GetMask("Player"));
        foreach (Collider2D targets in hitTargets)
        {
            if (targets.CompareTag("CargoShip") || targets.CompareTag("VIPBuilding"))
            {
                return targets.transform;
            }
        }
        return PlayerManager.GetPlayer().transform;
    }




    public virtual void IncreaseStatsPerLevel()
    {
        _health += GameManager.Instance.Level() * 10f;

        _currencyDrop += GameManager.Instance.Level() * 0.5f;

        _speed += GameManager.Instance.Level() * 0.05f;

        transform.localScale += new Vector3(GameManager.Instance.Level() * 0.01f, GameManager.Instance.Level() * 0.01f, GameManager.Instance.Level() * 0.01f);

    }

    public virtual void Destroy()
    {
        // Hide the ship visually and start the death animation coroutine
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        isDead = true;

        EventManager.EnemyDestroyedEvent(gameObject);

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

        // Create the debris
        GameObject currency = Instantiate(_currencyPrefab[Random.Range(0, _currencyPrefab.Count)], transform.position, transform.rotation);
        currency.GetComponent<CurrencyDrop>().SetCurrency(_currencyDrop);

        // Wait for the death animation to complete
        yield return StartCoroutine(DeathAnimation());

        // After the animation is done, deactivate the entire GameObject
        gameObject.SetActive(false);

    }

    IEnumerator DeathAnimation()
    {
        GameObject exp = ObjectPooler.Instance.SpawnFromPool(_deathExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f); // Wait for the animation to finish
        exp.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            if (!isDead)
            {
                TakeDamage(other.GetComponent<Bullet>().BulletDamage);
                other.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Getters and Setters
    /// </summary>
    /// 
    public float GetHealth() => _health;
    public float GetCurrencyDrop() => _currencyDrop;
    public float GetSpeed() => _speed;
    public float GetStopDistance() => _stopDistance;
    public void SetHealth(float health) => _health = health;
    public void SetCurrencyDrop(float currency) => _currencyDrop = currency;
    public void SetSpeed(float speed) => _speed = speed;
    public void SetStopDistance(float distance) => _stopDistance = distance;
}
