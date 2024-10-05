using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLightningExplosionAbility", menuName = "EnemyAbilities/EnemyLightningExplosionAbility", order = 1)]
public class EnemyLightningExplosionAbility : Ability
{
    GameObject _lightningExplosion;
    [SerializeField] float _damagePerSecond = 10f;



    public override void AbilityLogic(GameObject owner, Transform target)
    {
        _lightningExplosion = ObjectPooler.Instance.SpawnFromPool("LightningExplosion", target.position, Quaternion.identity);
        LightningExplosion lightningExplosionScript = _lightningExplosion.GetComponent<LightningExplosion>();
        lightningExplosionScript.damagePerSecond = _damagePerSecond;
        lightningExplosionScript.duration = duration;
        // Start the channel with an x and y offset of 50
        Vector3 finalTarget = new Vector3(target.position.x + Random.Range(20, 50), target.position.y + Random.Range(20, 50), target.position.z);
        lightningExplosionScript.StartChannel(owner.transform, finalTarget);

    }

    public override void ResetStats()
    {
        _damagePerSecond = 10f;
        duration = 10f;
    }


}
