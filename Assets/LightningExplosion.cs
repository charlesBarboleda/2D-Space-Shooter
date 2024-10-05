using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class LightningExplosion : MonoBehaviour
{
    public float speed;
    public float damagePerSecond;
    public float explosionDamage = 999999f;
    public ParticleSystem lightningExplosion;
    public ParticleSystem lightningExplosionSparks;
    Vector3 initScale;

    void OnEnable()
    {

        lightningExplosion.Stop();
        lightningExplosionSparks.Stop();
    }
    void OnDisable()
    {

        lightningExplosion.Stop();
        lightningExplosionSparks.Stop();
    }
    IEnumerator StartChannelCoroutine(Transform owner, Vector3 target, float chargeDuration = 3f)
    {
        initScale = lightningExplosion.transform.localScale;
        lightningExplosion.transform.localScale = Vector3.zero;
        lightningExplosion.Play();
        lightningExplosion.transform.SetParent(owner.transform);
        float t = 0;
        while (t < chargeDuration)
        {
            t += Time.deltaTime;
            lightningExplosion.transform.localScale = Vector3.Lerp(Vector3.zero, initScale, t);
            yield return null;
        }
        lightningExplosion.transform.localScale = initScale;
        lightningExplosion.transform.SetParent(null);
        float t2 = 0;
        while (t2 < 2f)
        {
            t2 += Time.deltaTime;
            lightningExplosion.transform.position = Vector3.MoveTowards(lightningExplosion.transform.position, target, speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(lightningExplosion.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        GameObject animation = ObjectPooler.Instance.SpawnFromPool("BlinkSmall", lightningExplosion.transform.position, Quaternion.identity);
        LayerMask _damageLayerMask = LayerMask.GetMask("Player", "CrimsonFleet", "Syndicates");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(lightningExplosion.transform.position, 30f, _damageLayerMask);

        // Apply damage or effects to all hit objects
        foreach (Collider2D hitCollider in hitColliders)
        {
            // Example of applying damage (assuming objects have a Damageable component)
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(explosionDamage);
            }
        }
        // Check if lightningExplosionSparks exists before trying to access it
        if (lightningExplosionSparks != null)
        {
            lightningExplosionSparks.transform.position = lightningExplosion.transform.position;
            lightningExplosionSparks.Play();
            lightningExplosion.Stop();
            yield return new WaitForSeconds(0.5f);
            lightningExplosionSparks.Stop();
            animation.SetActive(false);

        }
        else
        {
            Debug.LogError("Explosion particles were null before explosion!");
        }

        // Deactivate the GameObject only after the implosion effect has finished
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 30f);
    }




}
