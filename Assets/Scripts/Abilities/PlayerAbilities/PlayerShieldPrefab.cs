using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldPrefab : MonoBehaviour
{
    private float _dps;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamageable>().TakeDamage(_dps * Time.deltaTime);
        }
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            other.gameObject.SetActive(false);
        }
    }
    public void SetDamage(float dps)
    {
        this._dps = dps;
    }

}
