using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    AudioSource _audioSource;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] AudioClip _shootSound;
    [SerializeField] public float aimRange;

    [SerializeField] float _fireRate;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletDamage;
    [SerializeField] int _amountOfBullets;
    [SerializeField] float _shootingAngle;
    [SerializeField] float _bulletLifetime;

    public float nextFireTime;

    public virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public override void Update()
    {
        base.Update();

        Transform target = CheckForTargets();
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget < aimRange && Time.time >= nextFireTime)
        {
            Attack();
            // Play the shoot sound
            if (_shootSound != null)
            {
                _audioSource.PlayOneShot(_shootSound);
            }
            else
            {
                Debug.LogWarning("Attempted to play a shooting sound, but no AudioClip is assigned to _shootSound.");
            }
        }
    }

    public override void Attack()
    {
        FireBullets(_amountOfBullets, bulletSpawnPoint.position, CheckForTargets());
    }

    public virtual void FireBullets(int bulletAmount, Vector3 position, Transform target)
    {
        nextFireTime = Time.time + _fireRate;
        Vector3 targetPosition = target.transform.position;
        Vector3 targetDirection = targetPosition - position;
        float startAngle = -_amountOfBullets / 2.0f * _shootingAngle;

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject enemyBullet = ObjectPooler.Instance.SpawnFromPool("Bullet", position, Quaternion.identity);
            // Calculate the spread angle for each bullet
            float angle = startAngle + i * _shootingAngle;
            Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * targetDirection;
            enemyBullet.GetComponent<Bullet>().Initialize(_bulletSpeed, _bulletDamage, _bulletLifetime, bulletDirection);

            // Set the bullet's rotation
            enemyBullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, bulletDirection);

            // Set bullet properties
            enemyBullet.transform.gameObject.tag = "EnemyBullet";
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void IncreaseStatsPerLevel()
    {
        base.IncreaseStatsPerLevel();
        _bulletSpeed += GameManager.Instance.Level() * 0.1f;
        _bulletDamage += GameManager.Instance.Level() * 1f;
        aimRange += GameManager.Instance.Level() * 0.1f;
    }

    public void SetBulletAmount(int amount)
    {
        _amountOfBullets = amount;
    }
    public void SetBulletSpeed(float speed)
    {
        _bulletSpeed = speed;
    }
    public void SetBulletDamage(float damage)
    {
        _bulletDamage = damage;
    }
    public void SetFireRate(float rate)
    {
        _fireRate = rate;
    }
    public void SetShootingAngle(float angle)
    {
        _shootingAngle = angle;
    }

    public void SetAimRange(float range)
    {
        aimRange = range;
    }

    public int GetBulletAmount()
    {
        return _amountOfBullets;
    }
    public float GetBulletSpeed()
    {
        return _bulletSpeed;
    }
    public float GetBulletDamage()
    {
        return _bulletDamage;
    }
    public float GetFireRate()
    {
        return _fireRate;
    }
    public float GetAimRange()
    {
        return aimRange;
    }
    public float GetShootingAngle()
    {
        return _shootingAngle;
    }
    public float GetBulletLifetime()
    {
        return _bulletLifetime;
    }





}
