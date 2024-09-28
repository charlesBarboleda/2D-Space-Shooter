using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] bool _targetAllies = false;
    [SerializeField] bool _canSwitchTargets = true;
    [SerializeField] Vector3 _targetPosition;
    [SerializeField] GameObject _currentTarget;
    [SerializeField] bool prioritizePlayer = true;
    [SerializeField] GameObject _maximumPriority;
    [SerializeField] float _checkForTargetsInterval = 2f;

    private Faction _faction;
    private static List<ITargetable> _targets = new List<ITargetable>(); // List of registered targets

    void Awake()
    {
        _faction = GetComponent<Faction>();

    }

    void OnEnable()
    {
        _currentTarget = prioritizePlayer ? PlayerManager.Instance.gameObject : _maximumPriority;
        if (_canSwitchTargets)
        {
            _targetPosition = CheckForTargets(); // Immediately check for nearby targets
        }
        else
        {
            _targetPosition = _currentTarget.transform.position;
        }
        StartCoroutine(CheckForTargetsRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        if (_maximumPriority != null)
        {
            _currentTarget = _maximumPriority;
        }

        if (_currentTarget == null || !_currentTarget.activeInHierarchy ||
            (_currentTarget.GetComponent<Health>() != null && _currentTarget.GetComponent<Health>().isDead))
        {
            _targetPosition = CheckForTargets();
        }

        if (_currentTarget != null)
        {
            // Instead of using the center position, ensure the perimeter point is used
            _targetPosition = GetColliderPerimeterPoint(_currentTarget);
        }
    }

    IEnumerator CheckForTargetsRoutine()
    {
        while (true)
        {
            if (_canSwitchTargets)
            {
                _targetPosition = CheckForTargets();
            }
            yield return new WaitForSeconds(_checkForTargetsInterval);
        }
    }

    Vector3 CheckForTargets()
    {
        // If switching is not allowed and there is a current target, maintain it
        if (!_canSwitchTargets && _currentTarget != null)
        {
            return GetColliderPerimeterPoint(_currentTarget);
        }

        Vector3 bestTargetPoint = Vector3.zero;
        float closestDistance = Mathf.Infinity;
        ITargetable bestTarget = null;

        foreach (ITargetable target in _targets)
        {
            if (!target.IsAlive())
            {
                continue;
            }

            bool isValidTarget = false;

            // Determine if the target is valid based on faction rules
            if (_targetAllies && target.GetFactionType() == _faction.factionType)
            {
                isValidTarget = true; // Target allies
            }
            else if (!_targetAllies && _faction.IsHostileTo(target.GetFactionType()))
            {
                isValidTarget = true; // Target enemies
            }

            // If the target is valid, check its distance
            if (isValidTarget)
            {
                GameObject targetGameObject = (target as MonoBehaviour).gameObject;
                Vector3 perimeterPoint = GetColliderPerimeterPoint(targetGameObject);
                float distance = Vector3.Distance(transform.position, perimeterPoint);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTargetPoint = perimeterPoint;
                    bestTarget = target;
                }
            }
        }

        // Update the current target if a valid target was found
        if (bestTarget != null)
        {
            _currentTarget = (bestTarget as MonoBehaviour).gameObject; // Get GameObject from target
            return bestTargetPoint; // Return the new target's perimeter point
        }

        // Fallback: If no valid target is found, stick with player or maximum priority
        if (_currentTarget == null || !_currentTarget.activeInHierarchy)
        {
            _currentTarget = prioritizePlayer ? PlayerManager.Instance.gameObject : _maximumPriority;
        }

        // Return the player's or priority target's position if no other valid target was found
        return _currentTarget != null ? GetColliderPerimeterPoint(_currentTarget) : bestTargetPoint;
    }

    // Get a point on the perimeter of the target's Collider2D
    Vector3 GetColliderPerimeterPoint(GameObject target)
    {
        CompositeCollider2D targetCollider = target.GetComponent<CompositeCollider2D>();
        if (targetCollider != null)
        {
            // Get the vector direction from the source (this object) to the target
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            // Get the point on the perimeter of the collider by using ClosestPoint and raycasting outward
            Vector3 sourcePosition = transform.position;

            // Offset the source position slightly in the direction of the target to avoid calculating inside the collider
            Vector3 offsetPosition = sourcePosition + directionToTarget * 0.1f;

            // Find the closest point on the collider to this slightly offset source position
            Vector3 closestPoint = targetCollider.ClosestPoint(offsetPosition);

            // Debug line to see the result
            Debug.DrawLine(transform.position, closestPoint, Color.green, 1.0f);

            return closestPoint;
        }
        else
        {
            // Fallback to the target's center if there's no Collider2D
            return target.transform.position;
        }
    }


    // Register target to the list
    public static void RegisterTarget(ITargetable target)
    {
        if (!_targets.Contains(target))
        {
            _targets.Add(target);
        }
    }

    // Unregister target from the list
    public static void UnregisterTarget(ITargetable target)
    {
        if (_targets.Contains(target))
        {
            _targets.Remove(target);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 50f); // Example radius
    }

    // Public getters and setters
    public Vector3 TargetPosition { get => _targetPosition; }
    public GameObject CurrentTarget { get => _currentTarget; }
    public bool CanSwitchTargets { get => _canSwitchTargets; set => _canSwitchTargets = value; }
    public GameObject MaximumPriority { get => _maximumPriority; set => _maximumPriority = value; }
}
