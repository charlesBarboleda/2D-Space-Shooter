using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [Header("Turret Settings")]
    public GameObject turretPrefab;
    public int numberOfTurretsPerSide = 0;
    public float turretSpacing = 0.4f;

    private List<GameObject> turrets = new List<GameObject>();

    public void SpawnTurrets()
    {
        // Clear existing turrets
        foreach (var turret in turrets)
        {
            Destroy(turret);
        }
        turrets.Clear();

        // Calculate offsets
        float offset = (numberOfTurretsPerSide + 1) * turretSpacing;
        Vector3 leftOffset = new Vector3(-offset, 0, 0);
        Vector3 rightOffset = new Vector3(offset, 0, 0);

        // Spawn turrets
        for (int i = 0; i < numberOfTurretsPerSide; i++)
        {
            // Left side
            Vector3 leftPosition = transform.position + leftOffset;
            GameObject leftTurret = Instantiate(turretPrefab, leftPosition, Quaternion.identity);
            leftTurret.transform.SetParent(transform);
            turrets.Add(leftTurret);

            // Right side
            Vector3 rightPosition = transform.position + rightOffset;
            GameObject rightTurret = Instantiate(turretPrefab, rightPosition, Quaternion.identity);
            rightTurret.transform.SetParent(transform);
            turrets.Add(rightTurret);

            // Move offsets for the next turret
            leftOffset += new Vector3(turretSpacing, 0, 0);
            rightOffset += new Vector3(turretSpacing, 0, 0);
        }
    }

    public void UpdateTurretStats(float fireRate, float bulletSpeed, float bulletDamage)
    {
        foreach (var turret in turrets)
        {
            var turretScript = turret.GetComponent<Turret>();
            if (turretScript != null)
            {
                turretScript.SetStats(fireRate, bulletSpeed, bulletDamage);
            }
        }
    }
}
