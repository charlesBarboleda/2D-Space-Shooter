using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Faction))]
public class CargoShip : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject spawnAnimation;
    [SerializeField] GameObject deathAnimation;
    [SerializeField] float _health = 1000;
    public bool isDead { get; set; }
    Faction _faction;
    public List<string> deathEffect { get; set; }
    public string deathExplosion { get; set; }
    SpriteRenderer _spriteRenderer;
    List<BoxCollider2D> _colliders = new List<BoxCollider2D>();


    void Start()
    {
        _faction = GetComponent<Faction>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colliders.AddRange(GetComponents<BoxCollider2D>());
    }
    void OnEnable()
    {
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
    }

    public void TeleportAway()
    {
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
        gameObject.SetActive(false);
    }

    public float GetHealth()
    {
        return _health;
    }

    public void SetHealth(float _health)
    {
        this._health = _health;

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

    public void Die()
    {
        StartCoroutine(HandleDeath());
    }

    public IEnumerator HandleDeath()
    {
        isDead = true;
        _spriteRenderer.enabled = false;
        _colliders.ForEach(collider => collider.enabled = false);
        CameraShake.Instance.TriggerShake(5, 0.3f);

        yield return StartCoroutine(DeathAnimation());

        gameObject.SetActive(false);
    }

    public IEnumerator DeathAnimation()
    {
        GameObject exp = ObjectPooler.Instance.SpawnFromPool(deathExplosion, transform.position, Quaternion.identity);
        GameObject exp2 = ObjectPooler.Instance.SpawnFromPool(deathEffect[Random.Range(0, deathEffect.Count)], transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);

        exp.SetActive(false);
        exp2.SetActive(false);
    }
}
