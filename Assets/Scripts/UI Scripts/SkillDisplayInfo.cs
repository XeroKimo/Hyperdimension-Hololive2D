using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillDisplayInfo : MonoBehaviour
{
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI attackMod;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI manaCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSkill(SkillData skill)
    {
        displayName.text = skill.skillName;
        attackMod.text = skill.attackDescription.attackModifier.ToString();

        Debug.LogWarning("SkillDisplayInfo Set skill is in debug mode");
        rangeText.text = "-";
        manaCost.text = skill.attackDescription.manaCost.ToString();
    }
}
