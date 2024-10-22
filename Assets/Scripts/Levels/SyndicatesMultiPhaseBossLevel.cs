using System.Collections;
using System.Collections.Generic;
using Mkey;
using UnityEngine;
using UnityEngine.AI;

public class SyndicatesMultiPhaseBossLevel : SoloShooterBossLevel
{
    enum BossPhase
    {
        Phase1,
        Phase2
    }
    public bool phase1Complete = false;
    public bool phase2Complete = false;
    public bool hasTransitionedPhase2 = false;
    BossPhase currentPhase = BossPhase.Phase1;
    Kinematics bossKinematicsPhase1;
    Health bossHealthPhase1;
    AttackManager bossAttackManagerPhase1;
    NavMeshAgent navMeshAgent;
    BossShooter bossShooter;
    public SyndicatesMultiPhaseBossLevel(float health, int bulletAmount,
                                        float bulletDamage, float bulletSpeed,
                                        float firerate, float speed,
                                        float stopDistance, float attackRange,
                                        float fireAngle, float currencyDrop,
                                        List<Vector3> spawnPoints, string bossName,
                                        LevelManager levelManager, SpawnerManager spawnerManager,
                                        FormationType formationType, int numberOfShipsInFormation,
                                        float formationRadius, List<string> formationShipName) :
                                        base(health, bulletAmount, bulletDamage,
                                        bulletSpeed, firerate, speed, stopDistance,
                                        attackRange, fireAngle, currencyDrop,
                                        spawnPoints, bossName, levelManager,
                                        spawnerManager, formationType,
                                        numberOfShipsInFormation,
                                        formationRadius, formationShipName)
    {

    }

    public override void StartLevel()
    {
        SpawnerManager.Instance.StartCoroutine(StartPhase1());
    }

    public override void UpdateLevel()
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                if (ShouldTransitionToPhase2())
                {
                    GameManager.Instance.StartCoroutine(TransitionToPhase2());
                }
                break;
            case BossPhase.Phase2:
                if (IsBossDefeated())
                {
                    CompleteLevel();
                }
                break;

        }
    }

    IEnumerator StartPhase1()
    {
        // Spawn 2 waves of ships simultaneously
        SpawnerManager.Instance.StartCoroutine(SpawnerManager.Instance.SpawnEnemiesWaves(6, 1f, 10));

        // Start Phase 1 music
        Background.Instance.PlaySyndicatesBossPhase1Music();
        yield return new WaitForSeconds(3f);
        CameraFollowBehaviour.Instance.IncreasePlayerOrthographicSize(75, 6f);
        // Spawn the boss in idle mode
        bossShip = spawnerManager.SpawnShip(bossName, SpawnerManager.Instance.SoloBossSpawnPoints[Random.Range(0, SpawnerManager.Instance.SoloBossSpawnPoints.Count)], Quaternion.identity);
        bossShooter = bossShip.GetComponent<BossShooter>();
        navMeshAgent = bossShip.GetComponent<NavMeshAgent>();
        bossHealthPhase1 = bossShip.GetComponent<Health>();
        bossKinematicsPhase1 = bossShip.GetComponent<Kinematics>();
        bossAttackManagerPhase1 = bossShip.GetComponent<AttackManager>();

        // Boss stats and initial state
        bossHealthPhase1.isDead = true;
        bossKinematicsPhase1.ShouldMove = false;
        navMeshAgent.enabled = false;



        currentPhase = BossPhase.Phase1;
        SetBossStats();
    }

    void SetBossStats()
    {
        bossShooter.BulletAmount = bulletAmount;
        bossShooter.BulletDamage = bulletDamage;
        bossShooter.BulletSpeed = bulletSpeed;
        bossShooter.ShootingAngle = fireAngle;
        bossHealthPhase1.MaxHealth = health;
        bossHealthPhase1.CurrentHealth = health;
        bossHealthPhase1.CurrencyDrop = currencyDrop;
        bossKinematicsPhase1.Speed = speed;
        bossKinematicsPhase1.StopDistance = stopDistance;
        bossAttackManagerPhase1.AttackCooldown = fireRate;
        bossAttackManagerPhase1.AimRange = attackRange;
    }

    IEnumerator TransitionToPhase2()
    {
        if (hasTransitionedPhase2) yield break;
        hasTransitionedPhase2 = true;
        // Pan camera to boss and play Phase 2 music
        UIManager.Instance.syndicatesBossHealthBar.gameObject.SetActive(true);
        Debug.Log("Panning Camera to Boss");
        GameManager.Instance.StartCoroutine(CameraFollowBehaviour.Instance.PanToTargetAndBack(bossShip.transform, 8f));
        Debug.Log("Panned Camera to Boss");
        Background.Instance.PlaySyndicatesBossPhase2Music();
        bossHealthPhase1.isDead = false;
        bossKinematicsPhase1.ShouldMove = true;
        navMeshAgent.enabled = true;
        yield return new WaitForSeconds(10f);
        bossShip.GetComponent<AbilityHolder>().abilities[0].isUnlocked = true;
        bossShip.GetComponent<AbilityHolder>().abilities[1].isUnlocked = true;
        currentPhase = BossPhase.Phase2;
    }

    bool ShouldTransitionToPhase2()
    {
        return SpawnerManager.Instance.EnemiesList.Count == 1;
    }

    bool IsBossDefeated()
    {
        return bossShip == null || bossHealthPhase1.isDead || bossHealthPhase1.CurrentHealth <= 0;
    }

    public override void CompleteLevel()
    {
        UIManager.Instance.syndicatesBossHealthBar.gameObject.SetActive(false);
        levelManager.CompleteLevel();
    }

}
