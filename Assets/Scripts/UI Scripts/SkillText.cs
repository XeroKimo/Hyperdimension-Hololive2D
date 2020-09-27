using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillText : MonoBehaviour
{
    public int skillIndex;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI manaCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSkill(SkillData skill, int skillIndex)
    {
        displayName.text = skill.skillName;
        manaCost.text = skill.attackDescription.manaCost.ToString();
        this.skillIndex = skillIndex;
    }
}
