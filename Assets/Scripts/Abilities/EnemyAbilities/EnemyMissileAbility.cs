using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyMissile", menuName = "EnemyAbilities/EnemyMissile")]
public class EnemyMissileAbility : Ability
{
    public float damage = 100f;
    public float missileSpeed = 20f;
    public float turnSpeed = 5f;
    public float radius = 20f;

    public override void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false)
    {
        GameObject missile = ObjectPooler.Instance.SpawnFromPool("EnemyMissile", owner.transform.position, Quaternion.identity);
        EnemyMissile missileScript = missile.GetComponent<EnemyMissile>();
        missileScript.target = new Vector3(target.position.x + Random.Range(-30f, 30f), target.position.y + Random.Range(-20f, 20f), 0f);
        missileScript.damage = damage;
        missileScript.maxSpeed = missileSpeed;
        missileScript.turnSpeed = turnSpeed;
        missileScript.radius = radius;

    }

    public override void ResetStats()
    {
        cooldown = 5f;
        damage = 100f;
        missileSpeed = 30f;
        turnSpeed = 5f;
        radius = 20f;
        isUnlocked = true;

    }
}
