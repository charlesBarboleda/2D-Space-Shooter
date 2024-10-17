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
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        // SpawnAsteroidFields();
        // SpawnAsteroids();
    }
    void SpawnAsteroids()
    {

        for (int i = 0; i < asteroidCount; i++)
        {
            Vector3 spawnPosition = SpawnerManager.Instance.GetRandomPositionOutsideBuildings();
            SpawnAsteroid(spawnPosition);

        }
    }

    void SpawnAsteroidFields()
    {

        for (int i = 0; i < asteroidFieldCount; i++)
        {
            Vector3 spawnPosition = SpawnerManager.Instance.GetRandomPositionOutsideBuildings();
            SpawnAsteroidField(spawnPosition);

        }
    }

    void SpawnAsteroid(Vector3 spawnPosition)
    {

        int randomIndex = Random.Range(0, asteroidsList.Count);
        GameObject asteroid = ObjectPooler.Instance.SpawnFromPool(asteroidsList[randomIndex], spawnPosition, Quaternion.identity);
        NavMeshScript.Instance.UpdateNavMesh();
    }

    void SpawnAsteroidField(Vector3 spawnPosition)
    {

        GameObject asteroidField = ObjectPooler.Instance.SpawnFromPool(asteroidFieldTag, spawnPosition, Quaternion.identity);
        foreach (Transform child in asteroidField.transform)
        {
            child.transform.position = spawnPosition + new Vector3(Random.Range(-60, 60), Random.Range(-60, 60), 0);
        }
        NavMeshScript.Instance.UpdateNavMesh();
    }


}
