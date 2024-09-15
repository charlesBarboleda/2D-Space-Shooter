using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldPrefab : MonoBehaviour
{
    float _dps;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates") || other.CompareTag("CrimsonFleet"))
        {
            other.gameObject.GetComponent<IDamageable>().TakeDamage(_dps);
        }
    }
    public float Dps { get => _dps; set => _dps = value; }

}
