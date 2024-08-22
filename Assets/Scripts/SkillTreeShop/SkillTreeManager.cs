using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{

    public static SkillTreeManager Instance;
    public SkillTree skillTree;
    private List<System.Action> skillEffects;

    [Header("Skill Settings")]
    AbilityHolder abilityHolder;


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
    void Start()
    {
        abilityHolder = GameManager.Instance.GetPlayer().GetComponent<AbilityHolder>();
    }

    void ResetSkillTree()
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

                if (skill.skillLevel == skill.maxSkillLevel)
                {
                    skill.children.ForEach(child => child.isUnlocked = true);
                }

                foreach (var effect in effects)
                {
                    effect();
                }
            }
        }
    }
    public void ApplySurplusEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's turret count by 2
                
            }
        };

        ApplySkillEffect("Surplus", 1000, skillEffects);
    }
    public void ApplyAnnihilationEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's turret damage by 10%
            
            }
        };

        ApplySkillEffect("Annihilation", 100, skillEffects);
    }
    public void ApplyQuickSuccessionEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Decrease the player's turret's fire rate by 8%
                
            }
        };
        ApplySkillEffect("Quick Succession", 500, skillEffects);
    }
    public void ApplyUnlockTurretEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Unlock the Turret ability
                AbilityHolderManager.Instance.UnlockSkill(GameManager.Instance.GetPlayer().abilityHolder.abilities.Find(ability => ability is AbilityTurrets));
                UIManager.Instance.turretPanel.SetActive(true);
                Debug.Log("Turret Ability Unlocked");
            },
        };

        ApplySkillEffect("Unlock Turret", 0, skillEffects);
    }
    public void ApplyBulletHellEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's bullet count by 3
                int bulletIncrease = 3;
                GameManager.Instance.GetPlayer().weapon.amountOfBullets += bulletIncrease;
                Debug.Log("Bullet Count Increased by " + bulletIncrease + " Bullets");
            }
        };

        ApplySkillEffect("Bullet Hell", 0, skillEffects);
    }
    public void ApplyBlitzShotEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Decrease the player's fire rate by 10%
                float decreaseFireRate = 0.90f;
                GameManager.Instance.GetPlayer().weapon.fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            }
        };

        ApplySkillEffect("Blitz Shot", 250, skillEffects);
    }
    public void ApplyTriggerFingerEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Decrease the player's fire rate by 5%
                float decreaseFireRate = 0.95f;
                GameManager.Instance.GetPlayer().weapon.fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            }
        };

        ApplySkillEffect("Trigger Finger", 250, skillEffects);
    }
    public void ApplySpeedShooterEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Decrease the player's fire rate by 5%
                float decreaseFireRate = 0.95f;
                GameManager.Instance.GetPlayer().weapon.fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            }
        };

        ApplySkillEffect("Speed Shooter", 250, skillEffects);
    }
    public void ApplyRapidFireEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Decrease the player's fire rate by 3%
                float decreaseFireRate = 0.97f;
                GameManager.Instance.GetPlayer().weapon.fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            }
        };

        ApplySkillEffect("Rapid Fire", 250, skillEffects);
    }
    public void ApplyTenaciousEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's laser duration by 5%
                float increaseDuration = 1.05f;
                AbilityLaser laserAbility = abilityHolder.abilities.OfType<AbilityLaser>().FirstOrDefault();
                laserAbility.duration *= increaseDuration;
                Debug.Log("Laser Duration Increased by " + (increaseDuration - 1) * 100 + "%");
            }
        };
        ApplySkillEffect("Tenacious", 250, skillEffects);
    }
    public void ApplyExpeditiousEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Decrease the player's laser cooldown by 10%
                float decreaseCooldown = 0.90f;
                AbilityLaser laserAbility = abilityHolder.abilities.OfType<AbilityLaser>().FirstOrDefault();
                laserAbility.cooldown *= decreaseCooldown;
                Debug.Log("Laser Cooldown Decreased by " + (1 - decreaseCooldown) * 100 + "%");
            }
        };
        ApplySkillEffect("Expeditious", 250, skillEffects);
    }

    public void ApplyDestructionEffect()
    {
        skillEffects = new List<System.Action>
        {
            () =>
            {
                // Increase the player's laser damage by 10%
                float increaseDamage = 1.10f;
                AbilityLaser laserAbility = abilityHolder.abilities.OfType<AbilityLaser>().FirstOrDefault();
                laserAbility.dps *= increaseDamage;
                Debug.Log("Laser Damage Increased by " + increaseDamage + "%");
            }
        };

        ApplySkillEffect("Destruction", 250, skillEffects);
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
                Debug.Log("Laser Ability Unlocked");
            },
        };

        ApplySkillEffect("Unlock Laser", 0, skillEffects);
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
                // Increase the player's bullet count by 3
                int bulletIncrease = 3;
                GameManager.Instance.GetPlayer().weapon.amountOfBullets += bulletIncrease;
                Debug.Log("Bullet Count Increased by " + bulletIncrease + " Bullets");
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
