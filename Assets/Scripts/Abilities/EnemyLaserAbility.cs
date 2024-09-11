using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLaserAbility", menuName = "EnemyAbilities/EnemyLaserAbility")]
public class EnemyLaserAbility : Ability
{
    [SerializeField] float damagePerSecond = 10f;
    [SerializeField] float duration = 5f;
    List<Transform> _laserSpawnPoints;

    public override async void AbilityLogic(GameObject owner, Transform target)
    {
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
        GameObject laser = ObjectPooler.Instance.SpawnFromPool("ThraxLaser", worldPosition, laserRotation);

        // Assign laser settings
        EnemyLaserSettings laserScript = laser.GetComponent<EnemyLaserSettings>();
        laserScript.Dps = damagePerSecond;

        // Keep the laser active and follow the spawn point for the duration
        await FollowSpawnPoint(laser, spawnPoint);

        // Deactivate the laser after the duration
        laser.SetActive(false);
    }

    private async Task FollowSpawnPoint(GameObject laser, Transform spawnPoint)
    {
        float timeElapsed = 0f;

        // Keep updating the laser's position and rotation until the duration ends
        while (timeElapsed < duration)
        {
            laser.transform.position = spawnPoint.position;
            laser.transform.rotation = spawnPoint.rotation;

            // Wait for the next frame
            await Task.Yield();

            timeElapsed += Time.deltaTime;
        }
    }

    // Helper method to get all the laser spawn points from the enemy ship
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

    void SetStatsBasedOnLevel()
    {
        damagePerSecond += GameManager.Instance.Level * 0.5f;
        duration += GameManager.Instance.Level * 0.1f;
    }
}
