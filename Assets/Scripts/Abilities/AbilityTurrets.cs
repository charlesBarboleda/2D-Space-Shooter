using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Turrets")]
public class AbilityTurrets : Ability
{
    [SerializeField] int numberOfTurretsPerSide;
    [SerializeField] GameObject turret;
    [SerializeField] float turretSpacing = 0.5f;
    public float bulletDamage;
    public float fireRate;

    public override void AbilityLogic(GameObject owner, Transform target)
    {
        // Calculate the offset for the next turret
        float offset = (numberOfTurretsPerSide + 1) * turretSpacing;

        // Calculate the position relative to the player's rotation
        Vector3 leftOffset = new Vector3(-offset, -0.2f, 0);
        Vector3 rightOffset = new Vector3(offset, -0.2f, 0);

        // Rotate the offset based on the player's rotation
        Vector3 leftPosition = owner.transform.position + owner.transform.rotation * leftOffset;
        Vector3 rightPosition = owner.transform.position + owner.transform.rotation * rightOffset;

        // Spawn the turret on the left side
        GameObject leftTurret = Instantiate(turret, leftPosition, owner.transform.rotation);
        leftTurret.transform.SetParent(owner.transform);
        var leftTurretWeapon = leftTurret.GetComponent<PlayerTurretPrefab>();
        // Initialize turret properties here if needed
        leftTurretWeapon.bulletDamage = bulletDamage;
        leftTurretWeapon.fireRate = fireRate;

        // Spawn the turret on the right side
        GameObject rightTurret = Instantiate(turret, rightPosition, owner.transform.rotation);
        rightTurret.transform.SetParent(owner.transform);
        var rightTurretWeapon = rightTurret.GetComponent<PlayerTurretPrefab>();
        // Initialize turret properties here if needed
        rightTurretWeapon.bulletDamage = bulletDamage;
        rightTurretWeapon.fireRate = fireRate;


    }

    public void ResetTurretStats()
    {
        bulletDamage = 10f;
        fireRate = 0.4f;
        numberOfTurretsPerSide = 0;
        isUnlocked = false;
    }
}
