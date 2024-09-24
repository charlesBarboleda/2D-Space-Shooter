using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] float _checkForTargetsInterval = 2f; // Set interval to 2 seconds
    [SerializeField] float _checkForTargetsRadius = 50f; // Set radius to 50 units
    [SerializeField] bool _targetAllies; // Flag to target allies or not
    [SerializeField] Vector3 _targetPosition;
    [SerializeField] GameObject _currentTarget;

    Faction _faction;

    void Awake()
    {
        _faction = GetComponent<Faction>();
        StartCoroutine(CheckForTargetsRoutine());
        _currentTarget = PlayerManager.Instance.gameObject;
        _targetPosition = PlayerManager.Instance.transform.position;
    }

    void Update()
    {
        // Check if _currentTarget is null first
        if (_currentTarget == null || !_currentTarget.activeInHierarchy ||
            (_currentTarget.GetComponent<Health>() != null && _currentTarget.GetComponent<Health>().isDead))
        {
            _targetPosition = CheckForTargets(); // Continuously check for new targets
        }

        // Update target position based on current target if available
        if (_currentTarget != null)
        {
            _targetPosition = _currentTarget.transform.position;
        }
    }

    void OnEnable()
    {
        StopAllCoroutines(); // Ensure no duplicate coroutines
        StartCoroutine(CheckForTargetsRoutine());
        _currentTarget = PlayerManager.Instance.gameObject;
        _targetPosition = PlayerManager.Instance.transform.position;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator CheckForTargetsRoutine()
    {
        while (true)
        {
            _targetPosition = CheckForTargets();
            yield return new WaitForSeconds(_checkForTargetsInterval);
        }
    }

    Vector3 CheckForTargets()
    {
        LayerMask targetLayerMask = LayerMask.GetMask("Syndicates", "ThraxArmada", "CrimsonFleet", "Player");
        Transform playerTransform = PlayerManager.Instance.transform;
        Collider2D enemyCollider = GetComponent<Collider2D>();

        Vector3 bestTargetPoint = Vector3.zero;
        float closestDistance = Mathf.Infinity;
        GameObject previousTarget = _currentTarget; // Keep a reference to the previous target

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, _checkForTargetsRadius, targetLayerMask);

        foreach (Collider2D targetCollider in hitTargets)
        {
            if (targetCollider.gameObject == gameObject) continue; // Skip self

            Faction targetFaction = targetCollider.GetComponent<Faction>();
            Health targetHealth = targetCollider.GetComponent<Health>();

            // Skip dead targets
            if (targetHealth != null && targetHealth.isDead) continue;

            bool isValidTarget = false;

            // Determine if the target is valid based on _targetAllies flag
            if (_targetAllies && targetFaction != null && targetFaction.factionType == _faction.factionType)
            {
                isValidTarget = true; // Ally targeting
            }
            else if (!_targetAllies && targetFaction != null && _faction.IsHostileTo(targetFaction.factionType))
            {
                isValidTarget = true; // Hostile targeting
            }

            // If targeting allies, ensure player is not targeted
            if (_targetAllies && targetFaction != null && targetFaction.factionType == FactionType.Player)
            {
                isValidTarget = false; // Prevent targeting the player
            }

            if (isValidTarget)
            {
                Vector3 closestPointOnTarget = targetCollider.ClosestPoint(transform.position);
                float distance = Vector3.Distance(transform.position, closestPointOnTarget);

                Debug.Log($"Valid Target: {targetCollider.gameObject.name}, Distance: {distance}");

                // Only update current target if it's closer
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTargetPoint = closestPointOnTarget;
                    _currentTarget = targetCollider.gameObject; // Update current target
                }
            }
        }

        // If no valid targets are found, fallback logic
        if (_currentTarget == null)
        {
            // Only consider previous target if not targeting allies
            if (!_targetAllies && previousTarget != null)
            {
                // Use the previous target if it was a valid target
                _currentTarget = previousTarget;
            }
            // If targeting allies, do nothing; do not default to player
        }

        // Ensure we always return the target position
        return bestTargetPoint;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _checkForTargetsRadius);
    }

    /// <summary>
    /// Getters and Setters
    /// </summary>
    public Vector3 TargetPosition { get => _targetPosition; }
    public GameObject CurrentTarget { get => _currentTarget; }
}
