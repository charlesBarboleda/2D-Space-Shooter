using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PermanentShopManager : MonoBehaviour
{
    public TextMeshProUGUI healthAmountText, speedAmountText, radiusAmountText, bulletDamageAmountText, fireRateAmountText, bulletSpeedAmountText, bulletLifetimeAmountText;
    public Image currencyIcon;
    public TextMeshProUGUI currencyText, confirmationText, confirmationCurrencyText;
    public GameObject confirmationPanel, upgradesShopMenu, upgradesButtons, playMenu;
    int _healthAmountToUpgrade, _speedAmountToUpgrade, _radiusAmountToUpgrade, _bulletDamageAmountToUpgrade, _fireRateAmountToUpgrade, _bulletSpeedAmountToUpgrade, _bulletLifetimeAmountToUpgrade;
    int _totalCost;
    void Start()
    {
        currencyText.text = $"{Mathf.Round(PlayerPrefs.GetFloat("Credits"))}";
    }

    void Update()
    {
        // Set the currency icon position based on the currency text width
        currencyIcon.rectTransform.anchoredPosition = new Vector2(currencyText.preferredWidth + 130, currencyIcon.rectTransform.anchoredPosition.y);
    }
    public void IncreaseAmountToUpgrade(string typeToUpgrade)
    {
        switch (typeToUpgrade)
        {
            case "Health":
                _healthAmountToUpgrade++;
                healthAmountText.text = $"{_healthAmountToUpgrade}";
                break;
            case "Speed":
                _speedAmountToUpgrade++;
                speedAmountText.text = $"{_speedAmountToUpgrade}";
                break;
            case "PickUpRadius":
                _radiusAmountToUpgrade++;
                radiusAmountText.text = $"{_radiusAmountToUpgrade}";
                break;
            case "BulletDamage":
                _bulletDamageAmountToUpgrade++;
                bulletDamageAmountText.text = $"{_bulletDamageAmountToUpgrade}";
                break;
            case "FireRate":
                _fireRateAmountToUpgrade++;
                fireRateAmountText.text = $"{_fireRateAmountToUpgrade}";
                break;
            case "BulletSpeed":
                _bulletSpeedAmountToUpgrade++;
                bulletSpeedAmountText.text = $"{_bulletSpeedAmountToUpgrade}";
                break;
            case "BulletLifetime":
                _bulletLifetimeAmountToUpgrade++;
                bulletLifetimeAmountText.text = $"{_bulletLifetimeAmountToUpgrade}";
                break;
        }
        _totalCost += 1000;
    }
    public void DecreaseAmountToUpgrade(string typeToUpgrade)
    {
        switch (typeToUpgrade)
        {
            case "Health":
                if (_healthAmountToUpgrade > 0)
                {
                    _totalCost -= 1000;
                    _healthAmountToUpgrade--;
                    healthAmountText.text = $"{_healthAmountToUpgrade}";
                }
                break;
            case "Speed":
                if (_speedAmountToUpgrade > 0)
                {
                    _totalCost -= 1000;
                    _speedAmountToUpgrade--;
                    speedAmountText.text = $"{_speedAmountToUpgrade}";
                }
                break;
            case "PickUpRadius":
                if (_radiusAmountToUpgrade > 0)
                {
                    _totalCost -= 1000;
                    _radiusAmountToUpgrade--;
                    radiusAmountText.text = $"{_radiusAmountToUpgrade}";
                }
                break;
            case "BulletDamage":
                if (_bulletDamageAmountToUpgrade > 0)
                {
                    _totalCost -= 1000;
                    _bulletDamageAmountToUpgrade--;
                    bulletDamageAmountText.text = $"{_bulletDamageAmountToUpgrade}";
                }
                break;
            case "FireRate":
                if (_fireRateAmountToUpgrade > 0)
                {
                    _totalCost -= 1000;
                    _fireRateAmountToUpgrade--;
                    fireRateAmountText.text = $"{_fireRateAmountToUpgrade}";
                }
                break;
            case "BulletSpeed":
                if (_bulletSpeedAmountToUpgrade > 0)
                {
                    _totalCost -= 1000;
                    _bulletSpeedAmountToUpgrade--;
                    bulletSpeedAmountText.text = $"{_bulletSpeedAmountToUpgrade}";
                }
                break;
            case "BulletLifetime":
                if (_bulletLifetimeAmountToUpgrade > 0)
                {
                    _totalCost -= 1000;
                    _bulletLifetimeAmountToUpgrade--;
                    bulletLifetimeAmountText.text = $"{_bulletLifetimeAmountToUpgrade}";
                }
                break;
        }
    }

    public void PressConfirmButton()
    {
        confirmationPanel.SetActive(true);
        confirmationText.color = Color.white;
        confirmationText.text = "Purchase Upgrades?";
        confirmationCurrencyText.text = $"{_totalCost}";
    }


    public void PressYesInConfirm()
    {
        if (PlayerPrefs.GetFloat("Credits") >= _totalCost)
        {
            UpdateCurrentPermanentCurrency();
            ApplyAllUpgrades();
            confirmationPanel.SetActive(false);
            ResetAmountToUpgrade();
        }
        else
        {
            confirmationText.color = Color.red;
            confirmationText.text = "Not enough Credits!";
        }
    }
    public void PressNoInConfirm()
    {
        confirmationPanel.SetActive(false);
    }
    public void PressBackButton()
    {
        ResetAmountToUpgrade();
        confirmationPanel.SetActive(false);
        upgradesShopMenu.SetActive(false);
        upgradesButtons.SetActive(false);
        playMenu.SetActive(true);
    }
    void ResetAmountToUpgrade()
    {
        _totalCost = 0;

        _healthAmountToUpgrade = 0;
        healthAmountText.text = $"{_healthAmountToUpgrade}";

        _speedAmountToUpgrade = 0;
        speedAmountText.text = $"{_speedAmountToUpgrade}";

        _radiusAmountToUpgrade = 0;
        radiusAmountText.text = $"{_radiusAmountToUpgrade}";

        _bulletDamageAmountToUpgrade = 0;
        bulletDamageAmountText.text = $"{_bulletDamageAmountToUpgrade}";

        _fireRateAmountToUpgrade = 0;
        fireRateAmountText.text = $"{_fireRateAmountToUpgrade}";

        _bulletSpeedAmountToUpgrade = 0;
        bulletSpeedAmountText.text = $"{_bulletSpeedAmountToUpgrade}";

        _bulletLifetimeAmountToUpgrade = 0;
        bulletLifetimeAmountText.text = $"{_bulletLifetimeAmountToUpgrade}";
    }

    void UpdateCurrentPermanentCurrency()
    {
        float currentAmount = PlayerPrefs.GetFloat("Credits");
        float finalAmount = currentAmount - _totalCost;
        PlayerPrefs.SetFloat("Credits", finalAmount);
        PlayerPrefs.Save();
        currencyText.text = $"{Mathf.Round(PlayerPrefs.GetFloat("Credits"))}";


    }

    void ApplyAllUpgrades()
    {
        // Health Upgrade (Linear)
        float i = PlayerPrefs.GetFloat("Health", 100);
        float finalAmount = i + _healthAmountToUpgrade;
        PlayerPrefs.SetFloat("Health", finalAmount);

        // Speed with diminishing returns
        float baseSpeed = PlayerPrefs.GetFloat("SpeedBase", 15f); // Store the base speed
        int speedUpgrades = PlayerPrefs.GetInt("SpeedUpgrades", 0) + _speedAmountToUpgrade;
        float speedReductionFactor = 0.05f; // Define a reduction factor for speed
        float finalSpeed = baseSpeed * (1 + (1 - Mathf.Pow(1f - speedReductionFactor, speedUpgrades))); // Apply reduction factor for speed
        PlayerPrefs.SetFloat("Speed", finalSpeed); // Save the new speed
        PlayerPrefs.SetInt("SpeedUpgrades", speedUpgrades); // Save the number of speed upgrades

        // Pick Up Radius with diminishing returns
        float basePickUpRadius = PlayerPrefs.GetFloat("PickUpRadiusBase", 5f); // Store the base pickup radius
        int pickUpRadiusUpgrades = PlayerPrefs.GetInt("PickUpRadiusUpgrades", 0) + _radiusAmountToUpgrade;
        float radiusReductionFactor = 0.05f; // Define a reduction factor for pickup radius
        float finalPickUpRadius = basePickUpRadius * (1 + (1 - Mathf.Pow(1f - radiusReductionFactor, pickUpRadiusUpgrades))); // Apply reduction factor for pickup radius
        PlayerPrefs.SetFloat("PickUpRadius", finalPickUpRadius); // Save the new pickup radius
        PlayerPrefs.SetInt("PickUpRadiusUpgrades", pickUpRadiusUpgrades); // Save the number of pickup radius upgrades

        // Bullet Damage (Linear)
        float i4 = PlayerPrefs.GetFloat("BulletDamage", 50);
        float finalAmount4 = i4 + _bulletDamageAmountToUpgrade;
        PlayerPrefs.SetFloat("BulletDamage", finalAmount4);

        // Fire rate with diminishing returns (already implemented)
        float baseFireRate = PlayerPrefs.GetFloat("FireRateBase", 1f); // Store your base fire rate
        int fireRateUpgrades = PlayerPrefs.GetInt("FireRateUpgrades", 0) + _fireRateAmountToUpgrade;
        float fireRateReductionFactor = 0.025f; // Define a reduction factor for fire rate
        float finalFireRate = baseFireRate / (1 + fireRateUpgrades * fireRateReductionFactor); // Fire rate calculation
        PlayerPrefs.SetFloat("FireRate", finalFireRate); // Save the new fire rate
        PlayerPrefs.SetInt("FireRateUpgrades", fireRateUpgrades); // Save the number of fire rate upgrades

        // Bullet Speed (Linear)
        float i6 = PlayerPrefs.GetFloat("BulletSpeed", 30);
        float finalAmount6 = i6 - (_bulletSpeedAmountToUpgrade * 0.1f);
        PlayerPrefs.SetFloat("BulletSpeed", finalAmount6);

        // Bullet Lifetime (Linear)
        float i7 = PlayerPrefs.GetFloat("BulletLifetime", 5);
        float finalAmount7 = i7 - (_bulletLifetimeAmountToUpgrade * 0.1f);
        PlayerPrefs.SetFloat("BulletLifetime", finalAmount7);

        PlayerPrefs.Save();
    }



}
