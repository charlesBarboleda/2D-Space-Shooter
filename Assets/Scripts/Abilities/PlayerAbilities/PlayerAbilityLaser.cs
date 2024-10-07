using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Laser")]
public class AbilityLaser : Ability
{
    [SerializeField] GameObject _laserPrefab;
    public float dps;
    public float ultimateDpsMultiplier = 2f; // Example: ultimate deals more damage

    public override void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false)
    {
        // Object pool the laser prefab
        GameObject Laser = ObjectPooler.Instance.SpawnFromPool("PlayerLaser", owner.transform.position, Quaternion.identity);
        Laser.transform.rotation = owner.transform.rotation;

        // Play audio
        AudioSource ownerAudioSource = owner.GetComponent<AudioSource>();
        if (ownerAudioSource == null)
        {
            ownerAudioSource = owner.AddComponent<AudioSource>();
        }
        ownerAudioSource.clip = _audioClip;
        ownerAudioSource.loop = true;
        ownerAudioSource.volume = 0.5f;
        ownerAudioSource.Play();

        // Attach laser to owner
        Laser.transform.SetParent(owner.transform);

        // Pass damage values: if ultimate, apply multiplier
        PlayerLaserSettings laserScript = Laser.GetComponent<PlayerLaserSettings>();
        laserScript.Dps = isUltimate ? dps * ultimateDpsMultiplier : dps;

        owner.GetComponent<MonoBehaviour>().StartCoroutine(HandleLaser(Laser, owner, ownerAudioSource, isUltimate));
    }

    IEnumerator HandleLaser(GameObject laser, GameObject owner, AudioSource ownerAudioSource, bool isUltimate)
    {
        float timer = 0f;
        float maxDuration = isUltimate ? duration * 2f : duration; // Example: ultimate lasts longer

        while (timer < maxDuration)
        {
            // Follow mouse logic as before
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector3 direction = (mousePosition - owner.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            laser.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            timer += Time.deltaTime;
            yield return null;
        }

        // Stop audio and deactivate laser after duration
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
        cooldown = 45f;
        ultimateCooldown = 60f; // Different cooldown for ultimate
        isUnlocked = false;
    }
}

