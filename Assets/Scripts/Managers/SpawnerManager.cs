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
    [SerializeField] int spawnPointRadius = 200;
    [SerializeField] List<GameObject> _enemiesList;
    [SerializeField] int enemiesToSpawnLeft;
    [SerializeField] List<Vector3> _soloBossSpawnPoints = new List<Vector3>();

    public List<Ship> shipNamesCrimsonFleet = new List<Ship> {
        new Ship { name = "CrimsonSmall1", weight = 0.15f },
        new Ship { name = "CrimsonSmall2", weight = 0.15f },
        new Ship { name = "CrimsonSmall3", weight = 0.15f },
        new Ship { name = "CrimsonSmall4", weight = 0.15f },
        new Ship { name = "CrimsonBomber", weight = 0.38f },
        new Ship { name = "CrimsonBomberSpawner", weight = 0.01f },
        new Ship { name = "CrimsonBuffer", weight = 0.01f }
    };
    public List<Ship> shipNamesThraxArmada = new List<Ship> {
        new Ship { name = "ThraxSmall1", weight = 0.298f },
        new Ship { name = "ThraxSmall2", weight = 0.298f },
        new Ship { name = "ThraxSmall3", weight = 0.298f },
        new Ship { name = "ThraxTeleporter1", weight = 0.05f },
        new Ship { name = "ThraxTeleporter2", weight = 0.05f },
        new Ship { name = "ThraxCarrier1", weight = 0.005f },
        new Ship { name = "ThraxBoss1", weight = 0.001f }
    };
    public List<Ship> shipNamesEarly = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.5f },
        new Ship { name = "MeleeShip", weight = 0.5f }
    };
    public List<Ship> shipNamesEarly2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.3f },
        new Ship { name = "MeleeShip", weight = 0.3f },
        new Ship { name = "MediumShip", weight = 0.2f },
        new Ship { name = "MediumShip2", weight = 0.2f },
    };
    public List<Ship> shipNamesEarly3 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.1f },
        new Ship { name = "MeleeShip", weight = 0.1f },
        new Ship { name = "MediumShip", weight = 0.4f },
        new Ship { name = "MediumShip2", weight = 0.4f },
    };
    public List<Ship> shipNamesMid = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.1f },
        new Ship { name = "MeleeShip", weight = 0.1f },
        new Ship { name = "MediumShip", weight = 0.35f },
        new Ship { name = "MediumShip2", weight = 0.4f },
        new Ship { name = "LargeShip", weight = 0.05f },

    };
    public List<Ship> shipNamesMid2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.4f },
        new Ship { name = "LargeShip", weight = 0.2f },
    };
    public List<Ship> shipNamesMid3 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.35f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
    };
    public List<Ship> shipNamesLate = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.35f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
    };
    public List<Ship> shipNamesLate2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.3f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
        new Ship { name = "NukeShip2", weight = 0.05f },


    };
    public List<Ship> shipNamesLate3 = new List<Ship> {
        new Ship { name = "MediumShip", weight = 0.2f },
        new Ship { name = "MediumShip2", weight = 0.2f },
        new Ship { name = "LargeShip", weight = 0.3f },
        new Ship { name = "NukeShip2", weight = 0.3f },

    };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        StartCoroutine(SpawnCometsOverTime());

    }

    void OnDisable()
    {
        StopCoroutine(SpawnCometsOverTime());
    }
    void LateUpdate()
    {
        EnemiesList.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
    }

    public GameObject SpawnComet(Vector3 position, Quaternion rotation)
    {
        GameObject comet = ObjectPooler.Instance.SpawnFromPool(cometsList[Random.Range(0, cometsList.Count)], position, rotation);
        return comet;
    }



    public GameObject SpawnShip(string tag, Vector3 position, Quaternion rotation)
    {

        GameObject enemy = ObjectPooler.Instance.SpawnFromPool(tag, position, rotation);
        if (enemy != null)
        {
            AddEnemy(enemy);
            EnemiesToSpawnLeft--;
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
            cometSettings.Speed = Random.Range(20, 40);
        }
    }

    public IEnumerator SpawnEnemiesOverTime(List<Ship> shipList, float spawnRate, int numberOfEnemiesToSpawn, float spawnPointRadius)
    {
        for (int i = 0; i <= numberOfEnemiesToSpawn; i++)
        {
            // Select a random segment between two consecutive points
            float segmentIndex = Random.Range(0, numberOfSpawnPoints);
            float minAngle = segmentIndex * Mathf.PI * 2 / numberOfSpawnPoints;
            float maxAngle = (segmentIndex + 1) * Mathf.PI * 2 / numberOfSpawnPoints;
            float randomAngle = Random.Range(minAngle, maxAngle);

            // Calculate the random position on the circle
            Vector3 spawnPosition = new Vector3(Mathf.Cos(randomAngle) * spawnPointRadius, Mathf.Sin(randomAngle) * spawnPointRadius, 0);



            // Get a random ship based on probabilities
            string chosenShip = GetRandomShip(shipList);
            GameObject ship = SpawnShip(chosenShip, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnRate);
        }
    }
    private string GetRandomShip(List<Ship> ships)
    {
        float totalWeight = 0;
        foreach (var ship in ships)
        {
            totalWeight += ship.weight;
        }

        float randomWeight = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var ship in ships)
        {
            currentWeight += ship.weight;
            if (randomWeight <= currentWeight)
            {
                return ship.name;
            }
        }

        return ships[0].name; // Fallback in case no ship is chosen
    }

    public void AddEnemy(GameObject enemy)
    {
        EnemiesList.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        EnemiesList.Remove(enemy);
    }

    public void DisableSpawning()
    {
        gameObject.SetActive(false);
    }

    public void EnableSpawning()
    {
        gameObject.SetActive(true);
    }

    public List<GameObject> EnemiesList { get => _enemiesList; }
    public int EnemiesToSpawnLeft { get => enemiesToSpawnLeft; set => enemiesToSpawnLeft = value; }
    public List<Vector3> SoloBossSpawnPoints { get => _soloBossSpawnPoints; }

}
