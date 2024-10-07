using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGravityWell", menuName = "EnemyAbilities/EnemyGravityWell")]
public class EnemyGravityWellAbility : Ability
{
    [SerializeField] float _pullStrength = 3000f;
    Vector3 _aimOffset;

    [SerializeField] string _gravityWellTag = "GravityWellSmall";

    public override void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false)
    {
        _aimOffset = new Vector3(Random.Range(-100f, 100f), Random.Range(-100, 100f), 0f);
        SetStatsBasedOnLevel();

        // Spawn gravity well at the target position
        GameObject _gravityWell = ObjectPooler.Instance.SpawnFromPool(_gravityWellTag, target.position + _aimOffset, Quaternion.identity);

        // Assign gravity well settings
        EnemyGravityWell gravityWellScript = _gravityWell.GetComponent<EnemyGravityWell>();
        gravityWellScript.Duration = duration;
        gravityWellScript.PullStrength = _pullStrength;
    }





    public override void ResetStats()
    {
        duration = 5f;
        _pullStrength = 3000f;
    }

    void SetStatsBasedOnLevel()
    {
        _pullStrength = Mathf.Max(LevelManager.Instance.CurrentLevelIndex * 150f, 3000f);
        duration = Mathf.Max(LevelManager.Instance.CurrentLevelIndex * 0.3f, 5f);
    }
}
