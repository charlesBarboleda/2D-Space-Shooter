using System.Collections;
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
        Laser.transform.rotation = owner.transform.rotation;
        // Start playing the audio clip
        AudioSource ownerAudioSource = owner.GetComponent<AudioSource>();
        if (ownerAudioSource == null)
        {
            ownerAudioSource = owner.AddComponent<AudioSource>();
        }
        ownerAudioSource.clip = _audioClip;
        ownerAudioSource.loop = true;
        ownerAudioSource.volume = 0.5f;
        ownerAudioSource.Play();
        Laser.transform.rotation = owner.transform.rotation;
        Laser.transform.SetParent(owner.transform);

        //Pass the damage value to the laser
        PlayerLaserSettings laserScript = Laser.GetComponent<PlayerLaserSettings>();
        laserScript.Dps = dps;
        owner.GetComponent<MonoBehaviour>().StartCoroutine(HandleLaser(Laser, owner, ownerAudioSource));

    }
    IEnumerator HandleLaser(GameObject laser, GameObject owner, AudioSource ownerAudioSource)
    {
        float timer = 0f;

        while (timer < duration)
        {
            // Get mouse position in the world
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the mouse position stays in the same plane as the player

            // Calculate direction from owner to mouse position
            Vector3 direction = (mousePosition - owner.transform.position).normalized;

            // Rotate the laser to face the mouse cursor
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            laser.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            // Make the laser follow the owner
            // laser.transform.position = owner.transform.position;

            // Increment timer
            timer += Time.deltaTime;
            yield return null;
        }

        // After the duration, stop the audio and deactivate the laser
        ownerAudioSource.Stop();
        laser.SetActive(false);
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
