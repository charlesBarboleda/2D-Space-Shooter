using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }
    Vector3 gameAreaCenter;
    Vector3 gameAreaSize;
    [System.Serializable]
    public struct Building
    {
        public Vector3 center;
        public Vector3 size;
    }
    public List<Building> buildings = new List<Building>();

    [SerializeField] List<Transform> cometSpawnPoint = new List<Transform>();
    [SerializeField] List<string> cometsList = new List<string>();
    [SerializeField] int numberOfSpawnPoints = 360;
    [SerializeField] List<GameObject> _enemiesList;
    [SerializeField] int enemiesToSpawnLeft;
    [SerializeField] List<Vector3> _soloBossSpawnPoints = new List<Vector3>();
    List<GameObject> _defendingShipsList = new List<GameObject>();
    List<GameObject> _specialEnemiesList = new List<GameObject>();

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
    void Start()
    {

        gameAreaCenter = new Vector3(0, 0, 0);
        gameAreaSize = new Vector3(450, 450, 0);

    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    void LateUpdate()
    {
        EnemiesList.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
    }
    public Vector3 GetRandomPositionOutsideBuildings()
    {
        Vector3 randomPosition;

        // Loop until we find a random position that is outside all buildings
        do
        {
            // Generate random position within the game area
            randomPosition = new Vector3(
                Random.Range(gameAreaCenter.x - gameAreaSize.x / 2, gameAreaCenter.x + gameAreaSize.x / 2),
                Random.Range(gameAreaCenter.y - gameAreaSize.y / 2, gameAreaCenter.y + gameAreaSize.y / 2),
                Random.Range(gameAreaCenter.z - gameAreaSize.z / 2, gameAreaCenter.z + gameAreaSize.z / 2)
            );
        }
        // Check if the random position is inside any of the building bounds and continue if it is
        while (IsInsideAnyBuilding(randomPosition));

        return randomPosition;
    }

    bool IsInsideAnyBuilding(Vector3 position)
    {
        // Check against all buildings
        foreach (Building building in buildings)
        {
            bool insideX = position.x > building.center.x - building.size.x / 2 && position.x < building.center.x + building.size.x / 2;
            bool insideY = position.y > building.center.y - building.size.y / 2 && position.y < building.center.y + building.size.y / 2;
            bool insideZ = position.z > building.center.z - building.size.z / 2 && position.z < building.center.z + building.size.z / 2;

            // If the position is inside any building, return true
            if (insideX && insideY && insideZ)
            {
                return true;
            }
        }
        // Return false if the position is not inside any building
        return false;
    }

    public GameObject SpawnComet(Vector3 position, Quaternion rotation)
    {
        GameObject comet = ObjectPooler.Instance.SpawnFromPool(cometsList[Random.Range(0, cometsList.Count)], position, rotation);
        return comet;
    }

    public void ResetRound()
    {
        SpecialEnemiesList.Clear();
        DefendingShipsList.Clear();
        EnemiesList.Clear();
        EnemiesToSpawnLeft = 0;
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
    public IEnumerator SpawnCometsOverTime(int cometAmount, float cometSpawnRate)
    {
        for (int i = 0; i <= cometAmount; i++)
        {
            yield return new WaitForSeconds(cometSpawnRate);
            GameObject comet = SpawnComet(cometSpawnPoint[Random.Range(0, cometSpawnPoint.Count)].position, Quaternion.identity);
            Comet cometSettings = comet.GetComponent<Comet>();
            cometSettings.Speed = Random.Range(20, 40);
        }
    }

    public IEnumerator SpawnEnemiesOverTime(List<Ship> shipList, float spawnRate, int numberOfEnemiesToSpawn, float spawnPointRadius, List<GameObject> specialEnemies)
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
            specialEnemies.Add(ship);
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
        if (level < 15) return ShipDatabase.SyndicatesEarly;
        else if (level < 20) return ShipDatabase.SyndicatesEarly2;
        else if (level < 25) return ShipDatabase.SyndicatesEarly3;
        else if (level < 30) return ShipDatabase.SyndicatesMid;
        else if (level < 35) return ShipDatabase.SyndicatesMid2;
        else if (level < 40) return ShipDatabase.SyndicatesMid3;
        else if (level < 45) return ShipDatabase.SyndicateLate;
        else if (level < 50) return ShipDatabase.SyndicatesLate2;
        else if (level < 55) return ShipDatabase.SyndicatesLate3;

        return null;
    }

    private List<Ship> GetShipsBasedOnLevel(List<Ship> early, List<Ship> mid, List<Ship> late)
    {
        int level = LevelManager.Instance.CurrentLevelIndex;
        if (level < 20) return early;
        else if (level < 40) return mid;
        else return late;
    }
    public List<string> GetFormationShipNames()
    {
        Debug.Log("GetFormationShipNames called.");
        List<string> crimsonFleetShipNames = new List<string> { "CrimsonSmall1", "CrimsonSmall2", "CrimsonSmall3", "CrimsonSmall4" };
        List<string> syndicatesShipNames = new List<string> { "MediumShip", "MediumShip2", "SmallShip", "MeleeShip" };
        List<string> thraxArmadaShipNames = new List<string> { "ThraxSmall1", "ThraxSmall2", "ThraxSmall3" };


        switch (InvasionManager.Instance.DefendingFaction)
        {
            case FactionType.CrimsonFleet:
                Debug.Log("Crimson Fleet Names");
                return crimsonFleetShipNames;
            case FactionType.Syndicates:
                Debug.Log("Syndicates Names: " + syndicatesShipNames.Count + syndicatesShipNames[0]);
                return syndicatesShipNames;
            case FactionType.ThraxArmada:
                Debug.Log("Thrax Armada Names");
                return thraxArmadaShipNames;
            default:
                return new List<string> { "MediumShip" };
        }
    }
    public string GetSpawnerBossName()
    {
        switch (InvasionManager.Instance.DefendingFaction)
        {
            case FactionType.CrimsonFleet:
                return "CrimsonBomberSpawner";
            case FactionType.Syndicates:
                return Random.value <= 0.7 ? "Carrier" : "SuperCarrier";
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
    // --- Different formation methods ---

    public void SpawnCircleFormation(int _numberOfShipsInFormation, float _formationRadius, Vector3 _formationCenter, List<string> _formationShipName)
    {
        float angleStep = 360f / _numberOfShipsInFormation;
        for (int i = 0; i < _numberOfShipsInFormation; i++)
        {
            float angle = i * angleStep;
            Vector3 spawnPosition = _formationCenter + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _formationRadius;
            SpawnFormationShip(spawnPosition, _formationShipName);
            Debug.Log("Spawning Circle Formation from spawnermanager");
        }
    }

    public void SpawnVShapeFormation(int _numberOfShipsInFormation, float _formationRadius, Vector3 _formationCenter, List<string> _formationShipName)
    {
        float angleStep = 30f; // Angle between the ships on either side of the V
        int halfFormation = _numberOfShipsInFormation / 2;

        for (int i = 0; i < _numberOfShipsInFormation; i++)
        {
            float angle = angleStep * (i - halfFormation); // Spread ships based on angle, centered on 0

            // Calculate the position using polar coordinates
            float xOffset = Mathf.Sin(Mathf.Deg2Rad * angle) * _formationRadius;
            float yOffset = Mathf.Cos(Mathf.Deg2Rad * angle) * _formationRadius;

            // Adjust positions to make a V (ships closer at the bottom, wider at the top)
            Vector3 spawnPosition = _formationCenter + new Vector3(xOffset, -Mathf.Abs(yOffset), 0);
            SpawnFormationShip(spawnPosition, _formationShipName);
        }
    }


    public void SpawnLineFormation(int _numberOfShipsInFormation, float _formationRadius, Vector3 _formationCenter, List<string> _formationShipName)
    {
        float offset = _formationRadius * 1.5f;
        for (int i = 0; i < _numberOfShipsInFormation; i++)
        {
            Vector3 spawnPosition = _formationCenter + new Vector3(i * offset, 0, 0); // Horizontal line
            SpawnFormationShip(spawnPosition, _formationShipName);
        }
    }


    public void SpawnGridFormation(int _numberOfShipsInFormation, float _formationRadius, Vector3 _formationCenter, List<string> _formationShipName)
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(_numberOfShipsInFormation));
        float offset = _formationRadius * 1.5f;
        for (int i = 0; i < _numberOfShipsInFormation; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;
            Vector3 spawnPosition = _formationCenter + new Vector3(col * offset, row * offset, 0);
            SpawnFormationShip(spawnPosition, _formationShipName);
        }
    }

    public void SpawnStarFormation(int _numberOfShipsInFormation, float _formationRadius, Vector3 _formationCenter, List<string> _formationShipName)
    {
        float angleStep = 360f / _numberOfShipsInFormation;

        for (int i = 0; i < _numberOfShipsInFormation; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float radius = i % 2 == 0 ? _formationRadius * 0.5f : _formationRadius; // Alternate between inner and outer radius for star points

            Vector3 spawnPosition = _formationCenter + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            SpawnFormationShip(spawnPosition, _formationShipName);
        }
    }

    public void SpawnSpiralFormation(int _numberOfShipsInFormation, float _formationRadius, Vector3 _formationCenter, List<string> _formationShipName)
    {
        float angleStep = 360f / _numberOfShipsInFormation;
        float radiusStep = _formationRadius / _numberOfShipsInFormation;
        for (int i = 0; i < _numberOfShipsInFormation; i++)
        {
            float angle = i * angleStep;
            float radius = i * radiusStep;
            Vector3 spawnPosition = _formationCenter + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            SpawnFormationShip(spawnPosition, _formationShipName);
        }
    }

    void SpawnFormationShip(Vector3 spawnPosition, List<string> _formationShipName)
    {
        GameObject formationShip = SpawnShip(_formationShipName[Random.Range(0, _formationShipName.Count)], spawnPosition, Quaternion.identity);
    }


    public IEnumerator SpawnEnemiesWaves(int numberOfWaves, float spawnRate)
    {
        int numberOfShips = 15;
        EnemiesToSpawnLeft = numberOfWaves * numberOfShips;
        while (numberOfWaves > 0)
        {
            SpawnStarFormation(numberOfShips, 500, new Vector3(0, 0, 0), new List<string>(GetFormationShipNames()));
            yield return new WaitForSeconds(spawnRate);
            numberOfWaves--;
            numberOfShips += 15;
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
    public List<GameObject> SpecialEnemiesList { get => _specialEnemiesList; set => _specialEnemiesList = value; }
    public List<GameObject> DefendingShipsList { get => _defendingShipsList; }
    public int EnemiesToSpawnLeft { get => enemiesToSpawnLeft; set => enemiesToSpawnLeft = value; }
    public List<Vector3> SoloBossSpawnPoints { get => _soloBossSpawnPoints; }

}
