using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IDamageable
{

    [SerializeField] float health = 1000f;
    [SerializeField] List<GameObject> _turretChildren = new List<GameObject>();
    List<CircleCollider2D> _colliders = new List<CircleCollider2D>();
    SpriteRenderer _spriteRenderer;

    bool _isDead;
    public bool isDead { get => _isDead; set => _isDead = value; }
    [SerializeField] List<string> _deathEffect = new List<string>();
    public List<string> deathEffect { get => _deathEffect; set => _deathEffect = value; }
    [SerializeField] string _deathExplosion;
    public string deathExplosion { get => _deathExplosion; set => _deathExplosion = value; }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colliders.AddRange(GetComponents<CircleCollider2D>());
    }
    void OnEnable()
    {
        StartCoroutine(SpawnAnimation());
    }

    IEnumerator SpawnAnimation()
    {
        GameObject animation = ObjectPooler.Instance.SpawnFromPool("BuildingSpawn", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        animation.SetActive(false);
    }



    public void TeleportAway()
    {
        GameManager.Instance.RemoveEnemy(gameObject);
        StartCoroutine(TeleportEffect());

    }

    IEnumerator TeleportEffect()
    {

        yield return StartCoroutine(SpawnAnimation());
        gameObject.SetActive(false);
    }


    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float health)
    {
        this.health = health;

    }


    IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            {
                TakeDamage(bullet.BulletDamage);
                StartCoroutine(FlashRed());
            }
            other.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        if (health > 0)
        {
            StartCoroutine(FlashRed());
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        StartCoroutine(HandleDeath());
    }

    public IEnumerator HandleDeath()
    {
        isDead = true;

        // Disable the turrets if there are any
        if (_turretChildren.Count > 0) _turretChildren.ForEach(child => child.SetActive(false));

        // Disable all colliders
        _colliders.ForEach(collider => collider.enabled = false);

        // Hide the ship's sprite
        _spriteRenderer.enabled = false;

        // Shake the camera
        CameraShake.Instance.TriggerShake(3f, 0.3f);

        // Notify Objectives Manager
        ObjectivesManager.Instance.DestroyShip();

        // Notify Event Manager
        EventManager.EnemyDestroyedEvent(gameObject);

        // Wait for the death animation to complete
        yield return StartCoroutine(DeathAnimation());

        // After the animation is done, deactivate the entire GameObject
        gameObject.SetActive(false);
    }

    public IEnumerator DeathAnimation()
    {
        GameObject exp = ObjectPooler.Instance.SpawnFromPool(deathExplosion, transform.position, Quaternion.identity);
        GameObject exp2 = ObjectPooler.Instance.SpawnFromPool(deathEffect[Random.Range(0, deathEffect.Count)], transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f); // Wait for the animation to finish
        exp.SetActive(false);
        exp2.SetActive(false);
    }
}
