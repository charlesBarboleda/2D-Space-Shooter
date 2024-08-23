using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillTreeSkills/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public int cost;
    public TextMeshProUGUI levelText;
    public Image icon;
    public Image button;
    public int startingCost;
    public List<Skill> prequisites;
    public List<Skill> children;
    public int skillLevel;
    public int maxSkillLevel;
    public bool isUnlocked;



}
