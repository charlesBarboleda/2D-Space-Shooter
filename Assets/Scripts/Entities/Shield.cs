using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldHealth;
    [SerializeField] private PlayerManager playerManager;

    private void OnEnable()
    {
        shieldHealth += 99999f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            shieldHealth -= other.GetComponent<Bullet>().BulletDamage;
            other.gameObject.SetActive(false);

        }
    }

}
