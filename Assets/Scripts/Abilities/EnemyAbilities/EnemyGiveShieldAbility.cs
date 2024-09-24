using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGiveShield", menuName = "EnemyAbilities/EnemyGiveShield")]
public class EnemyGiveShieldAbility : Ability
{
    [SerializeField] float _shieldRegenRate = 100f;
    [SerializeField] float _maxShield = 5000f;
    [SerializeField] float _currentShield = 5000f;

    public override void AbilityLogic(GameObject owner, Transform target)
    {
        SetStatsBasedOnLevel();
        // Spawn shield at the target position

        GameObject _shield = ObjectPooler.Instance.SpawnFromPool("ThraxShield", target.position, Quaternion.identity);
        _shield.transform.SetParent(target);
        EnemyShield enemyShieldScript = _shield.GetComponent<EnemyShield>();
        enemyShieldScript.Size = target.localScale.x * 100f;
        enemyShieldScript.MaxShield = _maxShield;
        enemyShieldScript.CurrentShield = _currentShield;
        GameObject _beam = ObjectPooler.Instance.SpawnFromPool("ThraxShieldBeam", owner.transform.position, Quaternion.identity);
        ShieldBeamController beamController = _beam.GetComponent<ShieldBeamController>();
        beamController.OriginPoint = owner.transform;
        beamController.EndPoint = target;
        // Assign shield settings
        enemyShieldScript.ShieldRegenRate += _shieldRegenRate;
    }





    public override void ResetStats()
    {
    }

    void SetStatsBasedOnLevel()
    {
    }
}
