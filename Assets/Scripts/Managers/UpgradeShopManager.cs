using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class UpgradeShopManager : MonoBehaviour
{
    public static UpgradeShopManager Instance;
    public static HealthUpgrade healthUpgrade;
    public static BulletDamageUpgrade bulletDamageUpgrade;
    public static FireRateUpgrade fireRateUpgrade;
    public static BulletSpeedUpgrade bulletSpeedUpgrade;
    [SerializeField] GameObject upgradeShopPanel;


    void Awake()
    {
        bulletSpeedUpgrade = new BulletSpeedUpgrade();
        fireRateUpgrade = new FireRateUpgrade();
        bulletDamageUpgrade = new BulletDamageUpgrade();
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
        bulletDamageUpgrade.Initialize(
            "Increase Damage",
            $"Increase Bullet Damage by {bulletDamageUpgrade.bulletDamageUpgradeAmount}",
            100
        );
        fireRateUpgrade.Initialize(
            "Decrease Fire Rate",
            $"Decrease Fire Rate by {fireRateUpgrade.fireRateUpgradeAmount}",
            100
        );
        bulletSpeedUpgrade.Initialize(
            "Increase Bullet Speed",
            $"Increase Bullet Speed by {bulletSpeedUpgrade.bulletSpeedUpgradeAmount}",
            100
        );

    }

    void Update()
    {
        UpdateDescriptionText(healthUpgrade, $"Increase player health by {healthUpgrade.healthUpgradeAmount}");
        UpdateDescriptionText(bulletDamageUpgrade, $"Increase Bullet Damage by {bulletDamageUpgrade.bulletDamageUpgradeAmount}");
        UpdateDescriptionText(fireRateUpgrade, $"Decrease Fire Rate by {fireRateUpgrade.fireRateUpgradeAmount}");
        UpdateDescriptionText(bulletSpeedUpgrade, $"Increase Bullet Speed by {bulletSpeedUpgrade.bulletSpeedUpgradeAmount}");




    }

    private void UpdateDescriptionText(Upgrade upgrade, string description)
    {
        upgrade.upgradeDescription = description;
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

    public void ApplyBulletDamageUpgrade()
    {
        bulletDamageUpgrade.ApplyUpgrade();
    }

    public void ApplyFireRateUpgrade()
    {
        fireRateUpgrade.ApplyUpgrade();
    }

    public void ApplyBulletSpeedUpgrade()
    {
        bulletSpeedUpgrade.ApplyUpgrade();
    }




}

