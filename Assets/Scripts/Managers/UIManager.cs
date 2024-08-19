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
    [SerializeField] TextMeshProUGUI healthUpgrade, damageUpgrade, fireRateUpgrade, bulletSpeedUpgrade, extraBulletUpgrade, speedUpgrade, pickUpUpgrade;
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
        SetDescriptionText(healthUpgrade, UpgradeShopManager.healthUpgrade);

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
        SetCostText(healthCost, UpgradeShopManager.healthUpgrade);
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
