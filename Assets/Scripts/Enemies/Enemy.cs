using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] List<GameObject> currencyPrefab;
    protected PlayerManager player;

    // Animations
    public GameObject spawnAnimation;
    public GameObject deathExplosion;
    SpriteRenderer spriteRenderer;
    // Stats
    public bool shouldRotate;
    public float health;
    public float currencyDrop;
    public float speed;
    public float stopDistance;

    // Camera Shake
    public float cameraShakeMagnitude;
    public float cameraShakeRoughness;
    public float cameraShakeFadeInTime;
    public float cameraShakeFadeOutTime;
    public abstract void Attack();


    public virtual void OnEnable()
    {
        IncreaseStatsPerLevel();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void Awake()
    {
        player = GameManager.Instance.GetPlayer();
    }
    public virtual void Movement(Transform target)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > stopDistance)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
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

        health -= damage;
        if (health > 0)
        {
            StartCoroutine(FlashRed());
        }
        if (health <= 0)
        {
            Destroy();
        }
    }

    public virtual void IncreaseStatsPerLevel()
    {
        health += GameManager.Instance.level * 5f;
        currencyDrop += GameManager.Instance.level * 0.5f;
        speed += GameManager.Instance.level * 0.04f;
    }

    public virtual void Destroy()
    {
        GameObject exp = Instantiate(deathExplosion, transform.position, transform.rotation);
        Destroy(exp, 1f);
        EventManager.EnemyDestroyedEvent(gameObject);
        gameObject.SetActive(false);
        CameraShaker.Instance.ShakeOnce(cameraShakeMagnitude, cameraShakeRoughness, cameraShakeFadeInTime, cameraShakeFadeOutTime);
        GameObject currency = Instantiate(currencyPrefab[Random.Range(0, currencyPrefab.Count)], transform.position, transform.rotation);
        currency.GetComponent<CurrencyDrop>().SetCurrency(currencyDrop);
    }


}
