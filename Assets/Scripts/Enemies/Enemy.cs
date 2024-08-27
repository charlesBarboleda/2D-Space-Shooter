using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] List<GameObject> currencyPrefab;

    // Animations
    public GameObject spawnAnimation;
    public GameObject deathExplosion;
    SpriteRenderer spriteRenderer;
    // Stats
    [SerializeField] bool _shouldRotate;
    [SerializeField] float _health;
    [SerializeField] float _currencyDrop;
    [SerializeField] float _speed;
    [SerializeField] float _stopDistance;
    Transform target;

    // Camera Shake
    public float cameraShakeMagnitude;
    public float cameraShakeDuration;
    public abstract void Attack();


    public virtual void OnEnable()
    {
        IncreaseStatsPerLevel();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        bool rotateClockwise = true;
        float direction = rotateClockwise ? 1 : -1;
        transform.RotateAround(target.position, Vector3.forward, direction * _speed * Time.deltaTime);
    }

    public void Aim(Transform target)
    {
        Vector3 targetAim = target.transform.position;
        Vector3 direction = targetAim - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270f));
    }

    public void SpawnAnimation()
    {
        GameObject obj = Instantiate(spawnAnimation, transform.position, transform.rotation);
        Destroy(obj, 1f);
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }


    public void TakeDamage(float damage)
    {

        _health -= damage;
        if (_health > 0)
        {
            StartCoroutine(FlashRed());
        }
        if (_health <= 0)
        {
            Destroy();
        }
    }

    protected Transform CheckForTargets()
    {
        // Check for enemies using circle raycast
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, 75f);
        foreach (Collider2D targets in hitTargets)
        {
            if (targets.CompareTag("CargoShip"))
            {
                return targets.transform;
            }
        }
        return GameManager.Instance.GetPlayer().transform;
    }

    public virtual void IncreaseStatsPerLevel()
    {
        _health += GameManager.Instance.level * 10f;
        _currencyDrop += GameManager.Instance.level * 0.5f;
        _speed += GameManager.Instance.level * 0.05f;
    }

    public virtual void Destroy()
    {
        GameObject exp = Instantiate(deathExplosion, transform.position, transform.rotation);
        Destroy(exp, 1f);
        EventManager.EnemyDestroyedEvent(gameObject);
        gameObject.SetActive(false);
        CameraShake.Instance.TriggerShake(cameraShakeMagnitude, cameraShakeDuration);
        GameObject currency = Instantiate(currencyPrefab[Random.Range(0, currencyPrefab.Count)], transform.position, transform.rotation);
        currency.GetComponent<CurrencyDrop>().SetCurrency(_currencyDrop);
        ObjectivesManager.Instance.DestroyShip();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().BulletDamage);
            other.gameObject.SetActive(false);
        }
    }

}
