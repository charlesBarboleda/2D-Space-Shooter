using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{

    public static SkillTreeManager Instance;
    public SkillTree skillTree;
    private List<Action> skillEffects;

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
    public void ApplySkillEffect(string skillName, int costIncrement, List<Action> effects)
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
    public void ApplyTurboEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's speed by 10%
                float increaseSpeed = 1.10f;
                GameManager.Instance.GetPlayer().playerController.moveSpeed *= increaseSpeed;
                Debug.Log("Speed Increased by " + (increaseSpeed - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Turbo", 500, skillEffects);
    }
    public void ApplyAcceleratedEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's speed by 8%
                float increaseSpeed = 1.08f;
                GameManager.Instance.GetPlayer().playerController.moveSpeed *= increaseSpeed;
                Debug.Log("Speed Increased by " + (increaseSpeed - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Accelerated", 400, skillEffects);
    }
    public void ApplyNimbleEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's speed by 5%
                float increaseSpeed = 1.05f;
                GameManager.Instance.GetPlayer().playerController.moveSpeed *= increaseSpeed;
                Debug.Log("Speed Increased by " + (increaseSpeed - 1) * 100 + "%");
            }
        };
        ApplySkillEffect("Nimble", 200, skillEffects);
    }
    public void ApplyRapidEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's speed by 3%
                float increaseSpeed = 1.03f;
                GameManager.Instance.GetPlayer().playerController.moveSpeed *= increaseSpeed;
                Debug.Log("Speed Increased by " + (increaseSpeed - 1) * 100 + "%");
            }
        };
        ApplySkillEffect("Rapid", 100, skillEffects);

    }
    public void ApplyRejuvenationEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's regeneration by 1 hp/s
                float increaseHealthRegen = 1f;
                GameManager.Instance.GetPlayer().healthRegen += increaseHealthRegen;
                Debug.Log("Health Regen Increased by " + increaseHealthRegen + "HP/s");
            }
        };

        ApplySkillEffect("Rejuvenation", 500, skillEffects);
    }
    public void ApplyFortitudeEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's max HP by 12%
                float increaseHealth = 1.12f;
                GameManager.Instance.GetPlayer().playerHealth *= increaseHealth;
                GameManager.Instance.GetPlayer().maxHealth *= increaseHealth;

                Debug.Log("Health Increased by " + (increaseHealth - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Fortitude", 400, skillEffects);
    }
    public void ApplyResilienceEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's max HP by 10%
                float increaseHealth = 1.1f;
                GameManager.Instance.GetPlayer().playerHealth *= increaseHealth;
                GameManager.Instance.GetPlayer().maxHealth *= increaseHealth;

                Debug.Log("Health Increased by " + (increaseHealth - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Resilience", 300, skillEffects);
    }
    public void ApplyEnduranceEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's max HP by 7%
                float increaseHealth = 1.07f;
                GameManager.Instance.GetPlayer().playerHealth *= increaseHealth;
                GameManager.Instance.GetPlayer().maxHealth *= increaseHealth;
                Debug.Log("Health Increased by " + (increaseHealth - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Endurance", 200, skillEffects);
    }

    public void ApplyVitalityEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's max HP by 5%
                float increaseHealth = 1.05f;
                GameManager.Instance.GetPlayer().playerHealth *= increaseHealth;
                Debug.Log("Health Increased by " + (increaseHealth - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Vitality", 100, skillEffects);
    }
    public void ApplySurplusEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's turret count by 2
                abilityHolder.abilities.OfType<AbilityTurrets>().FirstOrDefault().AbilityLogic(GameManager.Instance.GetPlayer().gameObject, null);
                GameManager.Instance.GetPlayer().GetComponent<TurretManager>().SpawnTurrets();
                Debug.Log("Turret Count Increased by 2");
            }
        };

        ApplySkillEffect("Surplus", 1000, skillEffects);
    }
    public void ApplyAnnihilationEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                float increaseDamage = 1.1f;
            abilityHolder.abilities.OfType<AbilityTurrets>().FirstOrDefault().fireRate *= increaseDamage;
                Debug.Log("Turret Damage Increased by 10%");

            }
        };

        ApplySkillEffect("Annihilation", 100, skillEffects);
    }
    public void ApplyQuickSuccessionEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's turret's fire rate by 8%
                float decreaseFireRate = 0.92f;
            abilityHolder.abilities.OfType<AbilityTurrets>().FirstOrDefault().fireRate *= decreaseFireRate;
                Debug.Log("Turret Fire Rate Decreased by 8%");
            }
        };
        ApplySkillEffect("Quick Succession", 500, skillEffects);
    }
    public void ApplyUnlockTurretEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Unlock the Turret ability
                AbilityHolderManager.Instance.UnlockSkill(GameManager.Instance.GetPlayer().abilityHolder.abilities.Find(ability => ability is AbilityTurrets));
                // Spawn turrets for the player
                GameManager.Instance.GetPlayer().GetComponent<TurretManager>().SpawnTurrets();
                UIManager.Instance.turretPanel.SetActive(true);
                Debug.Log("Turret Ability Unlocked");
            },
        };

        ApplySkillEffect("Unlock Turret", 0, skillEffects);
    }
    public void ApplyBulletHellEffect()
    {
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
        skillEffects = new List<Action>
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
