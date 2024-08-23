using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [Header("Turret Settings")]


    public GameObject turretPrefab;
    public int numberOfTurretsPerSide = 0;
    public float turretSpacing = 0.2f;

    private List<GameObject> turrets = new List<GameObject>();

    public void SpawnTurrets()
    {
        // Calculate the offset for the next turret
        float offset = (numberOfTurretsPerSide + 1) * turretSpacing;

        // Calculate the position relative to the player's rotation
        Vector3 leftOffset = new Vector3(-offset, -0.3f, 0);
        Vector3 rightOffset = new Vector3(offset, -0.3f, 0);

        // Rotate the offset based on the player's rotation
        Vector3 leftPosition = transform.position + transform.rotation * leftOffset;
        Vector3 rightPosition = transform.position + transform.rotation * rightOffset;

        // Spawn the turret on the left side
        GameObject leftTurret = Instantiate(turretPrefab, leftPosition, transform.rotation);
        turrets.Add(leftTurret);
        leftTurret.transform.SetParent(transform);


        // Spawn the turret on the right side
        GameObject rightTurret = Instantiate(turretPrefab, rightPosition, transform.rotation);
        turrets.Add(rightTurret);
        rightTurret.transform.SetParent(transform);

        numberOfTurretsPerSide++;


    }

    public void SetTurretDamage(float bulletDamage)
    {
        foreach (var turret in turrets)
        {
            var turretScript = turret.GetComponent<Turret>();
            if (turretScript != null)
            {
                turretScript.bulletDamage = bulletDamage;
            }
        }
    }
    public void SetTurretFireRate(float fireRate)
    {
        foreach (var turret in turrets)
        {
            var turretScript = turret.GetComponent<Turret>();
            if (turretScript != null)
            {
                turretScript.fireRate = fireRate;
            }
        }
    }

    public float GetTurretDamage()
    {
        if (turrets.Count > 0)
        {
            var turretScript = turrets[0].GetComponent<Turret>();
            if (turretScript != null)
            {
                return turretScript.bulletDamage;
            }
        }
        return 0;
    }
    public float GetTurretFireRate()
    {
        if (turrets.Count > 0)
        {
            var turretScript = turrets[0].GetComponent<Turret>();
            if (turretScript != null)
            {
                return turretScript.fireRate;
            }
        }
        return 0;
    }
}
