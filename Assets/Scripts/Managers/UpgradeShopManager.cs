using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeShopManager : MonoBehaviour
{
    public static UpgradeShopManager Instance;
    public static HealthUpgrade healthUpgrade;
    [SerializeField] GameObject upgradeShopPanel;


    void Awake()
    {

        healthUpgrade = new HealthUpgrade();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        healthUpgrade.Initialize(
            "Increase Max Health",
            $"Increase player health by {healthUpgrade.healthUpgradeAmount}",
            100
        );
    }

    public void OpenUpgradeShop()
    {
        upgradeShopPanel.SetActive(true);
        Time.timeScale = 0;
    }


    public void ExitUpgradeShop()
    {
        upgradeShopPanel.SetActive(false);
        Debug.Log("Exit Upgrade Shop");
        Time.timeScale = 1;
    }

    public void ApplyHealthUpgrade()
    {
        healthUpgrade.ApplyUpgrade();
    }


}

