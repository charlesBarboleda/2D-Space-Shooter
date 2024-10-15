using System.Collections;
using UnityEngine;

public class PlayerTurretPrefab : MonoBehaviour
{
    [Header("Turret Settings")]
    public float fireRate = 0.5f;
    public float bulletSpeed = 40f;
    public float bulletDamage = 10f;
    public float bulletLifetime = 5f;
    public int amountOfBullets = 1;
    public WeaponType bulletType = WeaponType.PlayerBullet;

    bool isFiring = false;
    Coroutine firingCoroutine;

    private void Update()
    {
        // Check if firing input is received (e.g., mouse button 1 held down)
        if (Input.GetMouseButton(0) && !isFiring)
        {
            StartFiring();
        }
        else if (!Input.GetMouseButton(0) && isFiring)
        {
            StopFiring();
        }
    }

    private void StartFiring()
    {
        isFiring = true;
        firingCoroutine = StartCoroutine(FireBulletsContinuously());
    }

    private void StopFiring()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
        }
        isFiring = false;
    }

    private IEnumerator FireBulletsContinuously()
    {
        while (isFiring)
        {
            FireBullet();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void FireBullet()
    {
        Vector3 bulletSpawnPoint = transform.position;
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bulletSpawnPoint).normalized;
        direction.z = 0; // Ensure it's a 2D game

        // Fire multiple bullets based on the amountOfBulletsFiredFromWeaponPerShot
        for (int i = 0; i < amountOfBullets; i++)
        {
            // Add some random spread if you want (optional)
            Vector3 spread = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            Vector3 bulletDirection = (direction + spread).normalized;

            GameObject bullet = ObjectPooler.Instance.SpawnFromPool(bulletType.ToString(), bulletSpawnPoint, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(bulletSpeed, bulletDamage, bulletLifetime, bulletDirection);
            }
        }
    }

    public void SetFireRate(float newFireRate)
    {
        fireRate += newFireRate;
    }
    public void SetDamage(float newDamage)
    {
        bulletDamage += newDamage;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public float GetBulletDamage()
    {
        return bulletDamage;
    }
}
