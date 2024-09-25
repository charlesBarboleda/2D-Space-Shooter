using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image image;
    public Health health;
    public PlayerHealthBehaviour playerHealth;

    void Update()
    {
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        image.fillAmount = playerHealth != null ? Mathf.Lerp(image.fillAmount, playerHealth.currentHealth, playerHealth.maxHealth) : Mathf.Lerp(image.fillAmount, health.CurrentHealth / health.MaxHealth, 0.1f);

    }
}
