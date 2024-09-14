using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] float _aimRange = 20f;
    [SerializeField] float _attackCooldown = 1f;
    float _elapsedCooldown;

    // Update is called once per frame
    void Update()
    {
        HandleCooldown();
    }

    void HandleCooldown()
    {
        if (_elapsedCooldown > 0)
        {
            _elapsedCooldown -= Time.deltaTime;
        }
    }


    public bool IsTargetInRange(Transform target)
    {
        // Calculate edge distance
        if (target.TryGetComponent<Collider2D>(out Collider2D targetCollider))
        {
            Vector2 closestPointToTarget = targetCollider.ClosestPoint(transform.position);
            float distanceToTargetEdge = Vector2.Distance(transform.position, closestPointToTarget);
            return distanceToTargetEdge < _aimRange;
        }
        return false;
    }

    public float AimRange { get => _aimRange; set => _aimRange = value; }
    public float AttackCooldown { get => _attackCooldown; set => _attackCooldown = value; }
    public float ElapsedCooldown { get => _elapsedCooldown; set => _elapsedCooldown = value; }
}
