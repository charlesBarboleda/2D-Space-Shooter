using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Upgrade Shop")]
    [SerializeField] GameObject upgradeShopPanel;
    [SerializeField] TextMeshProUGUI healthUpgradeText, damageUpgradeText, fireRateUpgradeText, bulletSpeedUpgradeText, extraBulletUpgradeText, speedUpgradeText, pickUpUpgradeText;
    [SerializeField] TextMeshProUGUI healthCost, damageCost, fireRateCost, bulletSpeedCost, extraBulletCost, speedCost, pickUpCost;

    [Header("Game UI Elements")]
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] Image currencyIcon;
    public float iconOffset = 10f; // Base offset between text and icon
    public float additionalPaddingPerCharacter = 1f; // Extra padding per character

    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] GameObject gameOverPanel;

    [Header("Player Abilities UI")]
    [SerializeField] Image laserIconFill, shieldIconFill, teleportIconFill, turretIconFill;
    public GameObject laserPanel, shieldPanel, teleportPanel, turretPanel;

    [Header("Skill Tree")]
    [SerializeField] GameObject skillTreePanel;

    AbilityHolder abilityHolder;

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

        iconOffset = -280f;
        additionalPaddingPerCharacter = 1f;

        abilityHolder = PlayerManager.GetPlayer().GetComponent<AbilityHolder>();

    }
    void OnEnable()

    {
        PlayerManager.OnCurrencyChange += UpdateCurrencyText;
        EventManager.OnNextRound += UpdateRoundText;
        EventManager.OnGameOver += GameOver;
        EventManager.OnGameOver += UpdateHighScoreUI;

    }

    void OnDisable()
    {
        PlayerManager.OnCurrencyChange -= UpdateCurrencyText;
        EventManager.OnNextRound -= UpdateRoundText;
        EventManager.OnGameOver -= GameOver;
        EventManager.OnGameOver -= UpdateHighScoreUI;
    }

    void UpdateHighScoreUI()
    {
        highscoreText.text = $"Highscore: Level {PlayerPrefs.GetFloat("HighScore") + 1}";
    }



    void Update()
    {
        UpdateRoundText();
        UpdateCurrencyText();
        if (upgradeShopPanel.activeSelf)
        {
            UpdateAllUpgradeText();
        }



        foreach (Ability ability in abilityHolder.abilities)
        {
            if (ability.isUnlocked)
            {
                if (ability is AbilityLaser)
                {
                    laserIconFill.fillAmount = ability.currentCooldown / ability.cooldown;
                }
                if (ability is AbilityShield)
                {
                    shieldIconFill.fillAmount = ability.currentCooldown / ability.cooldown;
                }
                if (ability is AbilityTeleport)
                {
                    teleportIconFill.fillAmount = ability.currentCooldown / ability.cooldown;
                }
                if (ability is AbilityTurrets)
                {
                    turretIconFill.fillAmount = ability.currentCooldown / ability.cooldown;
                }
            }
        }

        // Calculate the width of the currency text
        float textWidth = currencyText.preferredWidth;

        // Clamp the text width to a minimum value
        textWidth = Mathf.Max(textWidth, 335f);

        // Apply a small buffer
        float buffer = 5f; // Adjust this value as needed

        // Update the position of the currency icon
        RectTransform iconRectTransform = currencyIcon.GetComponent<RectTransform>();
        iconRectTransform.anchoredPosition = new Vector2(textWidth + iconOffset + buffer, iconRectTransform.anchoredPosition.y);
    }


    // Methods for the Upgrade
    void UpdateAllUpgradeText()
    {
        // Updates the description text and cost text for each upgrade

        SetDescriptionText(healthUpgradeText, UpgradeShopManager.healthUpgrade);
        SetDescriptionText(damageUpgradeText, UpgradeShopManager.bulletDamageUpgrade);
        SetDescriptionText(fireRateUpgradeText, UpgradeShopManager.fireRateUpgrade);
        SetDescriptionText(bulletSpeedUpgradeText, UpgradeShopManager.bulletSpeedUpgrade);
        SetDescriptionText(extraBulletUpgradeText, UpgradeShopManager.extraBulletUpgrade);
        SetDescriptionText(speedUpgradeText, UpgradeShopManager.shipSpeedUpgrade);
        SetDescriptionText(pickUpUpgradeText, UpgradeShopManager.pickUpUpgrade);



        SetCostText(healthCost, UpgradeShopManager.healthUpgrade);
        SetCostText(damageCost, UpgradeShopManager.bulletDamageUpgrade);
        SetCostText(fireRateCost, UpgradeShopManager.fireRateUpgrade);
        SetCostText(bulletSpeedCost, UpgradeShopManager.bulletSpeedUpgrade);
        SetCostText(extraBulletCost, UpgradeShopManager.extraBulletUpgrade);
        SetCostText(speedCost, UpgradeShopManager.shipSpeedUpgrade);
        SetCostText(pickUpCost, UpgradeShopManager.pickUpUpgrade);
    }

    private void UpdateCurrencyText()
    {
        currencyText.text = $"{PlayerManager.GetPlayer().currency}";
    }

    private void UpdateRoundText()
    {
        if (GameManager.Instance.IsRound()) roundText.text = $"{GameManager.Instance.Level()}";
        if (GameManager.Instance.IsCountdown()) roundText.text = $"{Math.Round(GameManager.Instance.GetRoundCountdown(), 0)}";
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

    public void ExitSkillTree()
    {
        skillTreePanel.SetActive(false);
    }

    public void OpenSkillTree()
    {
        skillTreePanel.SetActive(true);
    }
}
