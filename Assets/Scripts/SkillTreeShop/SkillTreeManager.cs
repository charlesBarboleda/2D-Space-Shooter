using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;
    public SkillTree skillTree;

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
    public void ApplySkillEffect()
    {
        foreach (Skill skill in skillTree.skills)
        {
            if (GameManager.Instance.GetPlayer().currency >= skill.cost && skill.skillLevel <= skill.maxSkillLevel && ArePreqsMet(skill))
            {
                if (skill.skillName == "New Beginnings")
                {
                    {
                        GameManager.Instance.GetPlayer().currency -= skill.cost;
                        skill.skillLevel++;
                        skill.cost += 100;

                        // Increase the player's bullet damage by 3%
                        float increaseDamage = 1.03f;
                        GameManager.Instance.GetPlayer().weapon.bulletDamage *= increaseDamage;
                        Debug.Log("Bullet Damage Increased");



                        // Decrease the player's fire rate by 3%
                        float decreaseFireRate = 0.97f;
                        GameManager.Instance.GetPlayer().weapon.fireRate *= decreaseFireRate;
                        Debug.Log("Fire Rate Decreased");

                        // Increase the player's max HP by 25
                        float increaseHealth = 25f;
                        GameManager.Instance.GetPlayer().playerHealth += increaseHealth;
                        Debug.Log("Health Increased");


                        // Increase the player's ship speed by 3%
                        float increaseSpeed = 1.03f;
                        GameManager.Instance.GetPlayer().playerController.moveSpeed *= increaseSpeed;
                        Debug.Log("Speed Increased");


                    }
                }
            }
        }
    }

    private bool ArePreqsMet(Skill skill)
    {
        foreach (Skill preq in skill.prequisites)
        {
            if (!preq.isUnlocked)
            {
                return false;
            }
        }
        return true;
    }
}
