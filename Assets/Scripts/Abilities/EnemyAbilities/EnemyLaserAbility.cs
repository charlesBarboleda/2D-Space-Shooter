using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLaserAbility", menuName = "EnemyAbilities/EnemyLaserAbility")]
public class EnemyLaserAbility : Ability
{
    [SerializeField] float damagePerSecond = 10f;
    List<Transform> _laserSpawnPoints;
    [SerializeField] string _laserPoolTag = "ThraxLaser";
    private GameObject _owner;
    private Health _ownerHealth; // Reference to the Health component

    public override async void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false)
    {
        _owner = owner;
        _ownerHealth = _owner.GetComponent<Health>(); // Get the Health component

        // Ensure the owner has the Health component and isn't already dead
        if (_ownerHealth == null || _ownerHealth.isDead)
        {
            return; // Exit early if the owner is dead
        }

        // Get the spawn points from the enemy ship (owner)
        _laserSpawnPoints = GetLaserSpawnPoints(owner);
        if (_laserSpawnPoints.Count == 0)
        {
            return;
        }

        SetStatsBasedOnLevel();

        // Spawn lasers at each spawn point simultaneously
        List<Task> laserTasks = new List<Task>();
        foreach (Transform spawnPoint in _laserSpawnPoints)
        {
            laserTasks.Add(SpawnLaser(target, spawnPoint));
        }

        // Wait for all lasers to complete their duration
        await Task.WhenAll(laserTasks);
    }

    private async Task SpawnLaser(Transform target, Transform spawnPoint)
    {
        Vector3 worldPosition = spawnPoint.TransformPoint(Vector3.zero);

        // Calculate the direction from the spawn point to the target
        Vector3 laserDirection = -(target.position - worldPosition).normalized;

        // Calculate the rotation needed to face the target direction
        Quaternion laserRotation = Quaternion.LookRotation(laserDirection);

        // Spawn laser from the pool at the correct world position
        GameObject _laser = ObjectPooler.Instance.SpawnFromPool(_laserPoolTag, worldPosition, laserRotation);

        // Assign laser settings
        EnemyLaserSettings laserScript = _laser.GetComponent<EnemyLaserSettings>();
        laserScript.Dps = damagePerSecond;

        // Keep the laser active and follow the spawn point for the duration
        await FollowSpawnPoint(_laser, spawnPoint);

        // Deactivate the laser after the duration
        _laser.SetActive(false);
    }

    private async Task FollowSpawnPoint(GameObject laser, Transform spawnPoint)
    {
        float timeElapsed = 0f;

        // Keep updating the laser's position and rotation until the duration ends or the owner is dead
        while (timeElapsed < duration)
        {
            // Check if the owner is dead
            if (_ownerHealth.isDead)
            {
                break; // Exit if the owner is dead
            }

            laser.transform.position = spawnPoint.position;
            laser.transform.rotation = spawnPoint.rotation;

            // Wait for the next frame
            await Task.Yield();
            timeElapsed += Time.deltaTime;
        }

        // Deactivate the laser if the owner is dead or after the duration ends
        laser.SetActive(false);
    }

    private List<Transform> GetLaserSpawnPoints(GameObject owner)
    {
        List<Transform> laserPoints = new List<Transform>();

        foreach (Transform child in owner.transform)
        {
            if (child.CompareTag("LaserSpawnPoint")) // Ensure these points have a specific tag
            {
                laserPoints.Add(child);
            }
        }
        return laserPoints;
    }

    public override void ResetStats()
    {
        damagePerSecond = 10f;
        duration = 5f;
    }

    private void SetStatsBasedOnLevel()
    {
        damagePerSecond += LevelManager.Instance.CurrentLevelIndex * 0.5f;
        duration += LevelManager.Instance.CurrentLevelIndex * 0.1f;
    }
}
