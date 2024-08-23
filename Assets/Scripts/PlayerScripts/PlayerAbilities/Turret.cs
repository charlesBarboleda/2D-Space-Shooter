using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
    public float bulletDamage = 10f;

    bool isFiring = false;
    Coroutine firingCoroutine;

    private void Update()
    {
        // Check if firing input is received (e.g., mouse button 1 held down)
        if (Input.GetMouseButton(1) && !isFiring)
        {
            StartFiring();
        }
        else if (!Input.GetMouseButton(1) && isFiring)
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

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(bulletSpeed, bulletDamage, bulletScript.BulletLifetime, direction);
        }
    }

    public void SetStats(float newFireRate, float newBulletSpeed, float newBulletDamage)
    {
        fireRate = newFireRate;
        bulletSpeed = newBulletSpeed;
        bulletDamage = newBulletDamage;
    }
}
