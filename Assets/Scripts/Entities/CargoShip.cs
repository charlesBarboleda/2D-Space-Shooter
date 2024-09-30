using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Faction))]
public class CargoShip : MonoBehaviour, ITargetable
{
    List<Vector2> _wayPoints = new List<Vector2>();
    public float speed = 5f;
    int _currentWaypointIndex = 0;
    Health _health;
    Faction _faction;
    [SerializeField] GameObject spawnAnimation;

    void Awake()
    {
        _faction = GetComponent<Faction>();
        _health = GetComponent<Health>();
    }

    void Update()
    {
        if (_currentWaypointIndex < _wayPoints.Count)
        {
            // Move towards the current waypoint
            Vector2 target = _wayPoints[_currentWaypointIndex];
            Vector2 direction = target - (Vector2)transform.position;
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Rotate the ship to face the direction of movement
            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + 90); // Adjust the angle if needed to match your ship's sprite orientation
            }

            // Check if the ship has reached the waypoint
            if ((Vector2)transform.position == target)
            {
                _currentWaypointIndex++;
            }
        }
        else
        {
            // Path completed, teleport away or perform other logic
            TeleportAway();
        }
    }

    void OnDisable()
    {
        TargetManager.UnregisterTarget(this);
    }

    void OnEnable()
    {
        StartCoroutine(StartSpawnAnimationWithDelay());
    }

    IEnumerator StartSpawnAnimationWithDelay()
    {
        TargetManager.RegisterTarget(this);
        yield return new WaitForSeconds(0.1f);
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
    }


    public void TeleportAway()
    {
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
        gameObject.SetActive(false);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool IsAlive()
    {
        return !_health.isDead;
    }

    public FactionType GetFactionType()
    {
        return _faction.factionType;
    }
    public void SetWaypoints(List<Vector2> waypoints)
    {
        _wayPoints = waypoints;
    }

    public Health Health { get => _health; }
}
