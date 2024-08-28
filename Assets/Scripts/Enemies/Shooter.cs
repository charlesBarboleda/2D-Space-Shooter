using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] public float aimRange;

    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    [SerializeField] int amountOfBullets;
    [SerializeField] float shootingAngle;
    [SerializeField] float bulletLifetime;

    bool isFiring;
    public Coroutine firingCoroutine;


    public float nextFireTime;



    public override void Update()
    {
        base.Update();
        float distanceToTarget = Vector2.Distance(transform.position, CheckForTargets().position);
        if (distanceToTarget < aimRange && Time.time >= nextFireTime)

        {
            Attack();
        }
        else
        {
            if (firingCoroutine != null)
            {
                StopAttack();

            }


        }

    }

    public void StopAttack()
    {
        StopCoroutine(firingCoroutine);
        isFiring = false;
    }
    IEnumerator FireBulletsContinuously()
    {
        while (isFiring)
        {

            FireBullets(amountOfBullets, bulletSpawnPoint.position, CheckForTargets());
            yield return new WaitForSeconds(fireRate);
        }
    }
    public void FireBullets(int bulletAmount, Vector3 position, Transform target)
    {

        nextFireTime = Time.time + fireRate;
        Vector3 targetPosition = target.transform.position;
        Vector3 targetDirection = targetPosition - position;
        float startAngle = -amountOfBullets / 2.0f * shootingAngle;

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject enemyBullet = ObjectPooler.Instance.SpawnFromPool("Bullet", position, Quaternion.identity);


            // Calculate the spread angle for each bullet
            float angle = startAngle + i * shootingAngle;
            Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * targetDirection;
            enemyBullet.GetComponent<Bullet>().Initialize(bulletSpeed, bulletDamage, bulletLifetime, bulletDirection);

            // Set the bullet's rotation
            enemyBullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, bulletDirection);

            // Set bullet properties
            enemyBullet.transform.gameObject.tag = "EnemyBullet";
        }


    }

    public override void OnEnable()
    {
        base.OnEnable();
        bulletSpeed += GameManager.Instance.level * 0.07f;
        bulletDamage += GameManager.Instance.level * 1f;
        aimRange += GameManager.Instance.level * 0.03f;

    }


    public override void Attack()
    {

        isFiring = true;
        firingCoroutine = StartCoroutine(FireBulletsContinuously());
    }

    public void SetBulletAmount(int amount)
    {
        amountOfBullets = amount;
    }
    public void SetBulletSpeed(float speed)
    {
        bulletSpeed = speed;
    }
    public void SetBulletDamage(float damage)
    {
        bulletDamage = damage;
    }
    public void SetFireRate(float rate)
    {
        fireRate = rate;
    }
    public void SetShootingAngle(float angle)
    {
        shootingAngle = angle;
    }

    public void SetAimRange(float range)
    {
        aimRange = range;
    }

    public int GetBulletAmount()
    {
        return amountOfBullets;
    }
    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }
    public float GetBulletDamage()
    {
        return bulletDamage;
    }
    public float GetFireRate()
    {
        return fireRate;
    }
    public float GetAimRange()
    {
        return aimRange;
    }
    public float GetShootingAngle()
    {
        return shootingAngle;
    }
    public float GetBulletLifetime()
    {
        return bulletLifetime;
    }





}
