using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Laser")]
public class AbilityLaser : Ability
{

    [SerializeField] GameObject _laserPrefab;
    public float duration;
    public float dps;

    public override void AbilityLogic(GameObject owner, Transform target)
    {
        //Instantiate the laser prefab
        GameObject Laser = Instantiate(_laserPrefab, owner.transform.position, Quaternion.identity);
        Laser.transform.rotation = owner.transform.rotation;
        Laser.transform.SetParent(owner.transform);
        Destroy(Laser, duration);

        //Pass the damage value to the laser
        PlayerLaserSettings laserScript = Laser.GetComponent<PlayerLaserSettings>();
        laserScript.SetDamage(dps);
    }

    public void ResetLaserStats()
    {
        duration = 3f;
        dps = 5f;
        cooldown = 45f;
        isUnlocked = false;
    }


}
