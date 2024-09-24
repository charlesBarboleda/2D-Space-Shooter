using UnityEngine;
using System.Collections.Generic;


public class ShieldConduit : Enemy
{
    GameObject _shieldBeam;
    [SerializeField] float _shieldRegenRate = 100f;
    [SerializeField] float _maxShield = 5000f;
    [SerializeField] float _currentShield = 5000f;
    [SerializeField] float _tetherRange = 250f;

    // Dictionary to track conduits applied to a target
    private static Dictionary<GameObject, List<ShieldConduit>> _shieldRegistry = new Dictionary<GameObject, List<ShieldConduit>>();

    protected override void Update()
    {
        base.Update();

        if (_shieldBeam == null) return;

        // If the target is out of range, remove the beam and shield
        if (Vector2.Distance(transform.position, TargetManager.CurrentTarget.transform.position) > _tetherRange)
        {
            RemoveBeam(TargetManager.CurrentTarget);
        }
    }
    protected override void Attack()
    {
        if (TargetManager.CurrentTarget == null)
        {
            Debug.Log("ShieldConduit: No target found");
            return;
        }

        // Ensure the beam is not already active for this target
        if (_shieldBeam == null)
        {
            _shieldBeam = ObjectPooler.Instance.SpawnFromPool("ThraxShieldBeam", transform.position, Quaternion.identity);
            ShieldBeamController beamController = _shieldBeam.GetComponent<ShieldBeamController>();
            beamController.OriginPoint = transform;
            beamController.Target = TargetManager.CurrentTarget;
        }

        // Apply shield to the target
        ApplyShield(TargetManager.CurrentTarget);
    }

    void ApplyShield(GameObject target)
    {
        EnemyShield enemyShield = target.GetComponentInChildren<EnemyShield>();

        if (enemyShield != null)
        {
            if (!_shieldRegistry.ContainsKey(target))
            {
                _shieldRegistry[target] = new List<ShieldConduit>();
            }

            if (!_shieldRegistry[target].Contains(this))
            {
                // Register this conduit
                _shieldRegistry[target].Add(this);

                // Incrementally add the shield values for this conduit
                enemyShield.MaxShield += _maxShield;
                enemyShield.CurrentShield += _currentShield;
                enemyShield.ShieldRegenRate += _shieldRegenRate;
            }
        }
        else
        {
            // Create a new shield if none exists
            GameObject shieldObject = ObjectPooler.Instance.SpawnFromPool("ThraxShieldLarge", target.transform.position, Quaternion.identity);
            EnemyShield newShield = shieldObject.GetComponent<EnemyShield>();

            shieldObject.transform.SetParent(target.transform);
            newShield.MaxShield = _maxShield;
            newShield.CurrentShield = _currentShield;
            newShield.ShieldRegenRate = _shieldRegenRate;

            // Adjust shield size based on target
            float sizeMultiplier = Mathf.Clamp(target.GetComponent<Collider2D>().bounds.size.magnitude * 1.2f, 1f, 2f);
            newShield.Size = sizeMultiplier;

            // Register this conduit
            _shieldRegistry[target] = new List<ShieldConduit> { this };
        }
    }

    void RemoveBeam(GameObject target)
    {
        if (_shieldBeam != null)
        {
            _shieldBeam.SetActive(false);
            _shieldBeam = null;

            // Unregister this conduit from the target's shield
            if (_shieldRegistry.ContainsKey(target))
            {
                _shieldRegistry[target].Remove(this);

                // Recalculate shield if other conduits are still applied
                RecalculateShieldValues(target);

                if (_shieldRegistry[target].Count == 0)
                {
                    // If no conduits are registered for this target, remove its shield
                    _shieldRegistry.Remove(target);
                    Destroy(target.GetComponentInChildren<EnemyShield>().gameObject);
                }
            }
        }
    }

    // Recalculate the shield values based on all active conduits for the target
    void RecalculateShieldValues(GameObject target)
    {
        EnemyShield enemyShield = target.GetComponentInChildren<EnemyShield>();

        if (enemyShield != null)
        {
            // Reset shield values before recalculating
            float totalMaxShield = 0f;
            float totalCurrentShield = 0f;
            float totalShieldRegenRate = 0f;

            // Add the values of all active conduits
            if (_shieldRegistry.ContainsKey(target))
            {
                foreach (ShieldConduit conduit in _shieldRegistry[target])
                {
                    totalMaxShield += conduit._maxShield;
                    totalCurrentShield += conduit._currentShield;
                    totalShieldRegenRate += conduit._shieldRegenRate;
                }

                // Apply recalculated shield values
                enemyShield.MaxShield = totalMaxShield;
                enemyShield.CurrentShield = Mathf.Min(totalCurrentShield, totalMaxShield); // Ensure current shield doesn't exceed max
                enemyShield.ShieldRegenRate = totalShieldRegenRate;
            }
        }
    }

}

