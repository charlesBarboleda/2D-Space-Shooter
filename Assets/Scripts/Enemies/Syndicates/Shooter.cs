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
    [SerializeField] BulletPatterns _bulletPattern;
    Transform _target;

    public float nextFireTime;

    private void PlayShootSound()
    {
        if (_shootSound != null && !AudioSource.isPlaying)
        {
            AudioSource.PlayOneShot(_shootSound);
        }
    }

    protected override void Attack()
    {
        Vector3 targetPosition = TargetManager.TargetPosition;
        Vector3 spawnPosition = bulletSpawnPoint.position;

        switch (_bulletPattern)
        {
            case BulletPatterns.RadialBurst:
                ShootRadialPattern(spawnPosition);
                break;
            case BulletPatterns.Spiral:
                ShootSpiralPattern(spawnPosition);
                break;
            case BulletPatterns.Wave:
                ShootWavePattern(spawnPosition, targetPosition);
                break;
            case BulletPatterns.Cone:
                ShootConePattern(spawnPosition, targetPosition);
                break;
            case BulletPatterns.RandomSpread:
                ShootRandomSpread(spawnPosition);
                break;
        }

        PlayShootSound();
    }

    private void ShootRadialPattern(Vector3 position)
    {
        float startAngle = 0f;
        float angleStep = 360f / _amountOfBullets;

        for (int i = 0; i < _amountOfBullets; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 bulletDirection = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
            FireBullet(position, bulletDirection);
        }
    }

    // Spiral Pattern
    private void ShootSpiralPattern(Vector3 position)
    {
        float angleStep = 360f / _amountOfBullets;
        float spiralSpeed = 5f; // Speed at which the spiral rotates
        float currentAngle = Time.time * spiralSpeed;

        for (int i = 0; i < _amountOfBullets; i++)
        {
            float angle = currentAngle + (i * angleStep);
            Vector3 bulletDirection = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
            FireBullet(position, bulletDirection);
        }
    }

    // Wave Pattern (Sinusoidal)
    private void ShootWavePattern(Vector3 position, Vector3 targetPosition)
    {
        Vector3 targetDirection = (targetPosition - position).normalized;
        float waveAmplitude = 1f;
        float waveFrequency = 1f;

        for (int i = 0; i < _amountOfBullets; i++)
        {
            float offset = Mathf.Sin(i * waveFrequency) * waveAmplitude;
            Vector3 waveDirection = targetDirection + new Vector3(offset, 0, 0);
            FireBullet(position, waveDirection.normalized);
        }
    }

    // Cone Pattern
    private void ShootConePattern(Vector3 position, Vector3 targetPosition)
    {
        Vector3 targetDirection = (targetPosition - position).normalized;
        float startAngle = -_shootingAngle / 2.0f;

        for (int i = 0; i < _amountOfBullets; i++)
        {
            float angle = startAngle + (i * (_shootingAngle / _amountOfBullets));
            Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * targetDirection;
            FireBullet(position, bulletDirection);
        }
    }

    // Random Spread Pattern
    private void ShootRandomSpread(Vector3 position)
    {
        for (int i = 0; i < _amountOfBullets; i++)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            FireBullet(position, randomDirection);
        }
    }

    // Fire a single bullet
    private void FireBullet(Vector3 position, Vector3 direction)
    {
        GameObject enemyBullet = ObjectPooler.Instance.SpawnFromPool(_bulletType, position, Quaternion.identity);
        Bullet bullet = enemyBullet.GetComponent<Bullet>();
        bullet.Initialize(_bulletSpeed, _bulletDamage, _bulletLifetime, direction);
        enemyBullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        enemyBullet.tag = "EnemyBullet";
    }

    public override void IncreaseStatsPerLevel()
    {
        base.IncreaseStatsPerLevel();
        _bulletSpeed += LevelManager.Instance.CurrentLevelIndex * 0.01f;
        _bulletDamage += LevelManager.Instance.CurrentLevelIndex * 0.5f;
        AttackManager.AimRange = Mathf.Max(LevelManager.Instance.CurrentLevelIndex * 0.1f, 100f);
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
    public BulletPatterns BulletPattern { get => _bulletPattern; set => _bulletPattern = value; }
}
