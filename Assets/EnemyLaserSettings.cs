using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserSettings : MonoBehaviour
{

    float _dps;

    private void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (other.CompareTag("Player") || other.CompareTag("EnemyDestroyable") || other.CompareTag("CrimsonFleet") || other.CompareTag("Syndicates"))
            {
                damageable.TakeDamage(_dps);
            }
        }


    }
    public float Dps { get => _dps; set => _dps = value; }




}
