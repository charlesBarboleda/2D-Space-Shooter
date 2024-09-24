using System.Collections;
using System.Collections.Generic;
using AssetUsageDetectorNamespace;
using UnityEngine;

public class ShieldConduit : Enemy
{
    GameObject _shieldBeam;
    GameObject _shield;
    EnemyShield _existingShield;
    [SerializeField] float _tetherRange = 250f;
    [SerializeField] float _shieldRegenRate = 100f;
    [SerializeField] float _maxShield = 5000f;
    [SerializeField] float _currentShield = 5000f;

    protected override void Attack()
    {
        if (TargetManager.CurrentTarget == null) Debug.Log("ShieldConduit: No target found");

        _existingShield = TargetManager.CurrentTarget.GetComponentInChildren<EnemyShield>();
        _shieldBeam = ObjectPooler.Instance.SpawnFromPool("ThraxShieldBeam", transform.position, Quaternion.identity);
        Debug.Log(_shieldBeam.name + " spawned");

        if (_existingShield != null)
        {
            TargetManager.CurrentTarget.GetComponent<Health>().isDead = true;
            _existingShield.MaxShield += _maxShield;
            _existingShield.CurrentShield += _currentShield;
            _existingShield.ShieldRegenRate += _shieldRegenRate;
            return;
        }
        else
        {
            TargetManager.CurrentTarget.GetComponent<Health>().isDead = true;
            _shield = ObjectPooler.Instance.SpawnFromPool("ThraxShieldLarge", TargetManager.CurrentTarget.transform.position, Quaternion.identity);
            EnemyShield enemyShieldScript = _shield.GetComponent<EnemyShield>();
            enemyShieldScript.transform.SetParent(TargetManager.CurrentTarget.transform);

            // Get the size of the ship's collider
            Collider2D shipCollider = TargetManager.CurrentTarget.GetComponent<Collider2D>();

            // Calculate the shield size based on the ship size
            float shipSizeX = shipCollider.bounds.size.x;
            float shipSizeY = shipCollider.bounds.size.y;

            // Define a base size multiplier to scale the shield
            float baseSizeMultiplier = 1.2f; // Adjust this to your needs

            // Calculate shield size using a multiplier, and clamp it to prevent it from being too large or too small
            float shieldSize = Mathf.Clamp(Mathf.Max(shipSizeX, shipSizeY) * baseSizeMultiplier, 1f, 2f); // 5f is min, 50f is max

            // Apply the shield size
            enemyShieldScript.Size = shieldSize;

            // Initialize the shield stats
            enemyShieldScript.MaxShield += _maxShield;
            enemyShieldScript.CurrentShield += _currentShield;
            enemyShieldScript.ShieldRegenRate += _shieldRegenRate;
        }
    }


    protected override void Update()
    {
        base.Update();
        ShieldBeamController beamController = _shieldBeam.GetComponent<ShieldBeamController>();
        beamController.OriginPoint = transform;
        beamController.EndPoint = TargetManager.CurrentTarget.transform;
        if (Vector2.Distance(transform.position, TargetManager.CurrentTarget.transform.position) > _tetherRange)
        {
            TargetManager.CurrentTarget.GetComponent<Health>().isDead = false;
            _shieldBeam.SetActive(false);
            _existingShield.gameObject.SetActive(false);
            _existingShield.MaxShield -= _maxShield;
            _existingShield.CurrentShield -= _currentShield;
            _existingShield.ShieldRegenRate -= _shieldRegenRate;
            Debug.Log("Deactivating Shield Beam");
        }
        else
        {
            _shieldBeam.SetActive(true);
            _existingShield.gameObject.SetActive(true);
        }
        Debug.Log("Shield Beam Origin: " + beamController.OriginPoint.position);
        Debug.Log("Shield Beam End: " + beamController.EndPoint.position);
    }

}
