using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserSettings : MonoBehaviour
{


    float dps;
    private void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && other.CompareTag("Enemy"))
        {
            damageable.TakeDamage(dps);
            Debug.Log("Laser hit enemy for " + dps + " damage");
        }
    }
    public void SetDamage(float damage)
    {
        this.dps = damage;
    }
}
