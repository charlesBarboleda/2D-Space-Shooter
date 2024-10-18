using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosionEffect : MonoBehaviour
{
    public float damage;
    public float corrodeRadius = 20f;
    public float pulseInterval = 1f;
    public int pulseCount = 5;
    public bool shouldIncreaseCombo;

    public LayerMask layersToScan;
    IDamageable damageable;
    EnemyDebuffs enemyDebuffs;
    GameObject Corrosion;


    void OnDisable()
    {
        if (Corrosion != null) Corrosion.SetActive(false);
    }
    public void ApplyCorrode(GameObject target, float damage)
    {
        Corrosion = ObjectPooler.Instance.SpawnFromPool("Corrosion", target.transform.position, Quaternion.identity);
        Corrosion.transform.parent = target.transform;
        StartCoroutine(CorrodeEffect(target.transform, damage));
    }
    IEnumerator CorrodeEffect(Transform target, float damage)
    {
        for (int i = 0; i < pulseCount; i++)
        {
            // Spawn the Corrosion Pulse
            GameObject CorrodePulse = ObjectPooler.Instance.SpawnFromPool("CorrosionPulse", target.position, Quaternion.identity);
            CorrodePulse.transform.parent = target;
            // Scan for enemies to corrode and deal damage
            RaycastHit2D[] hitObjects = Physics2D.CircleCastAll(target.position, corrodeRadius, Vector2.zero, Mathf.Infinity, layersToScan);
            foreach (RaycastHit2D hit in hitObjects)
            {
                if (hit.collider.CompareTag("ThraxArmada") || hit.collider.CompareTag("Syndicates") || hit.collider.CompareTag("CrimsonFleet") || hit.collider.CompareTag("Asteroid"))
                {
                    damageable = hit.collider.GetComponent<IDamageable>();
                    enemyDebuffs = hit.collider.GetComponent<EnemyDebuffs>();

                    if (damageable != null)
                    {
                        if (gameObject.CompareTag("PlayerBullet"))
                        {
                            EventManager.BulletDamageEvent(damage);
                            EventManager.PlayerDamageDealtEvent(damage);
                            if (shouldIncreaseCombo)
                                ComboManager.Instance.IncreaseCombo();
                        }
                        UIManager.Instance.CreateOnHitDamageText(Mathf.Round(damage).ToString(), hit.collider.transform.position);
                        damageable.TakeDamage(damage);
                        if (!enemyDebuffs.debuffsList["Corrode"])
                        {
                            enemyDebuffs.ApplyDebuff("Corrode");
                            ApplyCorrode(hit.collider.gameObject, damage);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(pulseInterval);
            CorrodePulse.SetActive(false);
        }
        if (Corrosion.activeSelf)
        {
            Debug.Log("Corrosion Still Active...Disabling");
            Corrosion.SetActive(false);
            Debug.Log("Disabled Corrosion");
        }
        enemyDebuffs.RemoveDebuff("Corrode");
        gameObject.SetActive(false);
    }
}
