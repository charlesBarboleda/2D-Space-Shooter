using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameBossLevel : SoloShooterBossLevel
{
    public bool phase1Complete = false;
    public bool phase2Complete = false;
    public bool phase3Complete = false;
    public bool hasTransitionedPhase2 = false;

    public EndGameBossLevel(float health, float bulletDamage, float bulletSpeed,
    float firerate, float speed, float stopDistance, float attackRange, float fireAngle,
    float currencyDrop, List<Vector3> spawnPoints, string bossName, LevelManager levelManager,
    SpawnerManager spawnerManager, FormationType formationType, int numberOfShipsInFormation,
    float formationRadius, List<string> formationShipName) : base(health, bulletDamage,
    bulletSpeed, firerate, speed, stopDistance, attackRange, fireAngle, currencyDrop,
    spawnPoints, bossName, levelManager, spawnerManager, formationType, numberOfShipsInFormation,
    formationRadius, formationShipName)
    {

    }

    public override void StartLevel()
    {
        Debug.Log("Starting End Game Boss Level");
        spawnerManager.StartCoroutine(spawnerManager.SpawnEnemiesWaves(1, 30f));
        Debug.Log("Enemies to spawn: " + spawnerManager.EnemiesToSpawnLeft);
        Debug.Log("Enemies Count: " + spawnerManager.EnemiesList.Count);

        Debug.Log("Spawning Boss Ship");
        Background.Instance.PlayThraxBossPhase1Music();
        Debug.Log("Played Phase 1 Music");

        bossShip = spawnerManager.SpawnShip(bossName, spawnPoints[Random.Range(0, spawnPoints.Count)], Quaternion.identity);
        bossShip.GetComponent<Health>().isDead = true;
        bossShip.GetComponent<Kinematics>().ShouldMove = false;

        SetBossShooterStats(bossShip);

    }

    public override void UpdateLevel()
    {
        if (spawnerManager.EnemiesList.Count == 1 && spawnerManager.EnemiesToSpawnLeft <= 0)
        {
            phase1Complete = true;
            Debug.Log("Phase 1 Complete");
            Debug.Log("Phase 2 Started");


            // Start the boss ship
            bossShip.GetComponent<Health>().isDead = false;
            bossShip.GetComponent<Kinematics>().ShouldMove = true;


            if (!hasTransitionedPhase2)
            {
                // Pan the camera to the boss ship
                CameraFollowBehaviour cameraFollow = Camera.main.GetComponent<CameraFollowBehaviour>();
                cameraFollow.StartCoroutine(cameraFollow.PanToTargetAndBack(bossShip.transform, 7f));

                // Play Phase 2 music
                Background.Instance.PlayThraxBossPhase2Music();
                hasTransitionedPhase2 = true;
                Debug.Log("Played Phase 2 Music");
            }


        }


        if ((bossShip == null || bossShip.GetComponent<Health>().isDead ||
            !bossShip.activeInHierarchy) && spawnerManager.EnemiesToSpawnLeft <= 0 &&
            spawnerManager.EnemiesList.Count <= 0)
            CompleteLevel();

    }

    public override void CompleteLevel()
    {
        Debug.Log("Completing Level");
        levelManager.CompleteLevel();
    }


}
