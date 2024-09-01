using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed { get; private set; }
    public float BulletDamage { get; private set; }
    public float BulletLifetime { get; set; } = 5f;
    GameObject _bulletHitEffect;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (_bulletHitEffect != null) _bulletHitEffect.SetActive(false);

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
        if (other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("CargoShip") || other.CompareTag("VIPBuilding"))
        {

            BulletHitEffect();
        }
    }

    void BulletHitEffect()
    {
        _bulletHitEffect = ObjectPooler.Instance.SpawnFromPool("BulletHitEffect", transform.position, Quaternion.identity);
    }


}

