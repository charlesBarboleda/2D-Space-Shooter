using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed { get; private set; }
    public float BulletDamage { get; private set; }
    public float BulletLifetime { get; set; } = 5f;
    GameObject _bulletOnHitEffect;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;

    private Rigidbody2D rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (_bulletOnHitEffect != null) _bulletOnHitEffect.SetActive(false);
        if (_boxCollider2D != null) _boxCollider2D.enabled = true;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        if (rb != null) rb.velocity = Vector2.zero;

    }

    public void Initialize(float speed, float damage, float lifetime, Vector3 direction)
    {
        BulletSpeed = speed;
        BulletDamage = damage;
        BulletLifetime = lifetime;

        rb.velocity = direction.normalized * BulletSpeed;

        if (BulletLifetime > 0)
        {
            Invoke(nameof(Deactivate), BulletLifetime);
        }
    }

    private void Deactivate()
    {

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("EnemyDestroyable") || other.CompareTag("CrimsonFleet") || other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates"))
        {

            StartCoroutine(BulletOnHitEffect());
            other.GetComponent<IDamageable>()?.TakeDamage(BulletDamage);

        }
    }

    IEnumerator BulletOnHitEffect()
    {
        _spriteRenderer.enabled = false;
        _boxCollider2D.enabled = false;
        _bulletOnHitEffect = ObjectPooler.Instance.SpawnFromPool("BulletHitEffect", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Deactivate();
        _bulletOnHitEffect.SetActive(false);
    }



}

