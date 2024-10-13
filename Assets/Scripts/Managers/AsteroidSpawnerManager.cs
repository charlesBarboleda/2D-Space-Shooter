using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnerManager : MonoBehaviour
{
    [SerializeField] List<string> asteroidsList;
    [SerializeField] string asteroidFieldTag = "AsteroidField";
    public int asteroidCount = 20;
    public int asteroidFieldCount = 1;

    public static AsteroidSpawnerManager Instance { get; private set; }

    void Start()
    {
        SpawnAsteroids();
        SpawnAsteroidField();
    }

    void SpawnAsteroids()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            int randomIndex = Random.Range(0, asteroidsList.Count);
            GameObject asteroid = ObjectPooler.Instance.SpawnFromPool(asteroidsList[randomIndex], transform.position, Quaternion.identity);

            asteroid.transform.position = new Vector3(Random.Range(-300, 300), Random.Range(-300, 300), 0);
            NavMeshScript.Instance.UpdateNavMesh();
        }
    }

    void SpawnAsteroidField()
    {
        for (int i = 0; i < asteroidFieldCount; i++)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(-300, 300), Random.Range(-300, 300), 0);
            GameObject asteroidField = ObjectPooler.Instance.SpawnFromPool(asteroidFieldTag, spawnPoint, Quaternion.identity);
            foreach (Transform child in asteroidField.transform)
            {
                child.transform.position = spawnPoint + new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), 0);
            }
            NavMeshScript.Instance.UpdateNavMesh();
        }
    }


}
