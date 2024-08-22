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

    [Header("Skill Tree/Bullet Damage Pathway")]
    [SerializeField] TextMeshProUGUI newBeginningsLevelText, brutalityLevelText, violenceLevelText, ferocityLevelText, viciousLevelText, sprayAndPrayLevelText;
    [SerializeField] Image brutalityIcon, violenceIcon, ferocityIcon, viciousIcon, sprayAndPrayIcon;
    [SerializeField] Image brutalityButton, violenceButton, ferocityButton, viciousButton, sprayAndPrayButton;


    [Header("Skill Tree/Laser Pathway")]
    [SerializeField] TextMeshProUGUI unlockLaserLevelText, destructionLevelText, tenaciousLevelText, expeditiousLevelText;
    [SerializeField] Image unlockLaserIcon, destructionIcon, tenaciousIcon, expeditiousIcon;
    [SerializeField] Image unlockLaserButton, destructionButton, tenaciousButton, expeditiousButton;


    [Header("Upgrade Shop")]
    [SerializeField] GameObject upgradeShopPanel;
    [SerializeField] TextMeshProUGUI healthUpgradeText, damageUpgradeText, fireRateUpgradeText, bulletSpeedUpgradeText, extraBulletUpgradeText, speedUpgradeText, pickUpUpgradeText;
    [SerializeField] TextMeshProUGUI healthCost, damageCost, fireRateCost, bulletSpeedCost, extraBulletCost, speedCost, pickUpCost;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] GameObject gameOverPanel;

    [Header("Player Abilities UI")]
    [SerializeField] Image laserIconFill, shieldIconFill, teleportIconFill, turretIconFill;
    public GameObject laserPanel, shieldPanel, teleportPanel, turretPanel;


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
        abilityHolder = GameManager.Instance.GetPlayer().GetComponent<AbilityHolder>();
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
            // Bullet Damage Pathway
            UpdateSkillLevelText(newBeginningsLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "New Beginnings")) ? skillTree.skills.Find(skill => skill.skillName == "New Beginnings") : null);
            UpdateSkillLevelText(brutalityLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Brutality")) ? skillTree.skills.Find(skill => skill.skillName == "Brutality") : null);
            UpdateSkillLevelText(violenceLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Violence")) ? skillTree.skills.Find(skill => skill.skillName == "Violence") : null);
            UpdateSkillLevelText(ferocityLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Ferocity")) ? skillTree.skills.Find(skill => skill.skillName == "Ferocity") : null);
            UpdateSkillLevelText(viciousLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Vicious")) ? skillTree.skills.Find(skill => skill.skillName == "Vicious") : null);
            UpdateSkillLevelText(sprayAndPrayLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Spray And Pray")) ? skillTree.skills.Find(skill => skill.skillName == "Spray And Pray") : null);

            // Laser Pathway
            UpdateSkillLevelText(unlockLaserLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Unlock Laser")) ? skillTree.skills.Find(skill => skill.skillName == "Unlock Laser") : null);
            UpdateSkillLevelText(destructionLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Destruction")) ? skillTree.skills.Find(skill => skill.skillName == "Destruction") : null);
            UpdateSkillLevelText(tenaciousLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Tenacious")) ? skillTree.skills.Find(skill => skill.skillName == "Tenacious") : null);
            UpdateSkillLevelText(expeditiousLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Expeditious")) ? skillTree.skills.Find(skill => skill.skillName == "Expeditious") : null);



            UpdateSkillNodeOpacity(brutalityButton, brutalityIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Brutality")) ? skillTree.skills.Find(skill => skill.skillName == "Brutality") : null);
            UpdateSkillNodeOpacity(violenceButton, violenceIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Violence")) ? skillTree.skills.Find(skill => skill.skillName == "Violence") : null);
            UpdateSkillNodeOpacity(ferocityButton, ferocityIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Ferocity")) ? skillTree.skills.Find(skill => skill.skillName == "Ferocity") : null);
            UpdateSkillNodeOpacity(viciousButton, viciousIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Vicious")) ? skillTree.skills.Find(skill => skill.skillName == "Vicious") : null);
            UpdateSkillNodeOpacity(sprayAndPrayButton, sprayAndPrayIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Spray And Pray")) ? skillTree.skills.Find(skill => skill.skillName == "Spray And Pray") : null);
            UpdateSkillNodeOpacity(unlockLaserButton, unlockLaserIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Unlock Laser")) ? skillTree.skills.Find(skill => skill.skillName == "Unlock Laser") : null);
            UpdateSkillNodeOpacity(destructionButton, destructionIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Destruction")) ? skillTree.skills.Find(skill => skill.skillName == "Destruction") : null);
            UpdateSkillNodeOpacity(tenaciousButton, tenaciousIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Tenacious")) ? skillTree.skills.Find(skill => skill.skillName == "Tenacious") : null);
            UpdateSkillNodeOpacity(expeditiousButton, expeditiousIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Expeditious")) ? skillTree.skills.Find(skill => skill.skillName == "Expeditious") : null);


        }

        foreach (Ability ability in abilityHolder.abilities)
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

    void UpdateSkillNodeOpacity(Image button, Image icon, Skill skill)
    {
        if (skill.isUnlocked)
        {
            button.color = new Color(1, 1, 1, 1);
            icon.color = new Color(1, 1, 1, 1);
        }
        else
        {
            button.color = new Color(1, 1, 1, 0.1f);
            icon.color = new Color(1, 1, 1, 0.1f);
        }
    }







    void UpdateSkillLevelText(TextMeshProUGUI textCount, Skill skill)
    {

        if (skill.skillLevel == skill.maxSkillLevel)
        {
            textCount.text = "MAX";
        }
        else if (!skill.isUnlocked)
        {
            textCount.text = "LOCKED";
        }
        else
        {
            textCount.text = $"LVL. {skill.skillLevel}";
        }
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
