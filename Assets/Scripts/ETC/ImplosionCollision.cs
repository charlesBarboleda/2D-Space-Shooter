using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImplosionCollision : MonoBehaviour
{
    [SerializeField] EnemyVoidImplosion _implosionScript;

    private void OnParticleCollision(GameObject other)
    {
        // You can apply damage or any effect to the object here
        if (other.CompareTag("Player") || other.CompareTag("CrimsonFleet") || other.CompareTag("Syndicates"))
        {
            IDamageable iDamageable = other.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.TakeDamage(_implosionScript.Damage);
            }

        }
    }
}
