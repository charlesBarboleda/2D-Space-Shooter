using System.Collections;
using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(menuName = "Abilities/Laser")]
public class AbilityLaser : Ability
{
    [SerializeField] GameObject _laserPrefab;
    public float dps;
    public float ultimateDpsMultiplier = 3f; // Example: ultimate deals more damage

    public override void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false)
    {
        // Play audio for normal and ultimate abilities
        AudioSource ownerAudioSource = owner.GetComponent<AudioSource>();
        if (ownerAudioSource == null)
        {
            ownerAudioSource = owner.AddComponent<AudioSource>();
        }
        ownerAudioSource.clip = _audioClip;
        ownerAudioSource.loop = true;
        ownerAudioSource.volume = 0.5f;
        ownerAudioSource.Play();

        if (isUltimate)
        {
            UIManager.Instance.ActivateCrackAndShatter();
            GameManager.Instance.StartCoroutine(ActivateUltimateLasers(owner, ownerAudioSource, isUltimate));
            // Spawn lasers in every direction (360 degrees)

        }
        else
        {
            // Normal ability (single laser logic)
            GameObject Laser = ObjectPooler.Instance.SpawnFromPool("PlayerLaser", owner.transform.position, Quaternion.identity);
            Laser.transform.SetParent(owner.transform);

            // Pass damage values
            PlayerLaserSettings laserScript = Laser.GetComponent<PlayerLaserSettings>();
            laserScript.Dps = dps;

            // Handle normal laser logic
            GameManager.Instance.StartCoroutine(HandleLaser(Laser, owner, ownerAudioSource, false, 0f));
        }
    }

    IEnumerator ActivateUltimateLasers(GameObject owner, AudioSource ownerAudioSource, bool isUltimate = true)
    {
        yield return new WaitForSeconds(1f);
        int numLasers = 8; // You can increase or decrease this number to spawn more lasers
        float angleStep = 360f / numLasers;

        for (int i = 0; i < numLasers; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle); // Set the rotation for each laser

            GameObject laser = ObjectPooler.Instance.SpawnFromPool("PlayerLaser", owner.transform.position, rotation);
            laser.transform.SetParent(owner.transform);

            // Pass the damage value to the laser
            PlayerLaserSettings laserScript = laser.GetComponent<PlayerLaserSettings>();
            laserScript.Dps = dps * ultimateDpsMultiplier;

            // Start the coroutine for each laser to follow the player and rotate accordingly
            GameManager.Instance.StartCoroutine(HandleLaser(laser, owner, ownerAudioSource, isUltimate, angle));
        }
    }


    IEnumerator HandleLaser(GameObject laser, GameObject owner, AudioSource ownerAudioSource, bool isUltimate, float initialAngle)
    {
        float timer = 0f;
        float maxDuration = isUltimate ? ultimateDuration : duration; // Ultimate lasts longer
        float rotationSpeed = 90f; // Degrees per second

        while (timer < maxDuration)
        {
            timer += Time.deltaTime;
            if (!isUltimate)
            {
                // Normal laser follows the mouse cursor logic
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;

                Vector3 direction = (mousePosition - owner.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                laser.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }
            else
            {
                // Ultimate lasers rotate around the player

                // Calculate the angle for continuous rotation over time
                float currentAngle = initialAngle + (rotationSpeed * timer);

                // Calculate the new position based on the angle
                float radius = 1f; // Adjust radius as needed for how far the lasers are from the player
                Vector3 offset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * radius;

                // Update laser position to rotate around the player
                laser.transform.position = owner.transform.position + offset;

                // Rotate the laser to always face outward from the player
                laser.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            yield return null;
        }

        // Stop audio and deactivate laser after the duration
        ownerAudioSource.Stop();
        laser.SetActive(false);
    }



    public override void ResetStats()
    {
        currentCooldown = 45f;
        currentUltimateCooldown = 60f; // Separate cooldown for ultimate
        duration = 3f;
        dps = 5f;
        ultimateDpsMultiplier = 2f;
        ultimateDuration = 6f;
        cooldown = 45f;
        ultimateCooldown = 180f; // Different cooldown for ultimate
        isUnlocked = false;
    }
}

