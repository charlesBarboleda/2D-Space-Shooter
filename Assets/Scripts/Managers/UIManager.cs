using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Skill Tree")]
    [SerializeField] SkillTree skillTree;
    [SerializeField] GameObject skillTreePanel;
    [SerializeField] TextMeshProUGUI newBeginningsLevelText, brutalityLevelText, violenceLevelText, ferocityLevelText, viciousLevelText, sprayAndPrayLevelText;

    [Header("Upgrade Shop")]
    [SerializeField] GameObject upgradeShopPanel;
    [SerializeField] TextMeshProUGUI healthUpgradeText, damageUpgradeText, fireRateUpgradeText, bulletSpeedUpgradeText, extraBulletUpgradeText, speedUpgradeText, pickUpUpgradeText;
    [SerializeField] TextMeshProUGUI healthCost, damageCost, fireRateCost, bulletSpeedCost, extraBulletCost, speedCost, pickUpCost;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] GameObject gameOverPanel;


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
        if (upgradeShopPanel.activeSelf)
        {
            UpdateAllUpgradeText();
        }
        if (skillTreePanel.activeSelf)
        {
            UpdateSkillLevelText(newBeginningsLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "New Beginnings")) ? skillTree.skills.Find(skill => skill.skillName == "New Beginnings") : null);
            UpdateSkillLevelText(brutalityLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Brutality")) ? skillTree.skills.Find(skill => skill.skillName == "Brutality") : null);
            UpdateSkillLevelText(violenceLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Violence")) ? skillTree.skills.Find(skill => skill.skillName == "Violence") : null);
            UpdateSkillLevelText(ferocityLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Ferocity")) ? skillTree.skills.Find(skill => skill.skillName == "Ferocity") : null);
            UpdateSkillLevelText(viciousLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Vicious")) ? skillTree.skills.Find(skill => skill.skillName == "Vicious") : null);
            UpdateSkillLevelText(sprayAndPrayLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Spray And Pray")) ? skillTree.skills.Find(skill => skill.skillName == "Spray And Pray") : null);



        }

    }

    void UpdateSkillLevelText(TextMeshProUGUI textCount, Skill skill)
    {
        textCount.text = $"{skill.skillLevel}/{skill.maxSkillLevel}";
        if (skill.skillLevel == skill.maxSkillLevel)
        {
            textCount.text = "MAX";
        }
    }

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

    public void OpenSkillTree()
    {
        skillTreePanel.SetActive(true);
    }

    public void CloseSkillTree()
    {
        skillTreePanel.SetActive(false);
    }

}
