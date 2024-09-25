using System.Collections;
using System.Collections.Generic;
using AssetUsageDetectorNamespace;
using UnityEngine;

public class MultiPhaseBossLevel : SoloShooterBossLevel
{
    private enum BossPhase
    {
        Phase1,
        Phase2,
        Phase3,
        Complete
    }
    public bool phase1Complete = false;
    public bool phase2Complete = false;
    public bool phase3Complete = false;
    public bool hasTransitionedPhase2 = false;
    public bool hasTransitionedPhase3 = false;
    public string bossNamePhase2;
    BossPhase currentPhase = BossPhase.Phase1;
    Kinematics bossKinematicsPhase1;
    Kinematics bossKinematicsPhase2;
    Health bossHealthPhase1;
    Health bossHealthPhase2;
    GameObject bossShipPhase2;
    CameraFollowBehaviour cameraFollow;

    public MultiPhaseBossLevel(float health, float bulletDamage, float bulletSpeed,
    float firerate, float speed, float stopDistance, float attackRange, float fireAngle,
    float currencyDrop, List<Vector3> spawnPoints, string bossName, LevelManager levelManager,
    SpawnerManager spawnerManager, FormationType formationType, int numberOfShipsInFormation,
    float formationRadius, List<string> formationShipName, string bossNamePhase2) : base(health, bulletDamage,
    bulletSpeed, firerate, speed, stopDistance, attackRange, fireAngle, currencyDrop,
    spawnPoints, bossName, levelManager, spawnerManager, formationType, numberOfShipsInFormation,
    formationRadius, formationShipName)
    {
        this.bossNamePhase2 = bossNamePhase2;
    }

    public override void StartLevel()
    {
        StartPhase1();
    }

    public override void UpdateLevel()
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                if (ShouldTransitionToPhase2())
                {
                    StartPhase2();
                }
                break;
            case BossPhase.Phase2:
                if (ShouldTransitionToPhase3())
                {
                    StartPhase3();
                }
                break;
            case BossPhase.Phase3:
                if (IsBossDefeated())
                {
                    CompleteLevel();
                }
                break;
        }
    }

    void StartPhase1()
    {
        // Start the spawning of enemies
        spawnerManager.StartCoroutine(spawnerManager.SpawnEnemiesWaves(1, 30f));

        // Play the phase 1 music
        spawnerManager.StartCoroutine(Background.Instance.PlayThraxBossPhase1Music());

        // Spawn the boss in idle mode and grab the boss' health and kinematics
        bossShip = spawnerManager.SpawnShip(bossName, spawnPoints[Random.Range(0, spawnPoints.Count)], Quaternion.identity);
        bossHealthPhase1 = bossShip.GetComponent<Health>();
        bossKinematicsPhase1 = bossShip.GetComponent<Kinematics>();
        cameraFollow = Camera.main.GetComponent<CameraFollowBehaviour>();
        bossHealthPhase1.isDead = true;
        bossKinematicsPhase1.ShouldMove = false;

        // Spawn the second boss in idle mode and grab the boss' health and kinematics
        bossShipPhase2 = spawnerManager.SpawnShip(bossNamePhase2, spawnPoints[Random.Range(0, spawnPoints.Count)], Quaternion.identity);
        bossHealthPhase2 = bossShipPhase2.GetComponent<Health>();
        bossKinematicsPhase2 = bossShipPhase2.GetComponent<Kinematics>();
        bossHealthPhase2.isDead = true;
        bossKinematicsPhase2.ShouldMove = false;

        // Set the boss' stats
        SetBossShooterStats(bossShip);

        currentPhase = BossPhase.Phase1;
    }
    bool ShouldTransitionToPhase3()
    {
        return bossHealthPhase1.CurrentHealth <= bossHealthPhase1.MaxHealth / bossShip.GetComponent<BossPhaseController>().PhaseThreshold;
    }
    void StartPhase2()
    {
        Debug.Log("Phase 2 Started");

        bossHealthPhase1.isDead = false;
        bossKinematicsPhase1.ShouldMove = true;

        // Pan camera to boss and play Phase 2 music
        CameraShake.Instance.TriggerShakeMid(3f);
        cameraFollow.StartCoroutine(cameraFollow.PanToTargetAndBack(bossShip.transform, 10f));
        UIManager.Instance.bossHealthBar.gameObject.SetActive(true);
        cameraFollow.StartCoroutine(Background.Instance.PlayThraxBossPhase2Music());

        currentPhase = BossPhase.Phase2;
    }

    void StartPhase3()
    {
        GameObject bossShipPhase2 = spawnerManager.SpawnShip(bossNamePhase2, spawnPoints[Random.Range(0, spawnPoints.Count)], Quaternion.identity);
        // bossShipPhase2.transform.localScale = Vector3.zero;
        bossHealthPhase2 = bossShipPhase2.GetComponent<Health>();
        bossKinematicsPhase2 = bossShipPhase2.GetComponent<Kinematics>();
        bossHealthPhase2.isDead = true;
        bossKinematicsPhase2.ShouldMove = false;
        bossShipPhase2.GetComponent<Health>().isDead = true;
        GameObject portal = ObjectPooler.Instance.SpawnFromPool("ThraxPortal", bossShipPhase2.transform.position, Quaternion.identity);
    }

    bool ShouldTransitionToPhase2()
    {
        return spawnerManager.EnemiesList.Count == 1;
    }

    bool IsBossDefeated()
    {
        return bossShipPhase2 == null || bossHealthPhase2.CurrentHealth <= 0 || !bossShipPhase2.activeInHierarchy;
    }

    public override void CompleteLevel()
    {
        Debug.Log("Completing Level");
        levelManager.CompleteLevel();
    }


}
