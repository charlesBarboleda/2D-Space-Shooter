using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public WeaponType weaponType;
    public float fireRate = 0.2f;
    public float bulletSpeed = 30f;
    public float bulletDamage = 20f;
    public float bulletLifetime = 5f;
    public int amountOfBullets = 1; // Number of bullets fired in a burst
    public float shootingAngle = 10f; // Angle to spread bullets

    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Transform bulletSpawnPoint;

    private bool isFiring = false;
    private Coroutine firingCoroutine;

    private float _timeSinceLastShot = 0f; // Time tracking for fire rate cooldown

    private void Update()
    {
        // Update the cooldown timer
        _timeSinceLastShot += Time.deltaTime;

        // If mouse button is pressed and the fire rate cooldown has passed, fire the weapon
        if (Input.GetMouseButton(0) && !isFiring && _timeSinceLastShot >= fireRate)
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
            // Play the shooting sound and fire bullets
            FireBullets(amountOfBullets, bulletSpawnPoint.position);

            audioSource.pitch = Random.Range(0.85f, 1.15f);
            // Play the shooting sound
            audioSource.PlayOneShot(shootSound);

            // Reset the cooldown timer after firing
            _timeSinceLastShot = 0f;

            yield return new WaitForSeconds(fireRate);
        }
    }

    private void FireBullets(int bulletAmount, Vector3 position)
    {
        Vector3 bulletSpawnPoint = position;
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bulletSpawnPoint).normalized;
        direction.z = 0; // Ensure it's a 2D game

        float startAngle = -bulletAmount / 2.0f * shootingAngle; // Start angle to spread bullets evenly

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool(weaponType.ToString(), bulletSpawnPoint, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                // Calculate the spread angle for each bullet
                float spreadAngle = startAngle + i * shootingAngle;
                Vector3 bulletDirection = Quaternion.Euler(0, 0, spreadAngle) * direction;
                bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, bulletDirection);
                bulletScript.Initialize(bulletSpeed, bulletDamage, bulletLifetime, bulletDirection);
            }
        }
    }

    public void SetStats(float newFireRate, float newBulletSpeed, float newBulletDamage, int newAmountOfBullets, float newShootingAngle)
    {
        fireRate = newFireRate;
        bulletSpeed = newBulletSpeed;
        bulletDamage = newBulletDamage;
        amountOfBullets = newAmountOfBullets;
        shootingAngle = newShootingAngle;
    }
}
