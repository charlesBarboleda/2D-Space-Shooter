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

    [Header("Skill Tree/Fire Rate Pathway")]
    [SerializeField] TextMeshProUGUI rapidFireLevelText, speedShooterLevelText, triggerFingerLevelText, blitzShotLevelText, bulletHellLevelText;
    [SerializeField] Image rapidFireIcon, speedShooterIcon, triggerFingerIcon, blitzShotIcon, bulletHellIcon;
    [SerializeField] Image rapidFireButton, speedShooterButton, triggerFingerButton, blitzShotButton, bulletHellButton;

    [Header("Skill Tree/Turret Pathway")]
    [SerializeField] TextMeshProUGUI unlockTurretLevelText, quickSuccessionLevelText, annihilationLevelText, surplusLevelText;
    [SerializeField] Image unlockTurretIcon, quickSuccessionIcon, annihilationIcon, surplusIcon;
    [SerializeField] Image unlockTurretButton, quickSuccessionButton, annihilationButton, surplusButton;

    [Header("Skill Tree/Health Pathway")]
    [SerializeField] TextMeshProUGUI vitalityLevelText, enduranceLevelText, resilienceLevelText, fortitudeLevelText, rejuvenationLevelText;
    [SerializeField] Image vitalityIcon, enduranceIcon, resilienceIcon, fortitudeIcon, rejuvenationIcon;
    [SerializeField] Image vitalityButton, enduranceButton, resilienceButton, fortitudeButton, rejuvenationButton;


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

            // Fire Rate Pathway
            UpdateSkillLevelText(rapidFireLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Rapid Fire")) ? skillTree.skills.Find(skill => skill.skillName == "Rapid Fire") : null);
            UpdateSkillLevelText(speedShooterLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Speed Shooter")) ? skillTree.skills.Find(skill => skill.skillName == "Speed Shooter") : null);
            UpdateSkillLevelText(triggerFingerLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Trigger Finger")) ? skillTree.skills.Find(skill => skill.skillName == "Trigger Finger") : null);
            UpdateSkillLevelText(blitzShotLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Blitz Shot")) ? skillTree.skills.Find(skill => skill.skillName == "Blitz Shot") : null);
            UpdateSkillLevelText(bulletHellLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Bullet Hell")) ? skillTree.skills.Find(skill => skill.skillName == "Bullet Hell") : null);

            // Turret Pathway
            UpdateSkillLevelText(unlockTurretLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Unlock Turret")) ? skillTree.skills.Find(skill => skill.skillName == "Unlock Turret") : null);
            UpdateSkillLevelText(quickSuccessionLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Quick Succession")) ? skillTree.skills.Find(skill => skill.skillName == "Quick Succession") : null);
            UpdateSkillLevelText(annihilationLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Annihilation")) ? skillTree.skills.Find(skill => skill.skillName == "Annihilation") : null);
            UpdateSkillLevelText(surplusLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Surplus")) ? skillTree.skills.Find(skill => skill.skillName == "Surplus") : null);

            //Health Pathway
            UpdateSkillLevelText(vitalityLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Vitality")) ? skillTree.skills.Find(skill => skill.skillName == "Vitality") : null);
            UpdateSkillLevelText(enduranceLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Endurance")) ? skillTree.skills.Find(skill => skill.skillName == "Endurance") : null);
            UpdateSkillLevelText(resilienceLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Resilience")) ? skillTree.skills.Find(skill => skill.skillName == "Resilience") : null);
            UpdateSkillLevelText(fortitudeLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Fortitude")) ? skillTree.skills.Find(skill => skill.skillName == "Fortitude") : null);
            UpdateSkillLevelText(rejuvenationLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Rejuvenation")) ? skillTree.skills.Find(skill => skill.skillName == "Rejuvenation") : null);

            //Shield Pathway


            // Speed Pathway


            // Teleport Pathway


            UpdateSkillNodeOpacity(brutalityButton, brutalityIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Brutality")) ? skillTree.skills.Find(skill => skill.skillName == "Brutality") : null);
            UpdateSkillNodeOpacity(violenceButton, violenceIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Violence")) ? skillTree.skills.Find(skill => skill.skillName == "Violence") : null);
            UpdateSkillNodeOpacity(ferocityButton, ferocityIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Ferocity")) ? skillTree.skills.Find(skill => skill.skillName == "Ferocity") : null);
            UpdateSkillNodeOpacity(viciousButton, viciousIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Vicious")) ? skillTree.skills.Find(skill => skill.skillName == "Vicious") : null);
            UpdateSkillNodeOpacity(sprayAndPrayButton, sprayAndPrayIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Spray And Pray")) ? skillTree.skills.Find(skill => skill.skillName == "Spray And Pray") : null);
            UpdateSkillNodeOpacity(unlockLaserButton, unlockLaserIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Unlock Laser")) ? skillTree.skills.Find(skill => skill.skillName == "Unlock Laser") : null);
            UpdateSkillNodeOpacity(destructionButton, destructionIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Destruction")) ? skillTree.skills.Find(skill => skill.skillName == "Destruction") : null);
            UpdateSkillNodeOpacity(tenaciousButton, tenaciousIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Tenacious")) ? skillTree.skills.Find(skill => skill.skillName == "Tenacious") : null);
            UpdateSkillNodeOpacity(expeditiousButton, expeditiousIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Expeditious")) ? skillTree.skills.Find(skill => skill.skillName == "Expeditious") : null);
            UpdateSkillNodeOpacity(rapidFireButton, rapidFireIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Rapid Fire")) ? skillTree.skills.Find(skill => skill.skillName == "Rapid Fire") : null);
            UpdateSkillNodeOpacity(speedShooterButton, speedShooterIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Speed Shooter")) ? skillTree.skills.Find(skill => skill.skillName == "Speed Shooter") : null);
            UpdateSkillNodeOpacity(triggerFingerButton, triggerFingerIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Trigger Finger")) ? skillTree.skills.Find(skill => skill.skillName == "Trigger Finger") : null);
            UpdateSkillNodeOpacity(blitzShotButton, blitzShotIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Blitz Shot")) ? skillTree.skills.Find(skill => skill.skillName == "Blitz Shot") : null);
            UpdateSkillNodeOpacity(bulletHellButton, bulletHellIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Bullet Hell")) ? skillTree.skills.Find(skill => skill.skillName == "Bullet Hell") : null);
            UpdateSkillNodeOpacity(unlockTurretButton, unlockTurretIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Unlock Turret")) ? skillTree.skills.Find(skill => skill.skillName == "Unlock Turret") : null);
            UpdateSkillNodeOpacity(quickSuccessionButton, quickSuccessionIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Quick Succession")) ? skillTree.skills.Find(skill => skill.skillName == "Quick Succession") : null);
            UpdateSkillNodeOpacity(annihilationButton, annihilationIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Annihilation")) ? skillTree.skills.Find(skill => skill.skillName == "Annihilation") : null);
            UpdateSkillNodeOpacity(surplusButton, surplusIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Surplus")) ? skillTree.skills.Find(skill => skill.skillName == "Surplus") : null);
            UpdateSkillNodeOpacity(vitalityButton, vitalityIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Vitality")) ? skillTree.skills.Find(skill => skill.skillName == "Vitality") : null);
            UpdateSkillNodeOpacity(enduranceButton, enduranceIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Endurance")) ? skillTree.skills.Find(skill => skill.skillName == "Endurance") : null);
            UpdateSkillNodeOpacity(resilienceButton, resilienceIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Resilience")) ? skillTree.skills.Find(skill => skill.skillName == "Resilience") : null);
            UpdateSkillNodeOpacity(fortitudeButton, fortitudeIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Fortitude")) ? skillTree.skills.Find(skill => skill.skillName == "Fortitude") : null);
            UpdateSkillNodeOpacity(rejuvenationButton, rejuvenationIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Rejuvenation")) ? skillTree.skills.Find(skill => skill.skillName == "Rejuvenation") : null);


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
