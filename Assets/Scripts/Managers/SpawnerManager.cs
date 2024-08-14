using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    private string[][] enemyShipNames;
    private float[][] enemyProbabilities;

    void Awake()
    {
        enemyShipNames = new string[][]
        {
            new string[] { "SmallShip", "MeleeShip", "MediumShip", "MediumShip2" },
            new string[] { "NukeShip", "MediumShip", "MediumShip2", "SmallShip" },
            new string[] { "LargeShip", "Carrier1", "NukeShip", "MediumShip", "MediumShip2" },
            new string[] { "LargeShip", "Carrier1", "NukeShip", "MediumShip", "MediumShip2" },
            new string[] { "LargeShip", "Carrier1", "NukeShip", "NukeShip2", "MediumShip", "MediumShip2" },
            new string[] { "LargeShip", "Carrier1", "NukeShip", "NukeShip2" },
            new string[] { "LargeShip", "Carrier1", "NukeShip2" }
        };

        enemyProbabilities = new float[][]
        {
            new float[] { 0.2f, 0.2f, 0.3f, 0.3f },
            new float[] { 0.01f, 0.4f, 0.4f, 0.19f },
            new float[] { 0.09f, 0.03f, 0.2f, 0.35f, 0.35f },
            new float[] { 0.05f, 0.001f, 0.2f, 0.2f, 0.4f },
            new float[] { 0.1f, 0.05f, 0.2f, 0.2f, 0.1f, 0.2f },
            new float[] { 0.3f, 0.2f, 0.2f, 0.2f },
            new float[] { 0.2f, 0.2f, 0.2f }
        };
    }
    void Start()
    {
        InvokeRepeating("SpawnShipsWrapper", 0, GameManager.Instance.spawnRate);

    }
    void OnEnable()
    {
        InvokeRepeating("SpawnShipsWrapper", 0, GameManager.Instance.spawnRate);
    }

    void OnDisable()
    {
        CancelInvoke("SpawnShipsWrapper");
        SpawnBossShips();
    }


    private void SpawnBossShips()
    {
        if (player != null)
        {
            if (GameManager.Instance.level == 49)
            {
                SpawnBossShipRandomPosition("LargeShip", 3);
            }
            else if (GameManager.Instance.level == 99)
            {
                SpawnBossShipRandomPosition("BossShip", 1);
            }
            else if (GameManager.Instance.level == 149)
            {
                SpawnBossShipRandomPosition("BossShip", 2);
            }
            else
            {
                SpawnBossShipRandomPosition("BossShip", 3);
            }
        }
    }

    private void SpawnBossShipRandomPosition(string shipName, int amountToSpawn)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 randomPosition = Random.onUnitSphere * 50;
            GameObject ship = ObjectPooler.Instance.SpawnFromPool(shipName, player.transform.position + randomPosition, transform.rotation);
            ship.GetComponent<ShooterEnemy>().amountOfBullets += 5;
            ship.GetComponent<ShooterEnemy>().bulletSpeed += 4f;
            ship.transform.localScale += new Vector3(3f, 3f, 3f);
            ship.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }


    private void SpawnShip(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject enemy = ObjectPooler.Instance.SpawnFromPool(tag, position, rotation);
        GameManager.Instance.enemies.Add(enemy);
    }

    private string SelectRandomName(string[] names, float[] probabilities)
    {
        float[] cumulativeProbabilities = new float[probabilities.Length];
        cumulativeProbabilities[0] = probabilities[0];
        for (int i = 1; i < probabilities.Length; i++)
        {
            cumulativeProbabilities[i] = cumulativeProbabilities[i - 1] + probabilities[i];
        }

        float randomValue = Random.value;
        for (int i = 0; i < cumulativeProbabilities.Length; i++)
        {
            if (randomValue < cumulativeProbabilities[i])
            {
                return names[i];
            }
        }

        return names[names.Length - 1];
    }

    public void SpawnShipRandomLocation(string shipName)
    {
        Vector3 randomPosition = Random.onUnitSphere * 50;
        SpawnShip(shipName, randomPosition + player.transform.position, transform.rotation);
    }

    private void SpawnShipsWrapper()
    {
        int level = GameManager.Instance.level;
        if (player != null)
        {
            if (level <= 19)
            {
                SpawnShipRandomLocation(SelectRandomName(enemyShipNames[0], enemyProbabilities[0]));
            }
            else if (level < 40)
            {
                SpawnShipRandomLocation(SelectRandomName(enemyShipNames[1], enemyProbabilities[1]));
            }
            else if (level < 60)
            {
                SpawnShipRandomLocation(SelectRandomName(enemyShipNames[2], enemyProbabilities[2]));
            }
            else if (level < 80)
            {
                SpawnShipRandomLocation(SelectRandomName(enemyShipNames[3], enemyProbabilities[3]));
            }
            else if (level < 140)
            {
                SpawnShipRandomLocation(SelectRandomName(enemyShipNames[4], enemyProbabilities[4]));
            }
            else if (level < 200)
            {
                SpawnShipRandomLocation(SelectRandomName(enemyShipNames[5], enemyProbabilities[5]));
            }
            else
            {
                SpawnShipRandomLocation(SelectRandomName(enemyShipNames[6], enemyProbabilities[6]));
            }
        }
    }
}
