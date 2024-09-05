using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image image;

    void Update()
    {
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        image.fillAmount = Mathf.Lerp(image.fillAmount, PlayerManager.GetInstance().CurrentHealth() / PlayerManager.GetInstance().MaxHealth(), 0.1f);

    }
}
