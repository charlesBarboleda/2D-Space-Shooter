using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCoreBuilding : MonoBehaviour
{
    public int coresNeeded = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnergyCore"))
        {
            other.gameObject.SetActive(false);
            EventManager.CoreEnergizeEvent();
            coresNeeded--;
            if (coresNeeded <= 0)
            {
                DealAOEDamage();
                StartCoroutine(Deactivate());
            }
        }
    }



    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(3f);
        GameObject teleportAnim = ObjectPooler.Instance.SpawnFromPool("BlinkLarge", transform.position, Quaternion.identity);
        NavMeshScript.Instance.UpdateNavMesh();
        yield return new WaitForSeconds(0.1f);
        teleportAnim.SetActive(false);
        gameObject.SetActive(false);
    }
    void DealAOEDamage()
    {
        GameObject damageEffect = ObjectPooler.Instance.SpawnFromPool("ComboAOE", transform.position, Quaternion.identity);
        damageEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);
        damageEffect.transform.SetParent(transform);
        damageEffect.transform.localScale = Vector3.zero;
        StartCoroutine(IncreaseAOEScale(damageEffect, 150f, 50f));
    }
    IEnumerator IncreaseAOEScale(GameObject effect, float size, float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration; // Normalized value between 0 and 1

            // Scale the AOE effect
            effect.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(size, size, size), progress);


            yield return null;
        }

        effect.SetActive(false);
    }
}
