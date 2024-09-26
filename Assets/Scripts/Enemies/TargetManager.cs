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
        StartCoroutine(CheckForTargetsRoutine());
    }

    void OnEnable()
    {
        _currentTarget = prioritizePlayer ? PlayerManager.Instance.gameObject : _maximumPriority;
        _targetPosition = _currentTarget.transform.position;
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
            _targetPosition = _currentTarget.transform.position;
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
            Debug.Log($"Maintaining current target: {_currentTarget.transform.position}");
            return _currentTarget.transform.position;
        }

        Vector3 bestTargetPoint = Vector3.zero;
        float closestDistance = Mathf.Infinity;
        ITargetable bestTarget = null;

        foreach (ITargetable target in _targets)
        {
            Debug.Log($"Checking target: {target}");

            if (!target.IsAlive())
            {
                Debug.Log($"Skipping target {target} - not alive");
                continue;
            }

            bool isValidTarget = false;

            // Determine if the target is valid based on faction rules
            if (_targetAllies && target.GetFactionType() == _faction.factionType)
            {
                isValidTarget = true; // Target allies
                Debug.Log($"Target {target} is an ally.");
            }
            else if (!_targetAllies && _faction.IsHostileTo(target.GetFactionType()))
            {
                isValidTarget = true; // Target enemies
                Debug.Log($"Target {target} is an enemy.");
            }
            else
            {
                Debug.Log($"Target {target} is not valid. Is Ally: {_targetAllies}, Faction Type: {target.GetFactionType()}");
            }

            // If the target is valid, check its distance
            if (isValidTarget)
            {
                float distance = Vector3.Distance(transform.position, target.GetPosition());
                Debug.Log($"Evaluating target {target} at distance {distance}");

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTargetPoint = target.GetPosition();
                    bestTarget = target;
                    Debug.Log($"New closest target: {target} at distance {closestDistance}");
                }
            }
        }

        // Update the current target if a valid target was found
        if (bestTarget != null)
        {
            _currentTarget = (bestTarget as MonoBehaviour).gameObject; // Get GameObject from target
            Debug.Log($"Current target updated to: {_currentTarget.transform.position}");
            return bestTargetPoint; // Return the new target's position
        }

        // Fallback: If no valid target is found, stick with player or maximum priority
        if (_currentTarget == null || !_currentTarget.activeInHierarchy)
        {
            _currentTarget = prioritizePlayer ? PlayerManager.Instance.gameObject : _maximumPriority;
            Debug.Log($"Fallback to target: {_currentTarget?.transform.position}");
        }
        else
        {
            Debug.Log("No valid targets found, retaining current target.");
        }

        // Return the player's or priority target's position if no other valid target was found
        return _currentTarget != null ? _currentTarget.transform.position : bestTargetPoint;
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
