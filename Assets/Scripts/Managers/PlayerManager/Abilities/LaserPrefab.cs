using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPrefab : MonoBehaviour
{
    private float dps;
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && other.CompareTag("Enemy"))
        {
            damageable.TakeDamage(dps);
        }
    }
    public void SetDamage(float damage)
    {
        this.dps = damage;
    }
}
