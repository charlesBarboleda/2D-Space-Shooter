using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematics : MonoBehaviour
{
    [SerializeField] float _aimOffset;
    [SerializeField] bool _shouldRotate;
    [SerializeField] float _speed;
    [SerializeField] float _stopDistance;
    [SerializeField] float separationRadius = 2f;  // Radius for separation behavior
    [SerializeField] float separationWeight = 1.5f;   // Strength of the repulsion force
    [SerializeField] float maxSeparationForce = 0.5f;
    bool _rotateClockwise = false;
    float _cachedDistance;
    TargetManager _targetManager;
    Vector3 _cachedDirection;
    // Start is called before the first frame update
    void Awake()
    {
        _targetManager = GetComponent<TargetManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        if (_targetManager.CurrentTarget != null)
        {
            Movement(_targetManager.CurrentTarget);
        }
    }

    void HandleRotation()
    {
        if (_shouldRotate && _targetManager.CurrentTarget != null)
        {
            Aim(_targetManager.CurrentTarget);
        }
    }

    void OnEnable()
    {
        if (Random.value < 0.5) _rotateClockwise = true;
        else _rotateClockwise = false;
    }

    protected virtual void Aim(Transform target)
    {
        if (target == null) return;

        // Get the target's closest point using colliders
        Collider2D targetCollider = target.GetComponent<CompositeCollider2D>();
        if (targetCollider != null)
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
            Vector3 direction = closestPoint - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimOffset));
        }
        else
        {
            // Fallback if no collider is found
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimOffset));
        }
    }
    void Movement(Transform target)
    {
        if (target == null) return;

        // Get the target's closest point using colliders
        Collider2D targetCollider = target.GetComponent<CompositeCollider2D>();
        if (targetCollider != null)
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
            _cachedDirection = (closestPoint - transform.position).normalized;
            _cachedDistance = Vector3.Distance(transform.position, closestPoint);
        }
        else
        {
            // Fallback if no collider is found
            _cachedDirection = (transform.position - target.position).normalized;
            _cachedDistance = Vector3.Distance(transform.position, target.position);

        }

        // Get separation force to avoid collisions with other enemies
        Vector3 separationForce = CalculateSeparation();
        Debug.Log(gameObject.name + " has separation force of " + separationForce);

        if (_cachedDistance > _stopDistance)
        {
            Vector3 finalDirection = (_cachedDirection + separationForce).normalized;
            transform.position += finalDirection * _speed * Time.deltaTime;
            Debug.Log(gameObject.name + " is moving towards target" + target.name);
        }
        else
        {

            Orbit(target);
            Debug.Log(gameObject.name + " is orbiting target" + target.name);
        }


    }
    void Orbit(Transform target)
    {
        float rotationDirection = _rotateClockwise ? 1 : -1;

        // Maintain a consistent orbit distance to avoid drifting away
        float orbitRadius = Vector3.Distance(transform.position, target.position);
        float targetOrbitDistance = _stopDistance * 1.2f; // Keep a consistent orbit slightly larger than stop distance

        if (orbitRadius > targetOrbitDistance)
        {
            // Move towards the target to maintain orbit radius
            transform.position = Vector3.MoveTowards(transform.position, target.position, (_speed * Time.deltaTime));
        }

        // Apply rotational orbit around the target
        transform.RotateAround(target.position, Vector3.forward, rotationDirection * _speed * Time.deltaTime);

        // Update _cachedDirection for gizmos
        _cachedDirection = (target.position - transform.position).normalized;
    }


    Vector3 CalculateSeparation()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Syndicates", "ThraxArmada", "CrimsonFleet");
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius, enemyLayer);
        Vector3 separationForce = Vector3.zero;
        int count = 0;

        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy != null && enemy.transform != transform)
            {
                Vector3 directionToEnemy = transform.position - enemy.transform.position;
                float distance = directionToEnemy.magnitude;
                separationForce += directionToEnemy.normalized / Mathf.Max(distance, 0.1f);  // Avoid division by zero
                count++;
            }
        }

        if (count > 0)
        {
            separationForce /= count; // Average the forces from nearby enemies
            separationForce = Vector3.ClampMagnitude(separationForce * separationWeight, maxSeparationForce);  // Clamp the force
        }

        return separationForce;
    }
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _cachedDirection * 5);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (_cachedDirection + CalculateSeparation()).normalized * 20);
        }
    }
    public float Speed { get => _speed; set => _speed = value; }
    public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
}
