using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected PlayerManager player;
    public GameObject spawnAnimation;
    public GameObject deathExplosion;
    public bool shouldRotate;
    public float health;
    public float pointsWorth;
    public float speed;
    public float stopDistance;
    public float earlyGameHealth;
    public float midGameHealth;
    public float lateGameHealth;
    public abstract void Attack();


    public virtual void OnEnable()
    {
        AdjustStatsBasedOnLevel();
    }
    public virtual void Awake()
    {
        player = GameManager.Instance.GetPlayer();
    }
    public virtual void Movement()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > stopDistance)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    public void InitializeStats(float health, float pointsWorth, float speed)
    {
        this.health += health;
        this.pointsWorth = pointsWorth;
        this.speed += speed;
    }

    public void Aim()
    {
        Vector3 target = player.transform.position;
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270f));
    }

    public virtual void AdjustStatsBasedOnLevel()
    {
        float level = GameManager.Instance.level;
        if (level <= 50) health = earlyGameHealth;
        else if (level <= 80) health = midGameHealth;
        else if (level > 100) health = lateGameHealth;

        health += level switch
        {
            >= 200 => level * 500,
            >= 150 => level * 50,
            >= 100 => level * 20,
            >= 80 => level * 5,
            >= 60 => level * 2,
            >= 40 => level * 0.75f,
            >= 20 => level * 0.3f,
            _ => level * 0.1f
        };

        speed += level * 0.001f;
        transform.localScale += new Vector3(level * 0.01f, level * 0.01f, 0);

    }
    public virtual void SpawnAnimation()
    {
        GameObject obj = Instantiate(spawnAnimation, transform.position, transform.rotation);
        Destroy(obj, 1f);
    }



    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy();
            GameManager.Instance.points += pointsWorth;
        }
    }

    public int LaserDamageByLevel()
    {
        return GameManager.Instance.level * 2;
    }


    public void Destroy()
    {
        GameObject exp = Instantiate(deathExplosion, transform.position, transform.rotation);
        Destroy(exp, 1f);
        gameObject.SetActive(false);
    }


}
