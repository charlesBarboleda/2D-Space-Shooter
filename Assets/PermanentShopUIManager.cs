using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PermanentShopManager : MonoBehaviour
{
    public enum UpgradesType
    {
        Health,
        Speed,
        PickUpRadius,
        BulletDamage,
        BulletSpeed,
        BulletLifetime,
        FireRate,
    }
    public TextMeshProUGUI healthAmountText;
    public Image currencyIcon;
    public TextMeshProUGUI currencyText, confirmationText, confirmationCurrencyText;
    public GameObject confirmationPanel, upgradesShopMenu, playMenu;
    int _healthAmountToUpgrade;
    int _totalCost;
    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Credits", 10000);
        currencyText.text = $"{PlayerPrefs.GetFloat("Credits")}";
    }

    void Update()
    {
        // Set the currency icon position based on the currency text width
        currencyIcon.rectTransform.anchoredPosition = new Vector2(currencyText.preferredWidth + 130, currencyIcon.rectTransform.anchoredPosition.y);
        Debug.Log($"Total Credits: {PlayerPrefs.GetFloat("Credits")}");

    }
    public void IncreaseAmountToUpgrade(string typeToUpgrade)
    {
        switch (typeToUpgrade)
        {
            case "Health":
                _healthAmountToUpgrade++;
                _totalCost += 1000;
                healthAmountText.text = $"{_healthAmountToUpgrade}";
                break;
        }
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
                }
                healthAmountText.text = $"{_healthAmountToUpgrade}";
                break;
        }
    }

    public void PressConfirmButton()
    {
        confirmationPanel.SetActive(true);
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
        playMenu.SetActive(true);
    }
    void ResetAmountToUpgrade()
    {
        _totalCost = 0;

        _healthAmountToUpgrade = 0;
        healthAmountText.text = $"{_healthAmountToUpgrade}";
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







        PlayerPrefs.Save();
    }
}
