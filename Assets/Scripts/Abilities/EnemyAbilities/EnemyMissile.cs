using System.Collections;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    public float damage = 100f;
    public float maxSpeed = 60f; // The maximum speed the missile can reach
    public float acceleration = 60f; // How fast the missile accelerates to its max speed
    public float turnSpeed = 2f; // How fast the missile turns towards the target
    public Vector3 target;
    public float radius = 20f;

    private float currentSpeed = 0f; // Start with 0 speed and accelerate
    private Vector3 launchDirection; // The initial launch direction

    void Start()
    {
        // Randomize initial launch direction slightly
        launchDirection = (target - transform.position).normalized;
        launchDirection = Quaternion.Euler(0, 0, Random.Range(-10f, 10f)) * launchDirection;
        transform.up = launchDirection; // Start by facing the launch direction
    }

    void Update()
    {
        // Accelerate towards max speed
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

        // Calculate the direction towards the target
        Vector3 directionToTarget = (target - transform.position).normalized;

        // Gradually rotate the missile towards the target direction
        Vector3 newDirection = Vector3.RotateTowards(transform.up, directionToTarget, turnSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, newDirection);

        // Move missile forward based on current speed
        transform.position += transform.up * currentSpeed * Time.deltaTime;

        // Check if missile is close enough to explode
        if (Vector2.Distance(transform.position, target) <= 0.1f)
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        LayerMask _damageLayerMask = LayerMask.GetMask("Player", "Asteroid", "CrimsonFleet", "ThraxArmada");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, _damageLayerMask);
        ObjectPooler.Instance.SpawnFromPool("MissileExplosion", transform.position, Quaternion.identity);

        // Apply damage to all nearby objects
        foreach (var hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Asteroid") || other.CompareTag("CrimsonFleet") || other.CompareTag("ThraxArmada"))
        {
            StartCoroutine(Explode());
        }
    }
}
