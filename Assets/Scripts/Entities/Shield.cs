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
            if (shieldHealth <= 0)
            {
                playerManager.playerShield.shieldTimer = 0;
                DisableShield();
                Debug.Log("Shield is down");
            }
        }
    }

    private void DisableShield()
    {
        playerManager.playerShield.shieldTimer = 0;
        gameObject.SetActive(false);
    }
}
