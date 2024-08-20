using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] TextMeshProUGUI healthUpgradeText, damageUpgradeText, fireRateUpgradeText, bulletSpeedUpgradeText, extraBulletUpgradeText, speedUpgradeText, pickUpUpgradeText;
    [SerializeField] TextMeshProUGUI healthCost, damageCost, fireRateCost, bulletSpeedCost, extraBulletCost, speedCost, pickUpCost;


    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject objectiveTemplate;
    [SerializeField] GameObject upgradeShopExitButton;
    [SerializeField] Transform objectivesContainer;


    void Awake()
    {
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
        EventManager.OnNextRound += UpdateRoundText;
        EventManager.OnGameOver += GameOver;
        PlayerManager.OnCurrencyChange += UpdateCurrencyText;



    }

    void OnDestroy()
    {
        EventManager.OnNextRound -= UpdateRoundText;
        EventManager.OnGameOver -= GameOver;
        PlayerManager.OnCurrencyChange -= UpdateCurrencyText;
    }

    void Update()
    {
        UpdateRoundText();
        UpdateCurrencyText();
        UpdateAllUpgradeText();

    }

    void UpdateAllUpgradeText()
    {
        SetDescriptionText(healthUpgradeText, UpgradeShopManager.healthUpgrade);
        SetDescriptionText(damageUpgradeText, UpgradeShopManager.bulletDamageUpgrade);
        SetDescriptionText(fireRateUpgradeText, UpgradeShopManager.fireRateUpgrade);
        SetDescriptionText(bulletSpeedUpgradeText, UpgradeShopManager.bulletSpeedUpgrade);

        SetCostText(healthCost, UpgradeShopManager.healthUpgrade);
        SetCostText(damageCost, UpgradeShopManager.bulletDamageUpgrade);
        SetCostText(fireRateCost, UpgradeShopManager.fireRateUpgrade);
        SetCostText(bulletSpeedCost, UpgradeShopManager.bulletSpeedUpgrade);
    }

    private void UpdateCurrencyText()
    {
        currencyText.text = $"{GameManager.Instance.GetPlayer().currency}";
    }

    private void UpdateRoundText()
    {
        if (GameManager.Instance.isRound) roundText.text = $"{GameManager.Instance.level}";
        if (GameManager.Instance.isCountdown) roundText.text = $"{Math.Round(GameManager.Instance.roundCountdown, 0)}";


    }

    private void SetDescriptionText(TextMeshProUGUI text, Upgrade upgrade)
    {
        text.text = upgrade.upgradeDescription;
    }

    private void SetCostText(TextMeshProUGUI text, Upgrade upgrade)
    {
        text.text = $"{upgrade.upgradeCost}";
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

}
