using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aoiti.Pathfinding;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTeleport", menuName = "EnemyAbilities/EnemyTeleport")]
public class EnemyTeleport : Ability
{
    [SerializeField] float teleportDistance = 5f;

    public override async void AbilityLogic(GameObject owner, Transform target)
    {
        await Teleport(owner, target);
    }

    public async Task Teleport(GameObject owner, Transform target)
    {
        SetStatsBasedOnLevel();
        GameObject tpEffect = ObjectPooler.Instance.SpawnFromPool("EnemyTeleport", owner.transform.position, Quaternion.identity);
        owner.transform.position = target.position + new Vector3(UnityEngine.Random.Range(-teleportDistance, teleportDistance), UnityEngine.Random.Range(-teleportDistance, teleportDistance), 0);
        GameObject tpEffectPost = ObjectPooler.Instance.SpawnFromPool("EnemyTeleport", owner.transform.position, Quaternion.identity);
        await Task.Delay(2000);
        tpEffect.SetActive(false);
        tpEffectPost.SetActive(false);
    }

    public void SetStatsBasedOnLevel()
    {
        teleportDistance += GameManager.Instance.Level() * 0.5f;
        teleportDistance = Mathf.Min(teleportDistance, 100f);
        Debug.Log("Teleport distance: " + teleportDistance);
        cooldown -= GameManager.Instance.Level() * 0.1f;
        cooldown = Mathf.Max(cooldown, 3f);
    }

    public override void ResetStats()
    {
        teleportDistance = 50f;
        cooldown = 10f;
    }
}
