using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public int cost;
    public List<Skill> prequisites;
    public List<Skill> children;
    public int skillLevel;
    private int maxSkillLevel = 3;


}
