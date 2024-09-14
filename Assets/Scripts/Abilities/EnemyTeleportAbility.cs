using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTeleport", menuName = "EnemyAbilities/EnemyTeleport")]
public class EnemyTeleport : Ability
{
    [SerializeField] float teleportDistance = 50f;

    public override async void AbilityLogic(GameObject owner, Transform target)
    {
        await Teleport(owner, target);
    }

    async Task Teleport(GameObject owner, Transform target)
    {
        SetStatsBasedOnLevel();
        GameObject tpEffect = ObjectPooler.Instance.SpawnFromPool("EnemyTeleport", owner.transform.position, Quaternion.identity);
        owner.transform.position = target.position + new Vector3(UnityEngine.Random.Range(-teleportDistance, teleportDistance), UnityEngine.Random.Range(-teleportDistance, teleportDistance), 0);
        GameObject tpEffectPost = ObjectPooler.Instance.SpawnFromPool("EnemyTeleport", owner.transform.position, Quaternion.identity);
        await Task.Delay(2000);
        tpEffect.SetActive(false);
        tpEffectPost.SetActive(false);
    }

    void SetStatsBasedOnLevel()
    {
        teleportDistance += GameManager.Instance.Level * 0.5f;
        teleportDistance = Mathf.Min(teleportDistance, 100f);
        cooldown -= GameManager.Instance.Level * 0.1f;
        cooldown = Mathf.Max(cooldown, 1f);
    }

    public override void ResetStats()
    {
        cooldown = 10f;
        teleportDistance = 50f;
    }
}
