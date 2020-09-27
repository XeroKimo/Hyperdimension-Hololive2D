using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenu : MonoBehaviour
{
    public static SkillMenu instance;
    public SkillText skillTextPrefab;
    public BattleUnit currentUnit;
    public SkillDisplayInfo displayInfo;
    public List<SkillText> skillTexts = new List<SkillText>();
    public RectTransform skillTextContainer;

    private int m_currentSkillIndex = 0;
    [SerializeField]
    private int m_skillCount = 0;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void SetUnit(BattleUnit unit)
    {
        currentUnit = unit;
        List<SkillData> skills = unit.unitData.skills;

        m_skillCount = unit.unitData.skills.Count;

        if(skills.Count > skillTexts.Count)
            SpawnNewTexts(skills.Count - skillTexts.Count);
        else if(skillTexts.Count > skills.Count)
            DisableTexts(skillTexts.Count - skills.Count);

        SetSkillTexts(skills);
    }

    private void OnEnable()
    {
        if(m_skillCount == 0)
            return;

        SetSkillInfo(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            gameObject.SetActive(false);
            BattleState.instance.playerController.isActive = true;
        }
    }

    void SpawnNewTexts(int number)
    {
        for(int i = 0; i < number; i++)
        {
            SkillText text = Instantiate(skillTextPrefab, skillTextContainer);
            skillTexts.Add(text);
            text.gameObject.SetActive(true);
        }
    }

    void DisableTexts(int number)
    {
        for(int i = skillTexts.Count - 1; i >= skillTexts.Count - number; i--)
        {
            skillTexts[i].gameObject.SetActive(false);
        }
    }

    void SetSkillTexts(List<SkillData> skills)
    {
        for(int i = 0; i < skills.Count; i++)
        {
            skillTexts[i].SetSkill(skills[i], i);
            skillTexts[i].gameObject.SetActive(true);
        }
    }

    void SetSkillInfo(int index)
    {
        if(m_skillCount == 0)
            return;

        m_currentSkillIndex = index;

        displayInfo.SetSkill(currentUnit.unitData.skills[m_currentSkillIndex]);
    }

    void NextSkill()
    {
        if(m_skillCount == 0)
            return;

        m_currentSkillIndex = (m_currentSkillIndex + 1) % m_skillCount;

        displayInfo.SetSkill(currentUnit.unitData.skills[m_currentSkillIndex]);
    }

    void PreviousSkill()
    {
        if(m_skillCount == 0)
            return;

        m_currentSkillIndex--;
        if(m_currentSkillIndex < 0)
            m_currentSkillIndex += m_skillCount;

        displayInfo.SetSkill(currentUnit.unitData.skills[m_currentSkillIndex]);
    }
}
