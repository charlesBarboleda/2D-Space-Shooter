using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyMissileBarrage", menuName = "EnemyAbilities/EnemyMissileBarrage")]
public class EnemyMissileBarrageAbility : Ability
{
    public float damage = 100f;
    public float missileSpeed = 20f;
    public float turnSpeed = 5f;
    public float radius = 10f;

    public override void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false)
    {

        owner.GetComponent<MonoBehaviour>().StartCoroutine(MissileBarrage(target.position, owner));

    }

    public override void ResetStats()
    {
        cooldown = 20f;
        damage = 50f;
        missileSpeed = 80f;
        turnSpeed = 5f;
        radius = 10f;
        isUnlocked = true;

    }

    IEnumerator MissileBarrage(Vector3 target, GameObject owner)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject missile = ObjectPooler.Instance.SpawnFromPool("EnemyMissile", owner.transform.position, Quaternion.identity);
            EnemyMissile missileScript = missile.GetComponent<EnemyMissile>();
            missileScript.target = new Vector3(target.x + Random.Range(-20f, 20f), target.y + Random.Range(-20f, 20f), 0f);
            missileScript.damage = damage;
            missileScript.maxSpeed = missileSpeed;
            missileScript.turnSpeed = turnSpeed;
            missileScript.radius = radius;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
