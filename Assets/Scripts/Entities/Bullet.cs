using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed { get; private set; }
    public float BulletDamage { get; private set; }
    public float BulletLifetime { get; set; } = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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


}

