using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillTreeUIManager : MonoBehaviour
{
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

    [Header("Skill Tree/Speed Pathway")]
    [SerializeField] TextMeshProUGUI rapidLevelText, nimbleLevelText, acceleratedLevelText, turboLevelText;
    [SerializeField] Image rapidIcon, nimbleIcon, acceleratedIcon, turboIcon;
    [SerializeField] Image rapidButton, nimbleButton, acceleratedButton, turboButton;

    [Header("Skill Tree/Teleport Pathway")]
    [SerializeField] TextMeshProUGUI unlockTeleportLevelText, eradicationLevelText, enhancedLevelText, quickenedLevelText, extendedRangeLevelText;
    [SerializeField] Image unlockTeleportIcon, eradicationIcon, enhancedIcon, quickenedIcon, extendedRangeIcon;
    [SerializeField] Image unlockTeleportButton, eradicationButton, enhancedButton, quickenedButton, extendedRangeButton;

    void Update()
    {
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
            UpdateSkillLevelText(rapidLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Rapid")) ? skillTree.skills.Find(skill => skill.skillName == "Rapid") : null);
            UpdateSkillLevelText(nimbleLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Nimble")) ? skillTree.skills.Find(skill => skill.skillName == "Nimble") : null);
            UpdateSkillLevelText(acceleratedLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Accelerated")) ? skillTree.skills.Find(skill => skill.skillName == "Accelerated") : null);
            UpdateSkillLevelText(turboLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Turbo")) ? skillTree.skills.Find(skill => skill.skillName == "Turbo") : null);

            // Teleport Pathway
            UpdateSkillLevelText(unlockTeleportLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Unlock Teleport")) ? skillTree.skills.Find(skill => skill.skillName == "Unlock Teleport") : null);
            UpdateSkillLevelText(eradicationLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Eradication")) ? skillTree.skills.Find(skill => skill.skillName == "Eradication") : null);
            UpdateSkillLevelText(enhancedLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Enhanced")) ? skillTree.skills.Find(skill => skill.skillName == "Enhanced") : null);
            UpdateSkillLevelText(quickenedLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Quickened")) ? skillTree.skills.Find(skill => skill.skillName == "Quickened") : null);
            UpdateSkillLevelText(extendedRangeLevelText, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Extended Range")) ? skillTree.skills.Find(skill => skill.skillName == "Extended Range") : null);



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
            UpdateSkillNodeOpacity(rapidButton, rapidIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Rapid")) ? skillTree.skills.Find(skill => skill.skillName == "Rapid") : null);
            UpdateSkillNodeOpacity(nimbleButton, nimbleIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Nimble")) ? skillTree.skills.Find(skill => skill.skillName == "Nimble") : null);
            UpdateSkillNodeOpacity(acceleratedButton, acceleratedIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Accelerated")) ? skillTree.skills.Find(skill => skill.skillName == "Accelerated") : null);
            UpdateSkillNodeOpacity(turboButton, turboIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Turbo")) ? skillTree.skills.Find(skill => skill.skillName == "Turbo") : null);
            UpdateSkillNodeOpacity(unlockTeleportButton, unlockTeleportIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Unlock Teleport")) ? skillTree.skills.Find(skill => skill.skillName == "Unlock Teleport") : null);
            UpdateSkillNodeOpacity(eradicationButton, eradicationIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Eradication")) ? skillTree.skills.Find(skill => skill.skillName == "Eradication") : null);
            UpdateSkillNodeOpacity(enhancedButton, enhancedIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Enhanced")) ? skillTree.skills.Find(skill => skill.skillName == "Enhanced") : null);
            UpdateSkillNodeOpacity(quickenedButton, quickenedIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Quickened")) ? skillTree.skills.Find(skill => skill.skillName == "Quickened") : null);
            UpdateSkillNodeOpacity(extendedRangeButton, extendedRangeIcon, skillTree.skills.Contains(skillTree.skills.Find(skill => skill.skillName == "Extended Range")) ? skillTree.skills.Find(skill => skill.skillName == "Extended Range") : null);


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

    public void ExitSkillTree()
    {
        skillTreePanel.SetActive(false);
    }

    public void OpenSkillTree()
    {
        skillTreePanel.SetActive(true);
    }
}
