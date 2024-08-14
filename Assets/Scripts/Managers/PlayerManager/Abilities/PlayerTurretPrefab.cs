using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretPrefab : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private Weapon weapon = new Weapon();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !weapon.isFiring)
        {
            weapon.isFiring = true;
            weapon.firingCoroutine = StartCoroutine(FireBulletsContinuously());
        }

        if (Input.GetMouseButtonUp(0) && weapon.isFiring)
        {
            StopCoroutine(weapon.firingCoroutine);
            weapon.isFiring = false;
        }
    }

    private IEnumerator FireBulletsContinuously()
    {
        while (weapon.isFiring)
        {
            weapon.FireBullets(weapon.amountOfBullets, transform.position);
            yield return new WaitForSeconds(weapon.fireRate);
        }
    }
}
