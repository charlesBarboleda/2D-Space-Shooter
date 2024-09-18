using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] AudioClip _shootSound;
    [SerializeField] string _bulletType;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletDamage;
    [SerializeField] int _amountOfBullets;
    [SerializeField] float _shootingAngle;
    [SerializeField] float _bulletLifetime;
    Transform _target;

    public float nextFireTime;

    private void PlayShootSound()
    {
        if (_shootSound != null && !AudioSource.isPlaying)
        {
            AudioSource.PlayOneShot(_shootSound);
        }
        else
        {
            Debug.LogWarning("Attempted to play a shooting sound, but no AudioClip is assigned to _shootSound.");
        }
    }

    protected override void Attack()
    {
        FireBullets(_amountOfBullets, bulletSpawnPoint.position, TargetManager.TargetPosition);
        Debug.Log("Firing Bullets");
        PlayShootSound();
    }

    public virtual void FireBullets(int bulletAmount, Vector3 position, Vector3 targetPosition)
    {
        Vector3 targetDirection = (targetPosition - position).normalized;
        float startAngle = -_amountOfBullets / 2.0f * _shootingAngle;

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject enemyBullet = ObjectPooler.Instance.SpawnFromPool(_bulletType, position, Quaternion.identity);
            float angle = startAngle + i * _shootingAngle;
            Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * targetDirection;
            Bullet bullet = enemyBullet.GetComponent<Bullet>();
            bullet.Initialize(_bulletSpeed, _bulletDamage, _bulletLifetime, bulletDirection);
            enemyBullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, bulletDirection);
            enemyBullet.tag = "EnemyBullet";
        }
    }

    public override void IncreaseStatsPerLevel()
    {
        base.IncreaseStatsPerLevel();
        _bulletSpeed += LevelManager.Instance.CurrentLevelIndex * 0.01f;
        _bulletDamage += LevelManager.Instance.CurrentLevelIndex * 0.5f;
        AttackManager.AimRange += LevelManager.Instance.CurrentLevelIndex * 0.1f;
    }

    public override void BuffedState()
    {
        base.BuffedState();
        _bulletDamage *= 1.5f;
        _bulletSpeed *= 1.5f;
        AttackManager.AttackCooldown /= 1.5f;
        _amountOfBullets += 2;
    }

    public override void UnBuffedState()
    {
        base.UnBuffedState();
        _bulletDamage /= 1.5f;
        _bulletSpeed /= 1.5f;
        AttackManager.AttackCooldown *= 1.5f;
        _amountOfBullets -= 2;
    }


    // Properties for bullet settings


    public int BulletAmount { get => _amountOfBullets; set => _amountOfBullets = value; }
    public float BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }
    public float BulletDamage { get => _bulletDamage; set => _bulletDamage = value; }
    public float ShootingAngle { get => _shootingAngle; set => _shootingAngle = value; }
    public float BulletLifetime { get => _bulletLifetime; set => _bulletLifetime = value; }
}
