using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GasBlockade : MonoBehaviour
{
    Kinematics kinematics;
    NavMeshAgent navMeshAgent;

    [SerializeField] bool _shouldDisable = true;
    public float stuckTime = 10.0f; // The amount of time the enemy must be still to be considered stuck
    public float movementThreshold = 0.5f; // The maximum movement range considered "still"

    private Dictionary<GameObject, Coroutine> stuckCheckCoroutines = new Dictionary<GameObject, Coroutine>();

    void OnEnable()
    {
        if (_shouldDisable)
            StartCoroutine(DisableAfterTime());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        kinematics = other.GetComponent<Kinematics>();
        navMeshAgent = other.GetComponent<NavMeshAgent>();

        if (damageable != null)
        {
            if (other.CompareTag("Player"))
            {
                damageable.TakeDamage(10f);
            }
        }

        if (kinematics != null)
        {
            kinematics.outOfBounds = true;
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }

        // Start checking if the enemy is stuck for the specified enemy tags
        if ((other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates") || other.CompareTag("CrimsonFleet")) && !stuckCheckCoroutines.ContainsKey(other.gameObject))
        {
            Coroutine stuckCheck = StartCoroutine(CheckIfStuck(other.gameObject));
            stuckCheckCoroutines.Add(other.gameObject, stuckCheck);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (kinematics != null)
        {
            kinematics.outOfBounds = false;
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }

        // Stop checking if the enemy is stuck
        if (stuckCheckCoroutines.ContainsKey(other.gameObject))
        {
            StopCoroutine(stuckCheckCoroutines[other.gameObject]);
            stuckCheckCoroutines.Remove(other.gameObject);
        }
    }

    // Coroutine to check if the enemy is stuck
    IEnumerator CheckIfStuck(GameObject enemy)
    {
        Vector3 initialPosition = enemy.transform.position;
        yield return new WaitForSeconds(stuckTime); // Wait for the specified stuck time

        // Check if the enemy's position has changed beyond the allowed movement threshold
        float distanceMoved = Vector3.Distance(initialPosition, enemy.transform.position);
        if (distanceMoved <= movementThreshold)
        {
            // The enemy is stuck, disable it
            Debug.Log($"{enemy.name} is stuck in the gas blockade and will be disabled.");
            enemy.SetActive(false);
        }

        // Remove the coroutine from the dictionary since it finished running
        stuckCheckCoroutines.Remove(enemy);
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(30f);
        gameObject.SetActive(false);
    }
}
