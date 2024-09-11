using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    [SerializeField] List<Transform> cometSpawnPoint = new List<Transform>();
    [SerializeField] List<string> cometsList = new List<string>();
    [SerializeField] int numberOfSpawnPoints = 360;
    [SerializeField] int spawnPointRadius = 30;
    List<string> shipNamesEarly = new List<string> { "SmallShip", "MediumShip", "MeleeShip", "MediumShip2" };
    List<string> shipNamesMid = new List<string> { "SmallShip", "MediumShip", "MediumShip2", "MeleeShip", "LargeShip", "NukeShip" };
    List<string> shipNamesLate = new List<string> { "MediumShip", "MediumShip2", "LargeShip", "NukeShip", "NukeShip2" };
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        EventManager.OnEnemyDestroyed += RemoveEnemyFromList;
        StartCoroutine(SpawnEnemiesOverTime());
        StartCoroutine(SpawnCometsOverTime());

    }

    void OnDisable()
    {
        EventManager.OnEnemyDestroyed -= RemoveEnemyFromList;
        StopCoroutine(SpawnEnemiesOverTime());
        StopCoroutine(SpawnCometsOverTime());
    }

    private GameObject SpawnComet(Vector3 position, Quaternion rotation)
    {
        GameObject comet = ObjectPooler.Instance.SpawnFromPool(cometsList[Random.Range(0, cometsList.Count)], position, rotation);
        return comet;
    }



    private GameObject SpawnShip(string tag, Vector3 position, Quaternion rotation)
    {

        GameObject enemy = ObjectPooler.Instance.SpawnFromPool(tag, position, rotation);
        if (enemy != null)
        {
            GameManager.Instance.AddEnemy(enemy);
            GameManager.Instance.SetEnemiesToSpawnLeft(GameManager.Instance.GetEnemiesToSpawnLeft() - 1);
        }


        return enemy;
    }
    IEnumerator SpawnCometsOverTime()
    {
        for (int i = 0; i <= GameManager.Instance.CometsPerRound; i++)
        {
            yield return new WaitForSeconds(GameManager.Instance.CometSpawnRate);
            GameObject comet = SpawnComet(cometSpawnPoint[Random.Range(0, cometSpawnPoint.Count)].position, Quaternion.identity);
            Comet cometSettings = comet.GetComponent<Comet>();
            cometSettings.Speed = Random.Range(20, 50);
            Debug.Log("Comets Per Round: " + GameManager.Instance.CometsPerRound);
            Debug.Log("Comet Spawn Rate: " + GameManager.Instance.CometSpawnRate);
        }
    }

    IEnumerator SpawnEnemiesOverTime()
    {
        int spawnCount = 0;
        for (int i = 0; i <= GameManager.Instance.GetEnemiesToSpawnTotal(); i++)
        {
            spawnCount++;
            // Select a random segment between two consecutive points
            float segmentIndex = Random.Range(0, numberOfSpawnPoints);
            float minAngle = segmentIndex * Mathf.PI * 2 / numberOfSpawnPoints;
            float maxAngle = (segmentIndex + 1) * Mathf.PI * 2 / numberOfSpawnPoints;
            float randomAngle = Random.Range(minAngle, maxAngle);

            // Calculate the random position on the circle
            Vector3 spawnPosition = new Vector3(Mathf.Cos(randomAngle) * spawnPointRadius, Mathf.Sin(randomAngle) * spawnPointRadius, 0);

            if (GameManager.Instance.Level <= 20)
            {
                GameObject ship = SpawnShip(shipNamesEarly[Random.Range(0, shipNamesEarly.Count)], spawnPosition, Quaternion.identity);
            }
            else if (GameManager.Instance.Level > 20 && GameManager.Instance.Level <= 60)
            {
                GameObject ship = SpawnShip(shipNamesMid[Random.Range(0, shipNamesMid.Count)], spawnPosition, Quaternion.identity);
            }
            else
            {
                GameObject ship = SpawnShip(shipNamesLate[Random.Range(0, shipNamesLate.Count)], spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(GameManager.Instance.GetSpawnRate());
        }

    }

    private void RemoveEnemyFromList(GameObject enemy, Faction faction)
    {
        if (GameManager.Instance.GetEnemies().Contains(enemy))
        {
            GameManager.Instance.GetEnemies().Remove(enemy);

        }
    }

}
