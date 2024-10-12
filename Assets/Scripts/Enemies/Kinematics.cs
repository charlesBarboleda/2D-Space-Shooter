using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kinematics : MonoBehaviour
{
    [SerializeField] protected float _aimOffset;
    [SerializeField] protected bool _shouldRotate;
    [SerializeField] protected bool _shouldMove = true;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _stopDistance;
    protected bool _rotateClockwise = false;
    protected float _cachedDistance;
    protected TargetManager _targetManager;
    protected Vector3 _cachedDirection;
    protected NavMeshAgent agent;
    public bool outOfBounds = false;


    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    IEnumerator InitializeAgent()
    {
        // Wait for one frame to ensure everything is set up
        yield return null;

        if (agent != null)
        {
            agent.speed = _speed;
            agent.stoppingDistance = _stopDistance;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }
    protected virtual void OnEnable()
    {
        _rotateClockwise = Random.value > 0.5f;
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(InitializeAgent());
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing!");
            return;
        }
        _targetManager = GetComponent<TargetManager>();
        if (_targetManager == null)
        {
            Debug.LogError("TargetManager component missing!");
            return;
        }

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3 position = transform.position;
        position.z = 0;
        transform.position = position;

        if (outOfBounds) HandleOutOfBounds();
        else
        {
            HandleMovement();
            HandleRotation();
        }
    }
    protected virtual void HandleOutOfBounds()
    {
        if (_shouldMove)
        {
            Movement(new Vector3(0, 0, 0));
            Aim(new Vector3(0, 0, 0));
        }
    }

    protected virtual void HandleMovement()
    {
        if (_shouldMove)
        {
            agent.SetDestination(_targetManager.TargetPosition);
        }


        // if (_shouldMove && _targetManager.CurrentTarget != null)
        // {
        //     Movement(_targetManager.TargetPosition);
        // }
    }

    protected virtual void HandleRotation()
    {
        if (_shouldRotate && _targetManager.CurrentTarget != null)
        {
            Aim(_targetManager.TargetPosition);
        }
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
            // Move towards the target at max speed
            Vector3 finalDirection = _cachedDirection.normalized;
            transform.position += finalDirection * Speed * Time.deltaTime;
        }
        else if (_cachedDistance < _stopDistance)
        {

            Orbit(target);
            transform.position += -_cachedDirection * Speed / 6 * Time.deltaTime;
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
            transform.position = Vector3.MoveTowards(transform.position, target, (Speed * Time.deltaTime));
        }

        // Apply rotational orbit around the target
        transform.RotateAround(target, Vector3.forward, rotationDirection * Speed * Time.deltaTime);

        // Update _cachedDirection for gizmos
        _cachedDirection = (target - transform.position).normalized;
    }

    public float Speed { get => agent.speed; set => agent.speed = value; }
    public float StopDistance { get => agent.stoppingDistance; set => agent.stoppingDistance = value; }
    public bool ShouldMove { get => _shouldMove; set => _shouldMove = value; }
    public bool ShouldRotate { get => _shouldRotate; set => _shouldRotate = value; }
}
