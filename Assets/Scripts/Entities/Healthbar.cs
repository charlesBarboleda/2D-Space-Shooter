using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image image;
    public float currentHealth { get; private set; }
    public float maxHealth { get; private set; }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        image.fillAmount = currentHealth / maxHealth;
    }
}
