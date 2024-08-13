using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [SerializeField] private GameObject bulletPrefab;

    private bool isFiring;
    public float fireRate;
    public float bulletSpeed;
    public float bulletDamage;
    public int amountOfBullets;
    public float aimRange;
    public float shootingAngle;
    public float bulletLifetime;
    private Coroutine firingCoroutine;


    private float nextFireTime;

    private void Start()
    {
        SpawnAnimation();
        InitializeStats(health, pointsDrop, speed);
        player = GameManager.Instance.GetPlayer();
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        Movement(player.transform);
        if (shouldRotate) Aim(player.transform);

        if (Vector2.Distance(transform.position, player.transform.position) < aimRange && Time.time >= nextFireTime)

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




    private void StopAttack()
    {
        StopCoroutine(firingCoroutine);
        isFiring = false;
    }
    private IEnumerator FireBulletsContinuously()
    {
        while (isFiring)
        {
            FireBullets(amountOfBullets, transform.position, player.transform);
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



    public override void Attack()
    {

        isFiring = true;
        firingCoroutine = StartCoroutine(FireBulletsContinuously());
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
