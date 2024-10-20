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
        currencyText.text = $"{PlayerPrefs.GetFloat("Credits")}";
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
        currencyText.text = $"{PlayerPrefs.GetFloat("Credits")}";


    }

    void ApplyAllUpgrades()
    {
        float i = PlayerPrefs.GetFloat("Health", 100);
        float finalAmount = i + _healthAmountToUpgrade;
        PlayerPrefs.SetFloat("Health", finalAmount);

        float i2 = PlayerPrefs.GetFloat("Speed", 15);
        float finalAmount2 = i2 + (_speedAmountToUpgrade * 0.1f);
        PlayerPrefs.SetFloat("Speed", finalAmount2);


        float i3 = PlayerPrefs.GetFloat("PickUpRadius", 5);
        float finalAmount3 = i3 + (_radiusAmountToUpgrade * 0.1f);
        PlayerPrefs.SetFloat("PickUpRadius", finalAmount3);

        float i4 = PlayerPrefs.GetFloat("BulletDamage", 50);
        float finalAmount4 = i4 + _bulletDamageAmountToUpgrade;
        PlayerPrefs.SetFloat("BulletDamage", finalAmount4);

        float i5 = PlayerPrefs.GetFloat("FireRate", 1);
        float finalAmount5 = i5 - (_fireRateAmountToUpgrade * 0.01f);
        PlayerPrefs.SetFloat("FireRate", finalAmount5);

        float i6 = PlayerPrefs.GetFloat("BulletSpeed", 5);
        float finalAmount6 = i6 - (_bulletSpeedAmountToUpgrade * 0.1f);
        PlayerPrefs.SetFloat("BulletSpeed", finalAmount6);

        float i7 = PlayerPrefs.GetFloat("BulletLifetime", 5);
        float finalAmount7 = i7 - (_bulletLifetimeAmountToUpgrade * 0.1f);
        PlayerPrefs.SetFloat("BulletLifetime", finalAmount7);


        PlayerPrefs.Save();
    }
}
