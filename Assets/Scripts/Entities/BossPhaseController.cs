using System.Collections;
using System.Collections.Generic;
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
                _kinematics.ShouldMove = false;
                _kinematics.ShouldRotate = false;
                _health.isDead = true;
                StartCoroutine(PhaseTransitionOut());
                hasPhased = true;
            }
        }
    }

    IEnumerator PhaseTransitionOut()
    {
        GameObject portal = ObjectPooler.Instance.SpawnFromPool("ThraxPortal", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        // Reduce the size of the boss to zero using lerp
        float t = 0;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        while (t < 1)
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


}
