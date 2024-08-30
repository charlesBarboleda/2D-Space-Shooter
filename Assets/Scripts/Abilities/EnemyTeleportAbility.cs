using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        GameObject tpEffect = ObjectPooler.Instance.SpawnFromPool("EnemyTeleport", owner.transform.position, Quaternion.identity);
        owner.transform.position = target.position + new Vector3(Random.Range(-teleportDistance, teleportDistance), Random.Range(-teleportDistance, teleportDistance), 0);
        Debug.Log("Teleported to " + target.position + new Vector3(Random.Range(-teleportDistance, teleportDistance), Random.Range(-teleportDistance, teleportDistance), 0));
        GameObject tpEffectPost = ObjectPooler.Instance.SpawnFromPool("EnemyTeleport", owner.transform.position, Quaternion.identity);
        await Task.Delay(1000);
        tpEffect.SetActive(false);
        tpEffectPost.SetActive(false);
    }

    public override void ResetStats()
    {
        teleportDistance = 65f;
        cooldown = 10f;
    }
}
