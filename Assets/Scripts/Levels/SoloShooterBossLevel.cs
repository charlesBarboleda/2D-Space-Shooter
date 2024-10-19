using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloShooterBossLevel : Level
{

    public string bossName;
    public List<Vector3> spawnPoints = new List<Vector3>();
    public LevelManager levelManager;
    public SpawnerManager spawnerManager;
    public GameObject bossShip;
    public FormationType formationType = FormationType.Circle;
    public float health;
    public int bulletAmount;
    public float bulletDamage;
    public float bulletSpeed;
    public float fireRate;
    public float speed;
    public float stopDistance;
    public float attackRange;
    public float fireAngle;
    public float currencyDrop;
    public int numberOfShipsInFormation = 5;
    public float formationRadius = 10f;
    public List<string> formationShipName = new List<string>();
    public List<GameObject> formationShips = new List<GameObject>(); // List to store spawned formation ships


    public SoloShooterBossLevel(float health, int bulletAmount, float bulletDamage, float bulletSpeed, float firerate,
                                float speed, float stopDistance, float attackRange, float fireAngle,
                                float currencyDrop, List<Vector3> spawnPoints, string bossName,
                                LevelManager levelManager, SpawnerManager spawnerManager,
                                FormationType formationType, int numberOfShipsInFormation,
                                 float formationRadius, List<string> formationShipName)
    {
        this.health += health;
        this.bulletAmount += bulletAmount;
        this.bulletDamage += bulletDamage;
        this.bulletSpeed += bulletSpeed;
        this.fireRate += firerate;
        this.speed += speed;
        this.stopDistance += stopDistance;
        this.attackRange += attackRange;
        this.fireAngle = fireAngle;
        this.spawnPoints = spawnPoints;
        this.bossName = bossName;
        this.currencyDrop += currencyDrop;
        this.levelManager = levelManager;
        this.spawnerManager = spawnerManager;
        this.formationType = formationType;
        this.numberOfShipsInFormation = numberOfShipsInFormation;
        this.formationRadius = formationRadius;
        this.formationShipName = formationShipName;

    }
    public override void StartLevel()
    {
        Debug.Log("Starting Boss Level");
        bossShip = spawnerManager.SpawnShip(bossName, spawnPoints[Random.Range(0, spawnPoints.Count)], Quaternion.identity);
        spawnerManager.StartCoroutine(CameraFollowBehaviour.Instance.PanToTargetAndBack(bossShip.transform, 6f));
        SetBossShooterStats(bossShip);
        switch (formationType)
        {
            case FormationType.Circle:
                Debug.Log("Spawning Circle Formation from level");
                spawnerManager.SpawnCircleFormation(numberOfShipsInFormation, formationRadius, bossShip.transform.position, formationShipName);
                break;
            case FormationType.VShape:
                Debug.Log("Spawning VShape Formation");
                spawnerManager.SpawnVShapeFormation(numberOfShipsInFormation, formationRadius, bossShip.transform.position, formationShipName);
                break;
            case FormationType.Line:
                Debug.Log("Spawning Line Formation");
                spawnerManager.SpawnLineFormation(numberOfShipsInFormation, formationRadius, bossShip.transform.position, formationShipName);
                break;
            case FormationType.Grid:
                Debug.Log("Spawning Grid Formation");
                spawnerManager.SpawnGridFormation(numberOfShipsInFormation, formationRadius, bossShip.transform.position, formationShipName);
                break;
            case FormationType.Star:
                Debug.Log("Spawning Star Formation");
                spawnerManager.SpawnStarFormation(numberOfShipsInFormation, formationRadius, bossShip.transform.position, formationShipName);
                break;
            case FormationType.Spiral:
                Debug.Log("Spawning Spiral Formation");
                spawnerManager.SpawnSpiralFormation(numberOfShipsInFormation, formationRadius, bossShip.transform.position, formationShipName);
                break;
        }

    }



    public void SetBossShooterStats(GameObject bossShooter)
    {
        // Get the components
        Health healthScript = bossShooter.GetComponent<Health>();
        Kinematics kinematics = bossShooter.GetComponent<Kinematics>();
        AttackManager attackManager = bossShooter.GetComponent<AttackManager>();
        ShooterEnemy shooterEnemy = bossShooter.GetComponent<ShooterEnemy>();

        // Set the stats
        healthScript.MaxHealth = health;
        healthScript.CurrentHealth = health;
        healthScript.CurrencyDrop = currencyDrop;
        shooterEnemy.BulletAmount = bulletAmount;
        shooterEnemy.BulletDamage = bulletDamage;
        shooterEnemy.BulletSpeed = bulletSpeed;
        shooterEnemy.ShootingAngle = fireAngle;
        kinematics.Speed = speed;
        kinematics.StopDistance = stopDistance;
        attackManager.AttackCooldown = fireRate;
        attackManager.AimRange = attackRange;
    }

    public override void UpdateLevel()
    {
        if (bossShip == null || bossShip.GetComponent<Health>().isDead || !bossShip.activeInHierarchy)
            CompleteLevel();

    }

    public override void CompleteLevel()
    {
        Debug.Log("Completing Level");
        levelManager.CompleteLevel();
    }



}
