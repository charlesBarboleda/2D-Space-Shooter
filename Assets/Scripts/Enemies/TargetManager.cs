using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] float _checkForTargetsInterval = 3f;
    [SerializeField] float _checkForTargetsRadius = 50f;
    Faction _faction;

    Transform _currentTarget;

    void Awake()
    {
        _faction = GetComponent<Faction>();
    }

    void Update()
    {
        if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
        {
            Debug.Log(gameObject.name + " has no targets... Checking for targets ");
            _currentTarget = CheckForTargets();
            _checkForTargetsRadius += 10f;
        }
        if (_currentTarget != null)
        {

            Debug.Log(gameObject.name + " is targeting " + _currentTarget.name);
        }
    }

    void OnEnable()
    {
        StartCoroutine(CheckForTargetsRoutine());
    }

    void OnDisable()
    {
        StopCoroutine(CheckForTargetsRoutine());
    }
    IEnumerator CheckForTargetsRoutine()
    {
        _currentTarget = CheckForTargets();
        yield return new WaitForSeconds(_checkForTargetsInterval);

    }

    Transform CheckForTargets()
    {

        LayerMask targetLayerMask = LayerMask.GetMask("Syndicates", "ThraxArmada", "CrimsonFleet", "Player");
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, _checkForTargetsRadius, targetLayerMask);

        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D targetCollider in hitTargets)
        {

            Faction targetFaction = targetCollider.GetComponent<Faction>();
            if (targetFaction != null && _faction.IsHostileTo(targetFaction.factionType))
            {
                float distance = Vector3.Distance(transform.position, targetCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTarget = targetCollider.transform;


                }
            }
        }


        return bestTarget ?? PlayerManager.Instance.transform;  // Fallback to player if no other targets
    }

    /// <summary>
    /// Getters and Setters
    /// </summary>
    public Transform CurrentTarget { get => _currentTarget; }
}
