using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThraxMultiPhaseBossLevel : SoloShooterBossLevel
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
    AttackManager bossAttackManagerPhase1;
    Health bossHealthPhase2;
    GameObject bossShipPhase2;
    CameraFollowBehaviour cameraFollow;
    NavMeshAgent navMeshAgent;

    public ThraxMultiPhaseBossLevel(float health, int bulletAmount, float bulletDamage, float bulletSpeed,
    float firerate, float speed, float stopDistance, float attackRange, float fireAngle,
    float currencyDrop, List<Vector3> spawnPoints, string bossName, LevelManager levelManager,
    SpawnerManager spawnerManager, FormationType formationType, int numberOfShipsInFormation,
    float formationRadius, List<string> formationShipName, string bossNamePhase2) : base(health, bulletAmount, bulletDamage,
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
                    cameraFollow.StartCoroutine(TransitionToPhase2());
                }
                break;
            case BossPhase.Phase2:
                if (IsBossHealthLow())
                {
                    cameraFollow.StartCoroutine(TransitionToPhase3(5f));
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
        spawnerManager.StartCoroutine(spawnerManager.SpawnEnemiesWaves(7, 10f));

        // Play phase 1 music
        Background.Instance.PlayThraxBossPhase1Music();

        // Spawn the boss in idle mode
        bossShip = spawnerManager.SpawnShip(bossName, SpawnerManager.Instance.SoloBossSpawnPoints[Random.Range(0, SpawnerManager.Instance.SoloBossSpawnPoints.Count)], Quaternion.identity);
        navMeshAgent = bossShip.GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
        bossHealthPhase1 = bossShip.GetComponent<Health>();
        bossKinematicsPhase1 = bossShip.GetComponent<Kinematics>();
        bossAttackManagerPhase1 = bossShip.GetComponent<AttackManager>();
        cameraFollow = Camera.main.GetComponent<CameraFollowBehaviour>();


        // Boss stats and initial state
        bossHealthPhase1.isDead = true;
        bossKinematicsPhase1.ShouldMove = false;



        currentPhase = BossPhase.Phase1;
        SetBossStats();
    }

    void SetBossStats()
    {
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
        Debug.Log("Transitioning to Phase 2");
        navMeshAgent.enabled = true;
        bossHealthPhase1.isDead = false; // Activate the boss
        bossKinematicsPhase1.ShouldMove = true;

        // Pan camera to boss and play Phase 2 music
        Background.Instance.PlayThraxBossPhase2Music();
        UIManager.Instance.bossHealthBar.gameObject.SetActive(true);
        yield return cameraFollow.StartCoroutine(cameraFollow.PanToTargetAndBack(bossShip.transform, 11f));
        // Unlock the boss' first and second abilities
        yield return new WaitForSeconds(5f);
        bossShip.GetComponent<AbilityHolder>().abilities[0].isUnlocked = true;
        bossShip.GetComponent<AbilityHolder>().abilities[1].isUnlocked = true;

        currentPhase = BossPhase.Phase2;
    }

    IEnumerator TransitionToPhase3(float phaseTransitionDuration)
    {
        if (hasTransitionedPhase3) yield break;
        hasTransitionedPhase3 = true;
        Debug.Log("Transitioning to Phase 3");
        // Disable the UI panels
        UIManager.Instance.DeactivateAllUIPanels();
        // Disable boss attacks while transitioning
        bossAttackManagerPhase1.IsSilenced = true;
        // Disable turret attacks while transitioning
        AttackManager[] turretManager = bossShip.GetComponentsInChildren<AttackManager>();
        foreach (AttackManager turret in turretManager)
        {
            turret.IsSilenced = true;
        }
        UIManager.Instance.bossHealthBar.gameObject.SetActive(false);
        // Play Phase 3 music and disable boss movement and health
        Background.Instance.PlayThraxBossPhase3Music();

        navMeshAgent.enabled = false;
        bossKinematicsPhase1.ShouldMove = false; // Disable boss movement
        bossKinematicsPhase1.ShouldRotate = false; // Disable boss rotation
        bossHealthPhase1.isDead = true; // Mark boss as "dead" to stop its actions

        // Spawn portal
        GameObject portal = ObjectPooler.Instance.SpawnFromPool("ThraxPortal", bossShip.transform.position, Quaternion.identity);
        Vector3 expandedScale = portal.transform.localScale;
        // Pan camera to portal
        cameraFollow.ActivateTargetCamera(portal.transform);
        cameraFollow.ShakeTargetCamera(5f, 2f, 15f);

        Vector3 initialScale = Vector3.zero;
        portal.transform.localScale = initialScale;

        // Expand portal
        float elapsedTime = 0f;
        while (elapsedTime < phaseTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / phaseTransitionDuration);
            portal.transform.localScale = Vector3.Lerp(initialScale, expandedScale, progress); // Assuming the original size is (1, 1, 1)
            yield return null;
        }

        // Boss shrinks to portal
        elapsedTime = 0f;
        Vector3 bossInitialScale = bossShip.transform.localScale;
        while (elapsedTime < phaseTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / phaseTransitionDuration);
            bossShip.transform.localScale = Vector3.Lerp(bossInitialScale, Vector3.zero, progress);
            yield return null;
        }

        // Deactivate the portal and the boss after the boss shrinks
        bossShip.SetActive(false);
        portal.SetActive(false);

        // Now spawn the next portal
        GameObject nextPortal = ObjectPooler.Instance.SpawnFromPool("ThraxPortal", SpawnerManager.Instance.SoloBossSpawnPoints[Random.Range(0, SpawnerManager.Instance.SoloBossSpawnPoints.Count)], Quaternion.identity);
        Vector3 expandedSize = nextPortal.transform.localScale;
        // Pan the camera to the next portal
        cameraFollow.ActivateTargetCamera(nextPortal.transform);
        nextPortal.transform.localScale = Vector3.zero;
        elapsedTime = 0f;

        // Expand next portal
        while (elapsedTime < phaseTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / phaseTransitionDuration);
            nextPortal.transform.localScale = Vector3.Lerp(Vector3.zero, expandedSize, progress);
            yield return null;
        }

        // Spawn new boss ship with size zero
        bossShipPhase2 = spawnerManager.SpawnShip(bossNamePhase2, nextPortal.transform.position, Quaternion.identity);
        NavMeshAgent navMeshAgent2 = bossShipPhase2.GetComponent<NavMeshAgent>();
        navMeshAgent2.enabled = false;
        Vector3 bossExpandedSize = bossShipPhase2.transform.localScale;
        bossShipPhase2.transform.localScale = Vector3.zero;

        // Expand the new boss ship
        elapsedTime = 0f;
        while (elapsedTime < phaseTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / phaseTransitionDuration);
            bossShipPhase2.transform.localScale = Vector3.Lerp(Vector3.zero, bossExpandedSize, progress);
            yield return null;
        }

        // Enable movement and health systems for the new boss
        navMeshAgent2.enabled = true;
        bossShipPhase2.GetComponent<Kinematics>().ShouldMove = true;
        bossShipPhase2.GetComponent<Health>().isDead = false;

        // Disable the portal
        portal.SetActive(false);

        // Pan camera to new boss ship
        yield return cameraFollow.StartCoroutine(cameraFollow.PanToTargetAndBack(bossShipPhase2.transform, 3f));

        // Enable the UI panels
        UIManager.Instance.bossHealthBar.gameObject.SetActive(true);
        UIManager.Instance.ActivateAllUIPanels();

        yield return new WaitForSeconds(15f);
        // Unlock the boss' first ability
        bossShipPhase2.GetComponent<AbilityHolder>().abilities[0].isUnlocked = true;
        currentPhase = BossPhase.Phase3;
    }

    bool ShouldTransitionToPhase2()
    {
        return spawnerManager.EnemiesList.Count == 1; // Phase 1 complete when all ships are destroyed but 1
    }

    bool IsBossHealthLow()
    {
        return bossHealthPhase1.CurrentHealth <= bossHealthPhase1.MaxHealth / 10; // Check if a ninth of the health is gone
    }

    bool IsBossDefeated()
    {
        return bossShipPhase2 == null || !bossShipPhase2.activeInHierarchy || bossShipPhase2.GetComponent<Health>().CurrentHealth <= 0;
    }

    public override void CompleteLevel()
    {
        Debug.Log("Completing Level");
        UIManager.Instance.bossHealthBar.gameObject.SetActive(false);
        levelManager.CompleteLevel();
    }

}
