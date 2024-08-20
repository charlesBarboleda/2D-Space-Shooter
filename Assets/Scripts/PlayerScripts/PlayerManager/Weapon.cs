using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
    public BulletAudio bulletAudio;
    public float shootingAngle = 10f;

    public bool isFiring;
    public Coroutine firingCoroutine;
    public float fireRate = 0.2f;
    public int amountOfBullets = 1;
    public int bulletDamage = 20;
    public float bulletLifetime = 5f;
    public float bulletSpeed = 30f;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !isFiring)
        {
            isFiring = true;
            firingCoroutine = StartCoroutine(FireBulletsContinuously());
        }

        if (Input.GetMouseButtonUp(0) && isFiring)
        {
            StopCoroutine(firingCoroutine);
            isFiring = false;
        }
    }

    private IEnumerator FireBulletsContinuously()
    {
        while (isFiring)
        {
            FireBullets(amountOfBullets, transform.position);
            bulletAudio.PlayOneShot(bulletAudio.shootingSound);
            yield return new WaitForSeconds(fireRate);
        }
    }
    public void FireBullets(int bulletAmount, Vector3 position)
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Assuming a 2D game
        Vector3 bulletSpawnPoint = position;


        // Calculate the direction from the GameObject to the mouse position
        Vector3 direction = (mousePosition - bulletSpawnPoint).normalized;

        float startAngle = -bulletAmount / 2.0f * shootingAngle; // Start angle to spread bullets evenly

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject playerBullet = ObjectPooler.Instance.SpawnFromPool("PlayerBullet", bulletSpawnPoint, Quaternion.identity);


            // Calculate the spread angle for each bullet
            float angle = startAngle + i * shootingAngle;
            Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * direction;

            // Set the bullet's rotation
            playerBullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, bulletDirection);

            // Set bullet properties
            playerBullet.GetComponent<Bullet>().Initialize(bulletSpeed, bulletDamage, bulletLifetime, bulletDirection);
            playerBullet.transform.gameObject.tag = "PlayerBullet";

            // Apply the direction to the bullet (assuming the bullet script handles movement based on direction)
            // playerBullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
        }


    }

    public void IncreaseBulletDamage(int amount)
    {
        bulletDamage += amount;
    }
    public void IncreaseBulletSpeed(int amount)
    {
        bulletSpeed += amount;
    }
    public void IncreaseBullets(int amount)
    {
        amountOfBullets += amount;
    }

    public void ReduceFireRate(float amount)
    {
        fireRate -= amount;
    }
}
