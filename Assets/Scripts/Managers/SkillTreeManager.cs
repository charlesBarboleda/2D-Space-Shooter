using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip _buttonPressedAudioClip;
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
        abilityHolder = PlayerManager.GetInstance().AbilityHolder();
        _audioSource = GetComponent<AudioSource>();
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
            if (skill.skillName == skillName && PlayerManager.GetInstance().Currency() >= skill.cost && skill.skillLevel < skill.maxSkillLevel && ArePreqsMet(skill))
            {
                PlayerManager.GetInstance().SetCurrency(PlayerManager.GetInstance().Currency() - skill.cost);
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
        _audioSource.PlayOneShot(_buttonPressedAudioClip);

    }
    public void ApplyReducedEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's shield cooldown by 5%
                float decreaseCooldown = 0.95f;
                abilityHolder.abilities.OfType<AbilityShield>().FirstOrDefault().cooldown *= decreaseCooldown;
                Debug.Log("Shield Cooldown Decreased by " + (1 - decreaseCooldown) * 100 + "%");
            }
        };

        ApplySkillEffect("Reduced", 250, skillEffects);
    }
    public void ApplyProlongedEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's shield duration by 0.2 seconds
                float increaseDuration = 0.2f;
                abilityHolder.abilities.OfType<AbilityShield>().FirstOrDefault()._duration += increaseDuration;
                Debug.Log("Shield Duration Increased by " + increaseDuration + " seconds");
            }
        };

        ApplySkillEffect("Prolonged", 250, skillEffects);
    }
    public void ApplyExpandedEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's shield size by 7%
                float increaseSize = 1.07f;
                abilityHolder.abilities.OfType<AbilityShield>().FirstOrDefault()._shieldSize *= increaseSize;
                Debug.Log("Shield Size Increased by 7%");
            }
        };

        ApplySkillEffect("Expanded", 125, skillEffects);
    }
    public void ApplyDevastationEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's shield damage by 5%
                float increaseDamage = 1.05f;
                abilityHolder.abilities.OfType<AbilityShield>().FirstOrDefault()._shieldDamage *= increaseDamage;
                Debug.Log("Shield Damage Increased by 10%");
            },
        };

        ApplySkillEffect("Devastation", 250, skillEffects);
    }
    public void ApplyUnlockShieldEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Unlock the Shield ability
                PlayerAbilityHolderManager.Instance.UnlockSkill(PlayerManager.GetInstance().AbilityHolder().abilities.Find(ability => ability is AbilityShield));
                UIManager.Instance.shieldPanel.SetActive(true);
                Debug.Log("Shield Ability Unlocked");
            },
        };

        ApplySkillEffect("Unlock Shield", 0, skillEffects);
    }
    public void ApplyExtendedRangeEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's teleport distance by 7%
                float increaseDistance = 1.07f;
                abilityHolder.abilities.OfType<AbilityTeleport>().FirstOrDefault()._teleportDistance *= increaseDistance;
                Debug.Log("Teleport Distance Increased by 7%");
            }
        };

        ApplySkillEffect("Extended Range", 250, skillEffects);
    }
    public void ApplyQuickenedEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's teleport cooldown by 5%
                float decreaseCooldown = 0.95f;
                abilityHolder.abilities.OfType<AbilityTeleport>().FirstOrDefault().cooldown *= decreaseCooldown;
                Debug.Log("Teleport Cooldown Decreased by " + (1 - decreaseCooldown) * 100 + "%");
            }
        };
        ApplySkillEffect("Quickened", 250, skillEffects);
    }
    public void ApplyEnhancedEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's teleport duration by 0.2 seconds
                float increaseDuration = 0.2f;
                abilityHolder.abilities.OfType<AbilityTeleport>().FirstOrDefault()._teleportDuration += increaseDuration;
                Debug.Log("Teleport Duration Increased by " + increaseDuration + " seconds");

                // Increase the player's teleport size by 5%
                float increaseSize = 1.05f;
                abilityHolder.abilities.OfType<AbilityTeleport>().FirstOrDefault()._teleportSize *= increaseSize;
                Debug.Log("Teleport Size Increased by 5%");

            }
        };

        ApplySkillEffect("Enhanced", 125, skillEffects);
    }
    public void ApplyEradicationEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's teleport damage by 10%
               abilityHolder.abilities.OfType<AbilityTeleport>().FirstOrDefault()._teleportDamage *= 1.10f;
                Debug.Log("Teleport Damage Increased by 10%");
            },
        };

        ApplySkillEffect("Eradication", 50, skillEffects);
    }
    public void ApplyUnlockTeleportEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Unlock the Teleport ability
                PlayerAbilityHolderManager.Instance.UnlockSkill(PlayerManager.GetInstance().AbilityHolder().abilities.Find(ability => ability is AbilityTeleport));
                UIManager.Instance.teleportPanel.SetActive(true);
                Debug.Log("Teleport Ability Unlocked");
            },
        };

        ApplySkillEffect("Unlock Teleport", 0, skillEffects);
    }

    public void ApplyTurboEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's speed by 5%
                float _increaseSpeed = 1.05f;
                                PlayerManager.GetInstance().SetMoveSpeed(PlayerManager.GetInstance().MoveSpeed() *  _increaseSpeed);

                Debug.Log("Speed Increased by " + (_increaseSpeed - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Turbo", 250, skillEffects);
    }
    public void ApplyAcceleratedEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's speed by 4%
                float _increaseSpeed = 1.04f;
                                PlayerManager.GetInstance().SetMoveSpeed(PlayerManager.GetInstance().MoveSpeed() *  _increaseSpeed);

                Debug.Log("Speed Increased by " + (_increaseSpeed - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Accelerated", 200, skillEffects);
    }
    public void ApplyNimbleEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's speed by 3%
                float _increaseSpeed = 1.03f;
                PlayerManager.GetInstance().SetMoveSpeed(PlayerManager.GetInstance().MoveSpeed() *  _increaseSpeed);

                Debug.Log("Speed Increased by " + (_increaseSpeed - 1) * 100 + "%");
            }
        };
        ApplySkillEffect("Nimble", 100, skillEffects);
    }
    public void ApplyRapidEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's speed by 1%
                float _increaseSpeed = 1.01f;
                PlayerManager.GetInstance().SetMoveSpeed(PlayerManager.GetInstance().MoveSpeed() *  _increaseSpeed);
                Debug.Log("Speed Increased by " + (_increaseSpeed - 1) * 100 + "%");
            }
        };
        ApplySkillEffect("Rapid", 50, skillEffects);

    }
    public void ApplyRejuvenationEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's regeneration by 5 hp/s
                float increaseHealthRegen = 5f;
                PlayerManager.GetInstance().SetHealthRegenRate(PlayerManager.GetInstance().HealthRegenRate() + increaseHealthRegen);
                Debug.Log("Health Regen Increased by " + increaseHealthRegen + "HP/s");
                increaseHealthRegen++;
            }
        };

        ApplySkillEffect("Rejuvenation", 250, skillEffects);
    }
    public void ApplyFortitudeEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's max HP by 12%
                float increaseHealth = 1.12f;
               PlayerManager.GetInstance().SetCurrentHealth(PlayerManager.GetInstance().CurrentHealth() * increaseHealth);
                PlayerManager.GetInstance().SetMaxHealth(PlayerManager.GetInstance().MaxHealth() * increaseHealth);

                Debug.Log("Health Increased by " + (increaseHealth - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Fortitude", 200, skillEffects);
    }
    public void ApplyResilienceEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's max HP by 10%
                float increaseHealth = 1.1f;
PlayerManager.GetInstance().SetCurrentHealth(PlayerManager.GetInstance().CurrentHealth() * increaseHealth);
                PlayerManager.GetInstance().SetMaxHealth(PlayerManager.GetInstance().MaxHealth() * increaseHealth);

                Debug.Log("Health Increased by " + (increaseHealth - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Resilience", 150, skillEffects);
    }
    public void ApplyEnduranceEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's max HP by 7%
                float increaseHealth = 1.07f;
PlayerManager.GetInstance().SetCurrentHealth(PlayerManager.GetInstance().CurrentHealth() * increaseHealth);
                PlayerManager.GetInstance().SetMaxHealth(PlayerManager.GetInstance().MaxHealth() * increaseHealth);
                Debug.Log("Health Increased by " + (increaseHealth - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Endurance", 100, skillEffects);
    }

    public void ApplyVitalityEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's max HP by 5%
                float increaseHealth = 1.05f;
                PlayerManager.GetInstance().SetCurrentHealth(PlayerManager.GetInstance().CurrentHealth() * increaseHealth);
                PlayerManager.GetInstance().SetMaxHealth(PlayerManager.GetInstance().MaxHealth() * increaseHealth);
                Debug.Log("Health Increased by " + (increaseHealth - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("Vitality", 50, skillEffects);
    }
    public void ApplySurplusEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's turret count by 2
                PlayerManager.GetInstance().GetComponent<TurretManager>().SpawnTurrets();
                Debug.Log("Turret Count Increased by 2");
            }
        };

        ApplySkillEffect("Surplus", 500, skillEffects);
    }
    public void ApplyUnlockTurretEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Unlock the Turret ability
                PlayerAbilityHolderManager.Instance.UnlockSkill(PlayerManager.GetInstance().AbilityHolder().abilities.Find(ability => ability is AbilityTurrets));
                UIManager.Instance.turretPanel.SetActive(true);
                Debug.Log("Turret Ability Unlocked");

                // Increase the player's turret count by 2
                PlayerManager.GetInstance().GetComponent<TurretManager>().SpawnTurrets();
            },
        };

        ApplySkillEffect("Unlock Turret", 0, skillEffects);
    }
    public void ApplyQuickSuccessionEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's turret's fire rate by 7%
                float decreaseFireRate = 0.93f;
                PlayerManager.GetInstance().GetComponent<TurretManager>().SetTurretFireRate(PlayerManager.GetInstance().GetComponent<TurretManager>().GetTurretFireRate() * decreaseFireRate);
                Debug.Log("Turret Fire Rate Decreased by 7%");
            }
        };
        ApplySkillEffect("Quick Succession", 250, skillEffects);
    }
    public void ApplyAnnihilationEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's turret damage by 10%  
                float increaseDamage = 1.1f;
                PlayerManager.GetInstance().GetComponent<TurretManager>().SetTurretDamage(PlayerManager.GetInstance().GetComponent<TurretManager>().GetTurretDamage() * increaseDamage);
                Debug.Log("Turret Damage Increased by 10%");

            }
        };

        ApplySkillEffect("Annihilation", 50, skillEffects);
    }
    public void ApplyBulletHellEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's bullet count by 3
                int bulletIncrease = 1;
                PlayerManager.GetInstance().Weapon().amountOfBullets += bulletIncrease;
                Debug.Log("Bullet Count Increased by " + bulletIncrease + " Bullet");
            }
        };

        ApplySkillEffect("Bullet Hell", 1000, skillEffects);
    }
    public void ApplyBlitzShotEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's fire rate by 5%
                float decreaseFireRate = 0.95f;
                PlayerManager.GetInstance().Weapon().fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            }
        };

        ApplySkillEffect("Blitz Shot", 125, skillEffects);
    }
    public void ApplyTriggerFingerEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's fire rate by 5%
                float decreaseFireRate = 0.975f;
                PlayerManager.GetInstance().Weapon().fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            }
        };

        ApplySkillEffect("Trigger Finger", 125, skillEffects);
    }
    public void ApplySpeedShooterEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's fire rate by 2.5%
                float decreaseFireRate = 0.975f;
                PlayerManager.GetInstance().Weapon().fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            }
        };

        ApplySkillEffect("Speed Shooter", 125, skillEffects);
    }
    public void ApplyRapidFireEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's fire rate by 3%
                float decreaseFireRate = 0.97f;
                PlayerManager.GetInstance().Weapon().fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            }
        };

        ApplySkillEffect("Rapid Fire", 125, skillEffects);
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
        ApplySkillEffect("Tenacious", 125, skillEffects);
    }
    public void ApplyExpeditiousEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Decrease the player's laser cooldown by 5%
                float decreaseCooldown = 0.95f;
                AbilityLaser laserAbility = abilityHolder.abilities.OfType<AbilityLaser>().FirstOrDefault();
                laserAbility.cooldown *= decreaseCooldown;
                Debug.Log("Laser Cooldown Decreased by " + (1 - decreaseCooldown) * 100 + "%");
            }
        };
        ApplySkillEffect("Expeditious", 125, skillEffects);
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

        ApplySkillEffect("Destruction", 125, skillEffects);
    }

    public void ApplyLaserEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Unlock the Laser ability
                PlayerAbilityHolderManager.Instance.UnlockSkill(PlayerManager.GetInstance().AbilityHolder().abilities.Find(ability => ability is AbilityLaser));
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
                float increaseDamage = 1.05f;
                PlayerManager.GetInstance().Weapon().bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            },
        };

        ApplySkillEffect("Vicious", 250, skillEffects);
    }

    public void ApplySprayAndPrayEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's bullet count by 1
                int bulletIncrease = 1;
                PlayerManager.GetInstance().Weapon().amountOfBullets += bulletIncrease;
                Debug.Log("Bullet Count Increased by " + bulletIncrease + " Bullets");
            },
        };

        ApplySkillEffect("Spray And Pray", 1000, skillEffects);
    }
    public void ApplyFerocityEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 10%
                float increaseDamage = 1.05f;
                PlayerManager.GetInstance().Weapon().bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            },
        };

        ApplySkillEffect("Ferocity", 250, skillEffects);
    }

    public void ApplyViolenceEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 10%
                float increaseDamage = 1.05f;
                PlayerManager.GetInstance().Weapon().bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            },
        };

        ApplySkillEffect("Violence", 250, skillEffects);
    }

    public void ApplyBrutalityEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 2.5%
                float increaseDamage = 1.025f;
                PlayerManager.GetInstance().Weapon().bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            }
        };

        ApplySkillEffect("Brutality", 125, skillEffects);
    }

    public void ApplyNewBeginningsEffect()
    {
        skillEffects = new List<Action>
        {
            () =>
            {
                // Increase the player's bullet damage by 3%
                float increaseDamage = 1.03f;
                PlayerManager.GetInstance().Weapon().bulletDamage *= increaseDamage;
                Debug.Log("Bullet Damage Increased by " + increaseDamage + "%");
            },
            () =>
            {
                // Decrease the player's fire rate by 3%
                float decreaseFireRate = 0.97f;
                PlayerManager.GetInstance().Weapon().fireRate *= decreaseFireRate;
                Debug.Log("Fire Rate Decreased by " + (1 - decreaseFireRate) * 100 + "%");
            },
            () =>
            {
                // Increase the player's max HP by 25
                float increaseHealth = 25f;
                PlayerManager.GetInstance().SetCurrentHealth(PlayerManager.GetInstance().CurrentHealth() + increaseHealth);
                PlayerManager.GetInstance().SetMaxHealth(PlayerManager.GetInstance().MaxHealth() + increaseHealth);

                Debug.Log("Health Increased by " + increaseHealth);
            },
            () =>
            {
                // Increase the player's ship speed by 3%
                float _increaseSpeed = 1.03f;
                PlayerManager.GetInstance().SetMoveSpeed(PlayerManager.GetInstance().MoveSpeed() * _increaseSpeed);
                Debug.Log("Speed Increased by " + (_increaseSpeed - 1) * 100 + "%");
            }
        };

        ApplySkillEffect("New Beginnings", 50, skillEffects);
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
