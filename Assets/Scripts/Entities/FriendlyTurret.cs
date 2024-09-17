using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyTurret : ShooterEnemy
{
    protected override void Attack()
    {
        FireBullets(GetBulletAmount(), transform.position, TargetManager.TargetPosition);
    }

    public override void FireBullets(int bulletAmount, Vector3 position, Vector3 targetPosition)
    {
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