using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class LightningExplosion : MonoBehaviour
{
    public float duration;
    public float damagePerSecond;
    public ParticleSystem lightningChannel;
    public ParticleSystem lightningExplosion;
    public ParticleSystem lightningExplosionSparks;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Get mouse input in transform position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            StartChannel(PlayerManager.Instance.transform, mousePos);
        }
    }
    void OnEnable()
    {
        lightningChannel.Stop();
        lightningExplosion.Stop();
        lightningExplosionSparks.Stop();
    }
    void OnDisable()
    {
        lightningChannel.Stop();
        lightningExplosion.Stop();
        lightningExplosionSparks.Stop();
    }
    IEnumerator StartChannelCoroutine(Transform owner, Vector3 target)
    {
        lightningChannel.transform.position = owner.transform.position;
        // Rotate the lightning channel to face the player
        Vector3 direction = (target - owner.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lightningChannel.transform.rotation = Quaternion.Euler(0, 0, angle);
        lightningChannel.Play();
        yield return new WaitForSeconds(1f);
        lightningChannel.Stop();
        lightningExplosion.transform.position = target;
        lightningExplosionSparks.transform.position = target;
        lightningExplosion.Play();
        lightningExplosionSparks.Play();
        yield return new WaitForSeconds(duration);
        lightningExplosion.Stop();
        lightningExplosionSparks.Stop();
        gameObject.SetActive(false);
    }

    public void StartChannel(Transform owner, Vector3 target)
    {
        StartCoroutine(StartChannelCoroutine(owner, target));
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("CrimsonFleet") || other.CompareTag("Syndicates"))
        {
            other.GetComponent<IDamageable>().TakeDamage(damagePerSecond);
        }
    }




}
