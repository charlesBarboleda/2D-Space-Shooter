using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        SetHealth();
    }
    public void SetMaxHealth()
    {
        slider.maxValue = playerManager.playerHealth;
    }
    public void SetHealth()
    {
        slider.value = playerManager.playerHealth;


    }
}
