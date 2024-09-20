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
    List<Ship> _defendingShipsList = new List<Ship>();

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

    public IEnumerator SpawnEnemiesOverTime(List<Ship> shipList, float spawnRate, int numberOfEnemiesToSpawn, float spawnPointRadius, Dictionary<string, GameObject> specialEnemies)
    {
        if (shipList == null || shipList.Count == 0)
        {
            Debug.LogWarning("No ships available to spawn.");
            yield break; // Exit the coroutine if there are no ships to spawn
        }
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
            Enemy enemy = ship.GetComponent<Enemy>();
            specialEnemies.Add(enemy.EnemyID, ship);
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

    public List<Ship> DetermineDefendingShips()
    {
        List<Ship> ships = DetermineInvadingShips(InvasionManager.Instance.DefendingFaction);
        if (ships == null || ships.Count == 0)
        {
            Debug.LogWarning("No defending ships available for faction: " + InvasionManager.Instance.DefendingFaction);
        }
        return ships ?? new List<Ship>(); // Ensure we return an empty list instead of null
    }

    public List<Ship> DetermineSoloInvadingShips()
    {
        foreach (var faction in InvasionManager.Instance.InvadingFactions)
        {
            var ships = DetermineInvadingShips(faction);
            if (ships != null && ships.Count > 0)
            {
                Debug.Log($"Ships found for faction: {faction}");
                return ships;
            }
        }
        Debug.LogError("No ships found for any invading faction");
        return new List<Ship>(); // Return an empty list instead of null
    }

    private List<Ship> DetermineInvadingShips(FactionType faction)
    {
        switch (faction)
        {
            case FactionType.CrimsonFleet:
                return GetCrimsonFleetShips();
            case FactionType.ThraxArmada:
                return GetThraxArmadaShips();
            case FactionType.Syndicates:
                return GetSyndicatesShips();
            default:
                Debug.LogError("No ships found for faction: " + faction);
                return new List<Ship>(); // Return an empty list for unknown factions
        }
    }

    private List<Ship> GetCrimsonFleetShips()
    {
        return GetShipsBasedOnLevel(ShipDatabase.CrimsonFleetEarly, ShipDatabase.CrimsonFleetMid, ShipDatabase.CrimsonFleetLate);
    }

    private List<Ship> GetThraxArmadaShips()
    {
        return GetShipsBasedOnLevel(ShipDatabase.ThraxArmadaEarly, ShipDatabase.ThraxArmadaMid, ShipDatabase.ThraxArmadaLate);
    }

    private List<Ship> GetSyndicatesShips()
    {
        int level = LevelManager.Instance.CurrentLevelIndex + 1;
        if (level < 10) return ShipDatabase.SyndicatesEarly;
        else if (level < 20) return ShipDatabase.SyndicatesEarly2;
        else if (level < 30) return ShipDatabase.SyndicatesEarly3;
        else if (level < 40) return ShipDatabase.SyndicatesMid;
        else if (level < 50) return ShipDatabase.SyndicatesMid2;
        else if (level < 60) return ShipDatabase.SyndicatesMid3;
        else if (level < 70) return ShipDatabase.SyndicateLate;
        else if (level < 80) return ShipDatabase.SyndicatesLate2;
        else if (level < 90) return ShipDatabase.SyndicatesLate3;

        return null;
    }

    private List<Ship> GetShipsBasedOnLevel(List<Ship> early, List<Ship> mid, List<Ship> late)
    {
        int level = LevelManager.Instance.CurrentLevelIndex + 1;
        if (level < 30) return early;
        else if (level < 60) return mid;
        else return late;
    }
    public string GetSpawnerBossName()
    {
        switch (InvasionManager.Instance.DefendingFaction)
        {
            case FactionType.CrimsonFleet:
                return "CrimsonBomberSpawner";
            case FactionType.Syndicates:
                return Random.value <= 0.9 ? "Carrier" : "SuperCarrier";
            case FactionType.ThraxArmada:
                return "ThraxCarrier1";
            default:
                return "Carrier";
        }
    }
    public string GetShooterBossName()
    {
        List<string> syndicateBosses = new List<string> { "AssaultShip", "AssaultShip2" };
        List<string> crimsonBosses = new List<string> { "CrimsonDestroyer", "CrimsonDestroyer2" };
        List<string> thraxBosses = new List<string> { "ThraxBoss1", "ThraxBoss2" };

        switch (InvasionManager.Instance.DefendingFaction)
        {
            case FactionType.CrimsonFleet:
                return crimsonBosses[Random.Range(0, crimsonBosses.Count)];
            case FactionType.Syndicates:
                return syndicateBosses[Random.Range(0, syndicateBosses.Count)];
            case FactionType.ThraxArmada:
                return thraxBosses[Random.Range(0, thraxBosses.Count)];
            default:
                return "AssaultShip";
        }

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
    public List<Ship> DefendingShipsList { get => _defendingShipsList; }

}
