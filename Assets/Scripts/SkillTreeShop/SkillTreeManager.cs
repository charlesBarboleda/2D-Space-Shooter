using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;
    public SkillTree skillTree;
    private List<System.Action> skillEffects;

    void Awake()
    {
        ResetSkillTree();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private void ResetSkillTree()
    {
        foreach (Skill skill in skillTree.skills)
        {
            skill.skillLevel = 0;
            skill.isUnlocked = false;
            skill.cost = skill.startingCost;

            if (skill.skillName == "New Beginnings")
            {
                skill.isUnlocked = true;
            }
        }
    }
    public void ApplySkillEffect(string skillName, int costIncrement, List<System.Action> effects)
    {
        foreach (Skill skill in skillTree.skills)
        {
            if (skill.skillName == skillName && GameManager.Instance.GetPlayer().currency >= skill.cost && skill.skillLevel < skill.maxSkillLevel && ArePreqsMet(skill))
            {
                GameManager.Instance.GetPlayer().currency -= skill.cost;
                skill.skillLevel++;
                skill.cost += costIncrement;

                foreach (var effect in effects)
                {
                    effect();
                }
            }
        }
    }
    public void ApplyLaserEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Unlock the Laser ability
                AbilityHolderManager.Instance.UnlockSkill(GameManager.Instance.GetPlayer().abilityHolder.abilities.Find(ability => ability is AbilityLaser));
                UIManager.Instance.laserPanel.SetActive(true);
            },
        };

        ApplySkillEffect("Laser", 0, skillEffects);
    }
    public void ApplyViciousEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 10%
                float increaseDamage = 1.10f;
                GameManager.Instance.GetPlayer().weapon.bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            },
        };

        ApplySkillEffect("Vicious", 500, skillEffects);
    }

    public void ApplySprayAndPrayEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 10%
                int bulletIncrease = 2;
                GameManager.Instance.GetPlayer().weapon.amountOfBullets += bulletIncrease;
                Debug.Log("Bullet Amount Increased by " + bulletIncrease + " bullets");
            },
        };

        ApplySkillEffect("Spray And Pray", 500, skillEffects);
    }
    public void ApplyFerocityEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 10%
                float increaseDamage = 1.10f;
                GameManager.Instance.GetPlayer().weapon.bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            },
        };

        ApplySkillEffect("Ferocity", 500, skillEffects);
    }

    public void ApplyViolenceEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 10%
                float increaseDamage = 1.10f;
                GameManager.Instance.GetPlayer().weapon.bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            },
        };

        ApplySkillEffect("Violence", 500, skillEffects);
    }

    public void ApplyBrutalityEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 5%
                float increaseDamage = 1.05f;
                GameManager.Instance.GetPlayer().weapon.bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            }
        };

        ApplySkillEffect("Brutality", 250, skillEffects);
    }

    public void ApplyNewBeginningsEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 3%
                float increaseDamage = 1.03f;
                GameManager.Instance.GetPlayer().weapon.bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            },
            () =>
            {
                // Decrease the player's fire rate by 3%
                float decreaseFireRate = 0.97f;
                GameManager.Instance.GetPlayer().weapon.fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            },
            () =>
            {
                // Increase the player's max HP by 25
                float increaseHealth = 25f;
                GameManager.Instance.GetPlayer().playerHealth += increaseHealth;
                Debug.Log("Health Increased by " + increaseHealth);
            },
            () =>
            {
                // Increase the player's ship speed by 3%
                float increaseSpeed = 1.03f;
                GameManager.Instance.GetPlayer().playerController.moveSpeed *= increaseSpeed;
                Debug.Log("Speed Increased by " + (increaseSpeed - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("New Beginnings", 100, skillEffects);
    }

    private bool ArePreqsMet(Skill skill)
    {
        foreach (Skill preq in skill.prequisites)
        {
            if (preq.skillLevel < preq.maxSkillLevel)
            {
                return false;
            }
        }
        return true;
    }
}
