using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyTurret : ShooterEnemy
{

    public override void Update()
    {
        Aim(CheckForTargets());
        Transform target = CheckForTargets();
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget < GetAimRange() && Time.time >= nextFireTime)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        FireBullets(GetBulletAmount(), transform.position, CheckForTargets());

    }

    public override Transform CheckForTargets()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, 100f);
        foreach (Collider2D targets in hitTargets)
        {
            if (targets.CompareTag("Enemy"))
            {
                return targets.transform;
            }
        }
        return null;
    }
    public override void FireBullets(int bulletAmount, Vector3 position, Transform target)
    {
        nextFireTime = Time.time + GetFireRate();
        Vector3 targetPosition = target.transform.position;
        Vector3 targetDirection = targetPosition - position;
        float startAngle = -GetBulletAmount() / 2.0f * GetShootingAngle();

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject playerBullet = ObjectPooler.Instance.SpawnFromPool("PlayerBullet", position, Quaternion.identity);

            // Calculate the spread angle for each bullet
            float angle = startAngle + i * GetShootingAngle();
            Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * targetDirection;
            playerBullet.GetComponent<Bullet>().Initialize(GetBulletSpeed(), GetBulletDamage(), GetBulletLifetime(), bulletDirection);

            // Set the bullet's rotation
            playerBullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, bulletDirection);

            // Set bullet properties
            playerBullet.transform.gameObject.tag = "PlayerBullet";
        }
    }
}