using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float aimRange;

    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    [SerializeField] int amountOfBullets;
    [SerializeField] float shootingAngle;
    [SerializeField] float bulletLifetime;
    bool isFiring;
    Coroutine firingCoroutine;


    float nextFireTime;

    void Start()
    {
        SpawnAnimation();
        player = GameManager.Instance.GetPlayer();
    }


    void FixedUpdate()
    {
        if (player == null) return;
        Movement(player.transform);
        if (shouldRotate) Aim(player.transform);

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < aimRange && Time.time >= nextFireTime)

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

    void StopAttack()
    {
        StopCoroutine(firingCoroutine);
        isFiring = false;
    }
    IEnumerator FireBulletsContinuously()
    {
        while (isFiring)
        {

            FireBullets(amountOfBullets, bulletSpawnPoint.position, player.transform);
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
        bulletSpeed += GameManager.Instance.level * 0.05f;
        bulletDamage += GameManager.Instance.level * 0.5f;
        aimRange += GameManager.Instance.level * 0.05f;

    }


    public override void Attack()
    {

        isFiring = true;
        firingCoroutine = StartCoroutine(FireBulletsContinuously());
    }

}
