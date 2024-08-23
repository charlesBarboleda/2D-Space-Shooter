using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] int numberOfSpawnPoints = 360;
    [SerializeField] int spawnPointRadius = 30;
    List<string> shipNamesEarly = new List<string> { "SmallShip", "MediumShip", "MeleeShip", "MediumShip2" };

    void Awake()
    {

    }

    void OnEnable()
    {
        EventManager.OnEnemyDestroyed += RemoveEnemyFromList;
        StartCoroutine(SpawnEnemiesOverTime());
    }

    void OnDisable()
    {
        EventManager.OnEnemyDestroyed -= RemoveEnemyFromList;
        StopAllCoroutines();
    }



    private void SpawnShip(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject enemy = ObjectPooler.Instance.SpawnFromPool(tag, position, rotation);
        GameManager.Instance.enemies.Add(enemy);
    }

    IEnumerator SpawnEnemiesOverTime()
    {
        for (int i = 0; i < GameManager.Instance.enemiesToSpawn; i++)
        {
            // Select a random segment between two consecutive points
            float segmentIndex = Random.Range(0, numberOfSpawnPoints);
            float minAngle = segmentIndex * Mathf.PI * 2 / numberOfSpawnPoints;
            float maxAngle = (segmentIndex + 1) * Mathf.PI * 2 / numberOfSpawnPoints;
            float randomAngle = Random.Range(minAngle, maxAngle);

            // Calculate the random position on the circle
            Vector3 spawnPosition = new Vector3(Mathf.Cos(randomAngle) * spawnPointRadius, Mathf.Sin(randomAngle) * spawnPointRadius, 0);

            SpawnShip(shipNamesEarly[Random.Range(0, shipNamesEarly.Count)], spawnPosition, Quaternion.identity);


            yield return new WaitForSeconds(GameManager.Instance.spawnRate);
        }
    }

    private void RemoveEnemyFromList(GameObject enemy)
    {
        if (GameManager.Instance.enemies.Contains(enemy))
        {
            GameManager.Instance.enemies.Remove(enemy);

        }
    }

}
