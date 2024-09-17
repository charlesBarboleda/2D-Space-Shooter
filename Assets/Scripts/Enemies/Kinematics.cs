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
            Movement(_targetManager.TargetPosition);
        }
    }

    void HandleRotation()
    {
        if (_shouldRotate && _targetManager.CurrentTarget != null)
        {
            Aim(_targetManager.TargetPosition);
        }
    }
    void OnEnable()
    {
        _rotateClockwise = Random.value > 0.5f;
    }

    void Aim(Vector3 target)
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
        else
        {

            Orbit(target);

        }


    }
    void Orbit(Vector3 target)
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

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _cachedDirection * 5);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _cachedDirection.normalized * 20);
        }
    }
    public float Speed { get => _speed; set => _speed = value; }
    public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
}
