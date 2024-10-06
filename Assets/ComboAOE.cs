using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAOE : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates") || other.CompareTag("CrimsonFleet"))
        {
            other.GetComponent<IDamageable>().TakeDamage(1000);
        }
    }
}
