using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float stuckTime = 10.0f; // The amount of time the enemy must be still to be considered stuck
    public float movementThreshold = 0.5f; // The maximum movement range considered "still" (adjust as needed)

    void OnEnable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    void OnDisable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        NavMeshScript.Instance.UpdateNavMesh();
    }

    // Trigger when something starts colliding with the asteroid
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThraxArmada") || collision.gameObject.CompareTag("Syndicates") || collision.gameObject.CompareTag("CrimsonFleet"))
        {
            // Start checking if the enemy gets stuck
            StartCoroutine(CheckIfStuck(collision.gameObject));
        }
    }

    // Stop checking if the enemy exits the collision
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StopCoroutine(CheckIfStuck(collision.gameObject));
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
            Debug.Log($"{enemy.name} is stuck and will be disabled.");
            enemy.SetActive(false);
        }
    }
}
