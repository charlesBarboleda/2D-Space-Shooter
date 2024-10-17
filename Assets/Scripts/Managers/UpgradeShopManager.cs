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
    public static ExtraBulletUpgrade extraBulletUpgrade;
    public static BulletSpeedUpgrade bulletSpeedUpgrade;
    public static ShipSpeedUpgrade shipSpeedUpgrade;
    public static PickUpUpgrade pickUpUpgrade;
    [SerializeField] GameObject upgradeShopPanel;
    [SerializeField] AudioClip upgradeSound;
    AudioSource _audioSource;


    void Awake()
    {
        pickUpUpgrade = new PickUpUpgrade();
        shipSpeedUpgrade = new ShipSpeedUpgrade();
        extraBulletUpgrade = new ExtraBulletUpgrade();
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
        _audioSource = GetComponent<AudioSource>();
        InitializeUpgrades();

    }

    void Update()
    {
        UpdateAllUpgradeDescriptionText();

    }

    void InitializeUpgrades()
    {
        healthUpgrade.Initialize(
                   "Increase Max Health",
                   $"Increase player health by {healthUpgrade.healthUpgradeAmount}",
                   50
               );
        bulletDamageUpgrade.Initialize(
            "Increase Damage",
            $"Increase Bullet Damage by {bulletDamageUpgrade.bulletDamageUpgradeAmount}",
            50
        );
        fireRateUpgrade.Initialize(
            "Decrease Fire Rate",
            $"Decrease Fire Rate by {fireRateUpgrade.fireRateUpgradeAmount}",
            50
        );
        bulletSpeedUpgrade.Initialize(
            "Increase Bullet Speed",
            $"Increase Bullet Speed by {bulletSpeedUpgrade.bulletSpeedUpgradeAmount}",
            50
        );
        extraBulletUpgrade.Initialize(
            "Extra Bullet",
            $"Increase Bullet Count by {extraBulletUpgrade.extraBulletUpgradeAmount}",
            2500
        );
        shipSpeedUpgrade.Initialize(
            "Increase Ship Speed",
            $"Increase Ship Speed by {shipSpeedUpgrade.GetShipSpeedUpgradeAmount()} km/h",
            100
        );
        pickUpUpgrade.Initialize(
            "Increase Pick Up Radius",
            $"Increase Pick Up Radius by {pickUpUpgrade.pickUpUpgradeAmount}",
            100
        );

    }

    void UpdateAllUpgradeDescriptionText()
    {
        UpdateDescriptionText(healthUpgrade, $"Increase player health by {healthUpgrade.healthUpgradeAmount}");
        UpdateDescriptionText(bulletDamageUpgrade, $"Increase Bullet Damage by {bulletDamageUpgrade.bulletDamageUpgradeAmount}");
        UpdateDescriptionText(fireRateUpgrade, $"Decrease Fire Rate by {fireRateUpgrade.fireRateUpgradeAmount}");
        UpdateDescriptionText(bulletSpeedUpgrade, $"Increase Bullet Speed by {bulletSpeedUpgrade.bulletSpeedUpgradeAmount}");
        UpdateDescriptionText(extraBulletUpgrade, $"Increase Bullet Count by {extraBulletUpgrade.extraBulletUpgradeAmount}");
        UpdateDescriptionText(shipSpeedUpgrade, $"Increase Ship Speed by {shipSpeedUpgrade.GetShipSpeedUpgradeAmount()} km/h");
        UpdateDescriptionText(pickUpUpgrade, $"Increase Pick Up Radius by {pickUpUpgrade.pickUpUpgradeAmount}");
    }
    private void UpdateDescriptionText(Upgrade upgrade, string description)
    {
        upgrade.upgradeDescription = description;
    }

    public void ApplyHealthUpgrade()
    {
        healthUpgrade.ApplyUpgrade();
        _audioSource.PlayOneShot(upgradeSound);
    }

    public void ApplyBulletDamageUpgrade()
    {
        bulletDamageUpgrade.ApplyUpgrade();
        _audioSource.PlayOneShot(upgradeSound);

    }

    public void ApplyFireRateUpgrade()
    {
        fireRateUpgrade.ApplyUpgrade();
        _audioSource.PlayOneShot(upgradeSound);

    }

    public void ApplyBulletSpeedUpgrade()
    {
        bulletSpeedUpgrade.ApplyUpgrade();
        _audioSource.PlayOneShot(upgradeSound);

    }

    public void ApplyExtraBulletUpgrade()
    {
        extraBulletUpgrade.ApplyUpgrade();
        _audioSource.PlayOneShot(upgradeSound);

    }

    public void ApplyShipSpeedUpgrade()
    {
        shipSpeedUpgrade.ApplyUpgrade();
        _audioSource.PlayOneShot(upgradeSound);

    }

    public void ApplyPickUpUpgrade()
    {
        pickUpUpgrade.ApplyUpgrade();
        _audioSource.PlayOneShot(upgradeSound);

    }






}

