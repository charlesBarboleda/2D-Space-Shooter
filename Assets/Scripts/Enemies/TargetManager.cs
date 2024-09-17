using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] float _checkForTargetsInterval = 0.5f;
    [SerializeField] float _checkForTargetsRadius = 25f;
    Faction _faction;

    [SerializeField] Vector3 _targetPosition;
    [SerializeField] GameObject _currentTarget;

    void Awake()
    {
        _checkForTargetsInterval = 2f;
        _checkForTargetsRadius = 50f;
        _faction = GetComponent<Faction>();
        StartCoroutine(CheckForTargetsRoutine());
        _currentTarget = PlayerManager.Instance.gameObject;
        _targetPosition = PlayerManager.Instance.transform.position;

    }

    void Update()
    {
        // Check if _currentTarget is null first, so other checks aren't performed on null
        if (_currentTarget == null || !_currentTarget.activeInHierarchy ||
            (_currentTarget.GetComponent<Health>() != null && _currentTarget.GetComponent<Health>().isDead))
        {
            _currentTarget = PlayerManager.Instance.gameObject;
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
        StartCoroutine(CheckForTargetsRoutine());
        _currentTarget = PlayerManager.Instance.gameObject;
        _targetPosition = PlayerManager.Instance.transform.position;
    }

    void OnDisable()
    {
        StopCoroutine(CheckForTargetsRoutine());
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
        Collider2D playerCollider = playerTransform.GetComponent<Collider2D>();
        Collider2D enemyCollider = GetComponent<Collider2D>();

        // Default to player if no other target is found
        Vector3 bestTargetPoint = playerCollider.ClosestPoint(transform.position);
        float closestDistance = Vector3.Distance(enemyCollider.ClosestPoint(playerTransform.position), bestTargetPoint);

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, _checkForTargetsRadius, targetLayerMask);

        foreach (Collider2D targetCollider in hitTargets)
        {
            if (targetCollider.gameObject == gameObject) continue;

            Faction targetFaction = targetCollider.GetComponent<Faction>();
            Health targetHealth = targetCollider.GetComponent<Health>();

            if (targetHealth != null && targetHealth.isDead) continue;

            if (targetFaction != null && _faction.IsHostileTo(targetFaction.factionType))
            {
                Vector3 closestPointToTarget = enemyCollider.ClosestPoint(targetCollider.bounds.center);
                Vector3 closestPointOnTarget = targetCollider.ClosestPoint(transform.position);
                float distance = Vector3.Distance(closestPointToTarget, closestPointOnTarget);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTargetPoint = closestPointOnTarget;
                    _currentTarget = targetCollider.gameObject;
                    Debug.Log($"New target acquired: {_currentTarget.name}, Position: {bestTargetPoint}");
                }
            }
        }

        // Return the best target position found
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
