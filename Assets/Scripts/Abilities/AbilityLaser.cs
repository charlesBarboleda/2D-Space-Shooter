using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Laser")]
public class AbilityLaser : Ability
{

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _duration;
    [SerializeField] private float _dps;
    [SerializeField] private float _cooldown;

    public override void UseAbility(GameObject owner, Transform target)
    {
        //Instantiate the laser prefab
        GameObject Laser = Instantiate(_laserPrefab, owner.transform.position, Quaternion.identity);
        Laser.transform.rotation = owner.transform.rotation;
        Laser.transform.SetParent(owner.transform);
        Destroy(Laser, _duration);

        //Pass the damage value to the laser
        PlayerLaserPrefab laserScript = Laser.GetComponent<PlayerLaserPrefab>();
        laserScript.SetDamage(_dps);


    }




}
