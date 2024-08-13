using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Laser")]
public class AbilityLaser : Ability
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float duration;
    [SerializeField] private float dps;

    public override void UseAbility(GameObject owner, Transform target)
    {
        //Instantiate the laser prefab
        GameObject Laser = Instantiate(laserPrefab, owner.transform.position, Quaternion.identity);
        Laser.transform.rotation = owner.transform.rotation;
        Laser.transform.SetParent(owner.transform);
        Destroy(Laser, duration);

        //Pass the damage value to the laser
        LaserPrefab laserScript = Laser.GetComponent<LaserPrefab>();


    }




}
