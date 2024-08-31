using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Laser")]
public class AbilityLaser : Ability
{

    [SerializeField] GameObject _laserPrefab;
    public float duration;
    public float dps;

    // Start is called before the first frame update
    public override void AbilityLogic(GameObject owner, Transform target)
    {
        //Object Pool the laser prefab
        GameObject Laser = ObjectPooler.Instance.SpawnFromPool("PlayerLaser", owner.transform.position, Quaternion.identity);
        // Start playing the audio clip
        AudioSource ownerAudioSource = owner.GetComponent<AudioSource>();
        if (ownerAudioSource == null)
        {
            ownerAudioSource = owner.AddComponent<AudioSource>();
        }
        ownerAudioSource.clip = _audioClip;
        ownerAudioSource.loop = true;
        ownerAudioSource.Play();
        Laser.transform.rotation = owner.transform.rotation;
        Laser.transform.SetParent(owner.transform);

        //Pass the damage value to the laser
        PlayerLaserSettings laserScript = Laser.GetComponent<PlayerLaserSettings>();
        laserScript.SetDamage(dps);
        owner.GetComponent<MonoBehaviour>().StartCoroutine(DeactivateLaserAfterDuration(Laser, ownerAudioSource));

    }
    private IEnumerator DeactivateLaserAfterDuration(GameObject laser, AudioSource ownerAudioSource)
    {
        // Wait for the duration of the laser
        yield return new WaitForSeconds(duration);

        // Stop the audio and destroy the laser
        ownerAudioSource.Stop();
        Destroy(laser);
    }

    public override void ResetStats()
    {
        currentCooldown = 45f;
        duration = 3f;
        dps = 5f;
        cooldown = 45f;
        isUnlocked = false;
    }
}
