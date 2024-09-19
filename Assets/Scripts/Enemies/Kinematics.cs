using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematics : MonoBehaviour
{
    [SerializeField] protected float _aimOffset;
    [SerializeField] protected bool _shouldRotate;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _stopDistance;
    protected bool _rotateClockwise = false;
    protected float _cachedDistance;
    protected TargetManager _targetManager;
    protected Vector3 _cachedDirection;
    // Start is called before the first frame update
    void Awake()
    {
        _targetManager = GetComponent<TargetManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    protected virtual void HandleMovement()
    {
        if (_targetManager.CurrentTarget != null)
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
        // The factor for smooth rotation
        float rotationSpeed = _speed * 100 * Time.deltaTime;

        // Apply the rotation
        transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed);

    }

    void Movement(Vector3 target)
    {
        if (target == null) return;

        // Get the target's closest point
        if (target != null)
        {
            _cachedDistance = Vector3.Distance(transform.position, target);
            _cachedDirection = (target - transform.position).normalized;
        }


        if (_cachedDistance > _stopDistance)
        {
            Vector3 finalDirection = _cachedDirection.normalized;
            transform.position += finalDirection * _speed * Time.deltaTime;

        }
        else if (_cachedDistance < _stopDistance)
        {
            transform.position += -_cachedDirection * _speed * Time.deltaTime;
            Orbit(target);
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
            transform.position = Vector3.MoveTowards(transform.position, target, (_speed * Time.deltaTime));
        }

        // Apply rotational orbit around the target
        transform.RotateAround(target, Vector3.forward, rotationDirection * _speed * Time.deltaTime);

        // Update _cachedDirection for gizmos
        _cachedDirection = (target - transform.position).normalized;
    }

    public float Speed { get => _speed; set => _speed = value; }
    public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
}
