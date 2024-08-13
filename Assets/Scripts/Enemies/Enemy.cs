using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected PlayerManager player;

    // Animations
    public GameObject spawnAnimation;
    public GameObject deathExplosion;
    private SpriteRenderer spriteRenderer;
    // Stats
    public bool shouldRotate;
    public float health;
    public float pointsDrop;
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
    public void InitializeStats(float health, float pointsDrop, float speed)
    {
        this.health += health;
        this.pointsDrop = pointsDrop;
        this.speed += speed;
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
        StartCoroutine(FlashRed());
        if (health <= 0)
        {
            Destroy();
        }
    }



    public virtual void Destroy()
    {
        GameObject exp = Instantiate(deathExplosion, transform.position, transform.rotation);
        Destroy(exp, 1f);
        gameObject.SetActive(false);
        CameraShaker.Instance.ShakeOnce(cameraShakeMagnitude, cameraShakeRoughness, cameraShakeFadeInTime, cameraShakeFadeOutTime);
    }


}
