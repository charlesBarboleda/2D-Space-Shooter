using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGravityWell", menuName = "EnemyAbilities/EnemyGravityWell")]
public class EnemyGravityWellAbility : Ability
{
    [SerializeField] float _duration = 5f;
    [SerializeField] float _pullStrength = 3000f;
    Vector3 _aimOffset;

    [SerializeField] string _gravityWellTag = "GravityWellSmall";

    public override void AbilityLogic(GameObject owner, Transform target)
    {
        _aimOffset = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f);
        SetStatsBasedOnLevel();

        // Spawn gravity well at the target position
        GameObject _gravityWell = ObjectPooler.Instance.SpawnFromPool(_gravityWellTag, target.position + _aimOffset, Quaternion.identity);

        // Assign gravity well settings
        EnemyGravityWell gravityWellScript = _gravityWell.GetComponent<EnemyGravityWell>();
        gravityWellScript.Duration = _duration;
        gravityWellScript.PullStrength = _pullStrength;
    }





    public override void ResetStats()
    {
        _duration = 5f;
        _pullStrength = 3000f;
    }

    void SetStatsBasedOnLevel()
    {
        _pullStrength = Mathf.Max(LevelManager.Instance.CurrentLevelIndex * 150f, 3000f);
        _duration = Mathf.Max(LevelManager.Instance.CurrentLevelIndex * 0.3f, 5f);
    }
}
