using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTree", menuName = "SkillTree")]
public class SkillTree : ScriptableObject
{
    public string skillTreeName;
    public List<Skill> skills;
    public List<Skill> unlockedSkills;
    public List<Skill> lockedSkills;

}
