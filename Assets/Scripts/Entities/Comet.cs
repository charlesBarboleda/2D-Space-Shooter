using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Comet : MonoBehaviour, IDamageable
{
    int hitsToBreak = 5;
    [SerializeField] List<Transform> _targets;
    [SerializeField] float _speed = 5f;
    [SerializeField] List<string> _powerUps;
    SpriteRenderer _spriteRenderer;
    List<CircleCollider2D> _colliders = new List<CircleCollider2D>();
    bool _isDead;


    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colliders.AddRange(GetComponents<CircleCollider2D>());
    }
    void OnEnable()
    {
        isDead = false;
        hitsToBreak = 5;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        _colliders.ForEach(collider => collider.enabled = true);
    }
    void OnDisable()
    {
        isDead = true;
    }
    void Update()
    {
        int RandomTarget = Random.Range(0, _targets.Count);
        transform.position = Vector3.MoveTowards(transform.position, _targets[RandomTarget].position, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _targets[RandomTarget].position) < 1f)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        CameraFollowBehaviour.Instance.ShakePlayerCamera(20f, 5.0f, 1f);

        hitsToBreak--;
        if (hitsToBreak <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            UIManager.Instance.CreateOnHitDamageText("1", other.transform.position);
            TakeDamage(0);
        }
    }

    public virtual void Die()
    {
        // Hide the ship visually and start the death animation coroutine
        StartCoroutine(HandleDeath());
    }

    public IEnumerator HandleDeath()
    {

        isDead = true;

        // Disable all colliders
        _colliders.ForEach(collider => collider.enabled = false);

        // Hide the ship's sprite
        _spriteRenderer.enabled = false;


        // Drop a random power up
        if (_powerUps.Count > 0)
        {
            GameObject powerUp = ObjectPooler.Instance.SpawnFromPool(_powerUps[Random.Range(0, _powerUps.Count)], transform.position, Quaternion.identity);
        }

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
    public float Speed { get => _speed; set => _speed = value; }
    public int HitsToBreak { get => hitsToBreak; set => hitsToBreak = value; }

    public bool isDead { get => _isDead; set => _isDead = value; }
    [SerializeField] List<string> _deathEffect;
    public List<string> deathEffect { get => _deathEffect; set => _deathEffect = value; }
    [SerializeField] string _deathExplosion;
    public string deathExplosion { get => _deathExplosion; set => _deathExplosion = value; }
}
