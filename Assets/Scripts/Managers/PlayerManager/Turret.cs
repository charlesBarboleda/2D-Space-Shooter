using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject turret1;
    [SerializeField] private GameObject turret2;
    public float turretCountDownTime;

    public void Turrets()
    {

        if (turretCountDownTime > 0)
        {
            turret2.SetActive(true);
            turret1.SetActive(true);
            weapon.FireBullets(weapon.amountOfBullets, turret1.transform.position);
            weapon.FireBullets(weapon.amountOfBullets, turret2.transform.position);
        }
    }

    public void DisableTurrets()
    {
        turret1.SetActive(false);
        turret2.SetActive(false);
    }
    public void TurretTimer()
    {
        if (turretCountDownTime > 0)
        {
            turretCountDownTime -= Time.deltaTime;
            Turrets();
        }
        else
        {
            turretCountDownTime = 0;
            DisableTurrets();
        }
    }

    public void AddTurretTime(float time)
    {
        turretCountDownTime += time;
    }
}
