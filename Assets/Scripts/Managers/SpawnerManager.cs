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

    List<Ship> shipNamesCrimsonFleet = new List<Ship> {
        new Ship { name = "CrimsonSmall1", weight = 0.15f },
        new Ship { name = "CrimsonSmall2", weight = 0.15f },
        new Ship { name = "CrimsonSmall3", weight = 0.15f },
        new Ship { name = "CrimsonSmall4", weight = 0.15f },
        new Ship { name = "CrimsonBomber", weight = 0.34f },
        new Ship { name = "CrimsonBomberSpawner", weight = 0.025f },
        new Ship { name = "CrimsonBuffer", weight = 0.025f }


    };
    List<Ship> shipNamesEarly = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.5f },
        new Ship { name = "MeleeShip", weight = 0.5f }
    };
    List<Ship> shipNamesEarly2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.3f },
        new Ship { name = "MeleeShip", weight = 0.3f },
        new Ship { name = "MediumShip", weight = 0.2f },
        new Ship { name = "MediumShip2", weight = 0.2f },
    };
    List<Ship> shipNamesEarly3 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.1f },
        new Ship { name = "MeleeShip", weight = 0.1f },
        new Ship { name = "MediumShip", weight = 0.4f },
        new Ship { name = "MediumShip2", weight = 0.4f },
    };
    List<Ship> shipNamesMid = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.1f },
        new Ship { name = "MeleeShip", weight = 0.1f },
        new Ship { name = "MediumShip", weight = 0.35f },
        new Ship { name = "MediumShip2", weight = 0.4f },
        new Ship { name = "LargeShip", weight = 0.05f },

    };
    List<Ship> shipNamesMid2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.4f },
        new Ship { name = "LargeShip", weight = 0.2f },
    };
    List<Ship> shipNamesMid3 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.35f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
    };
    List<Ship> shipNamesLate = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.35f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
    };
    List<Ship> shipNamesLate2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.3f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
        new Ship { name = "NukeShip2", weight = 0.05f },


    };
    List<Ship> shipNamesLate3 = new List<Ship> {
        new Ship { name = "MediumShip", weight = 0.2f },
        new Ship { name = "MediumShip2", weight = 0.2f },
        new Ship { name = "LargeShip", weight = 0.3f },
        new Ship { name = "NukeShip2", weight = 0.3f },

    };



    public struct Ship
    {
        public string name;
        public float weight;
    }
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
        StartCoroutine(SpawnEnemiesOverTime());
        StartCoroutine(SpawnCometsOverTime());

    }

    void OnDisable()
    {
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

            // Choose the correct ship list based on the level
            List<Ship> shipList;
            if (GameManager.Instance.Level >= 1 && GameManager.Instance.Level < 10)
            {
                shipList = shipNamesEarly;
            }
            else if (GameManager.Instance.Level >= 10 && GameManager.Instance.Level < 20)
            {
                shipList = shipNamesCrimsonFleet;
            }
            else if (GameManager.Instance.Level >= 20 && GameManager.Instance.Level < 30)
            {
                shipList = shipNamesEarly3;
            }
            else if (GameManager.Instance.Level >= 30 && GameManager.Instance.Level < 40)
            {
                shipList = shipNamesMid;
            }
            else if (GameManager.Instance.Level >= 40 && GameManager.Instance.Level < 50)
            {
                shipList = shipNamesMid2;
            }
            else if (GameManager.Instance.Level >= 50 && GameManager.Instance.Level < 60)
            {
                shipList = shipNamesMid3;
            }
            else if (GameManager.Instance.Level >= 60 && GameManager.Instance.Level < 70)
            {
                shipList = shipNamesLate;
            }
            else if (GameManager.Instance.Level >= 70 && GameManager.Instance.Level < 80)
            {
                shipList = shipNamesLate2;
            }
            else if (GameManager.Instance.Level >= 80 && GameManager.Instance.Level < 90)
            {
                shipList = shipNamesLate3;
            }
            else
            {
                shipList = shipNamesLate3;
            }

            // Get a random ship based on probabilities
            string chosenShip = GetRandomShip(shipList);
            GameObject ship = SpawnShip(chosenShip, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(GameManager.Instance.GetSpawnRate());
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


}
