using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillTreeSkills/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public int cost;
    public Sprite skillIcon;
    public int startingCost;
    public List<Skill> prequisites;
    public List<Skill> children;
    public int skillLevel;
    public int maxSkillLevel;
    public bool isUnlocked;



}
