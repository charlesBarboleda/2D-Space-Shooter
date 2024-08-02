using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] public GameObject shield;
    public float shieldTimer;

    // Update is called once per frame
    void Update()
    {
        if (shieldTimer > 0)
        {
            shieldTimer -= Time.deltaTime;
            ActivateShield();
        }
        else
        {
            shieldTimer = 0;
            DisableShield();
        }
    }


    private void ActivateShield()
    {
        shield.SetActive(true);
    }

    private void DisableShield()
    {
        shield.SetActive(false);
    }

    public void AddShieldTime(float time)
    {
        shieldTimer += time;
    }

}
