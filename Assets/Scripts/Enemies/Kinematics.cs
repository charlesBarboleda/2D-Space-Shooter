using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematics : MonoBehaviour
{
    [SerializeField] protected float _aimOffset;
    [SerializeField] protected bool _shouldRotate;
    [SerializeField] protected bool _shouldMove = true;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _maxSpeed = 10f; // Maximum speed
    [SerializeField] protected float _acceleration = 1f; // Acceleration value
    [SerializeField] protected float _stopDistance;
    protected bool _rotateClockwise = false;
    protected float _cachedDistance;
    protected TargetManager _targetManager;
    protected Vector3 _cachedDirection;
    protected float _currentSpeed; // Speed that changes over time based on acceleration

    void Awake()
    {
        _targetManager = GetComponent<TargetManager>();
        _currentSpeed = 0f; // Start with zero speed
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    protected virtual void HandleMovement()
    {
        if (_shouldMove && _targetManager.CurrentTarget != null)
        {
            Movement(_targetManager.TargetPosition);
        }
    }

    protected virtual void HandleRotation()
    {
        if (_shouldRotate && _targetManager.CurrentTarget != null)
        {
            Aim(_targetManager.TargetPosition);
        }
    }

    protected virtual void OnEnable()
    {
        _rotateClockwise = Random.value > 0.5f;
    }

    protected virtual void Aim(Vector3 target)
    {
        if (target == null) return;

        // Calculate the direction to the target
        Vector3 directionToTarget = (target - transform.position).normalized;

        // Get the desired angle in degrees
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Add aim offset (if needed)
        targetAngle += _aimOffset;

        // Get the current rotation
        Quaternion currentRotation = transform.rotation;

        // Create the target rotation
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));

        // Smoothly rotate towards the target using RotateTowards
        float rotationSpeed = _speed * 100 * Time.deltaTime;

        // Apply the rotation
        transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed);
    }

    void Movement(Vector3 target)
    {
        if (target == null) return;

        // Get the target's closest point
        _cachedDistance = Vector3.Distance(transform.position, target);
        _cachedDirection = (target - transform.position).normalized;

        if (_cachedDistance > _stopDistance)
        {
            // Accelerate towards the target
            _currentSpeed = Mathf.Min(_currentSpeed + _acceleration * Time.deltaTime, _maxSpeed); // Gradually increase speed

            Vector3 finalDirection = _cachedDirection.normalized;
            transform.position += finalDirection * _currentSpeed * Time.deltaTime;
        }
        else if (_cachedDistance < _stopDistance)
        {
            // Decelerate when orbiting
            _currentSpeed = Mathf.Max(_currentSpeed - _acceleration * Time.deltaTime, 0f); // Gradually decrease speed

            Orbit(target);
            transform.position += -_cachedDirection * _currentSpeed * Time.deltaTime;
        }
    }

    protected virtual void Orbit(Vector3 target)
    {
        float rotationDirection = _rotateClockwise ? 1 : -1;

        // Maintain a consistent orbit distance to avoid drifting away
        float orbitRadius = Vector3.Distance(transform.position, target);
        float targetOrbitDistance = _stopDistance * 1.2f; // Keep a consistent orbit slightly larger than stop distance

        if (orbitRadius > targetOrbitDistance)
        {
            // Move towards the target to maintain orbit radius
            transform.position = Vector3.MoveTowards(transform.position, target, (_currentSpeed * Time.deltaTime));
        }

        // Apply rotational orbit around the target
        transform.RotateAround(target, Vector3.forward, rotationDirection * _currentSpeed * Time.deltaTime);

        // Update _cachedDirection for gizmos
        _cachedDirection = (target - transform.position).normalized;
    }

    public float Speed { get => _currentSpeed; set => _currentSpeed = value; }
    public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
}
