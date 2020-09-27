using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public AttackDescription attackDescription;
    public HitboxDescription hitboxDescription;
}
