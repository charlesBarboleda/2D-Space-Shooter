using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserSettings : MonoBehaviour
{

    AudioSource _audioSource;
    AudioClip _audioClip;
    float dps;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
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
