using System.Collections;
using System.Collections.Generic;
using AssetUsageDetectorNamespace;
using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    Health _health;
    Kinematics _kinematics;
    bool hasPhased = false;
    [SerializeField] float phaseThreshold = 3f;


    void Awake()
    {
        _health = GetComponent<Health>();
        _kinematics = GetComponent<Kinematics>();
    }

    void Update()
    {
        if (_health.CurrentHealth < _health.MaxHealth / phaseThreshold)
        {
            if (!hasPhased)
            {

                PhaseTransition();

            }
        }
    }
    IEnumerator PhaseTransition()
    {
        _kinematics.ShouldMove = false;
        _kinematics.ShouldRotate = false;
        _health.isDead = true;
        yield return PhaseTransitionOut(3f);
        hasPhased = true;
        yield return PhaseTransitionIn(3f);
    }
    IEnumerator PhaseTransitionIn(float duration)
    {
        GameObject portal = ObjectPooler.Instance.SpawnFromPool("ThraxPortal", transform.position, Quaternion.identity);
        portal.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(3f);
        float t = 0;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = transform.localScale;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        portal.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1f);
        portal.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator PhaseTransitionOut(float duration)
    {
        GameObject portal = ObjectPooler.Instance.SpawnFromPool("ThraxPortal", transform.position, Quaternion.identity);
        portal.transform.localScale = Vector3.zero;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = portal.transform.localScale;
        float t = 0;
        // Increase the size of the portal to the size of the boss using lerp
        while (t < duration)
        {
            t += Time.deltaTime;
            portal.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        // Reduce the size of the boss to zero using lerp
        t = 0;
        startScale = transform.localScale;
        endScale = Vector3.zero;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        // Stop the portal particles and deactivate the portal
        portal.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1f);
        portal.SetActive(false);
        gameObject.SetActive(false);
    }

    public float PhaseThreshold { get => phaseThreshold; set => phaseThreshold = value; }

}
