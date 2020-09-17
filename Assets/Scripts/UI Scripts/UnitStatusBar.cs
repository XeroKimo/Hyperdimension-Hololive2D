using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitStatusBar : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_unitName;
    [SerializeField]
    Slider m_hpBar;
    [SerializeField]
    Slider m_mpBar;

    BaseUnit m_unit;

    public void TrackUnit(BaseUnit unit)
    {
        m_unit = unit;

        UnitStats totalStats = unit.GetUnitData().totalStats;
        m_unitName.text = unit.GetUnitData().unitDisplayPrefab.name;
        m_hpBar.value = totalStats.currentHP / totalStats.maxHP;
        m_mpBar.value = totalStats.currentMP / totalStats.maxMP;

        m_unit.OnUnitDies += OnUnitDies;
        m_unit.OnDamageTaken += OnDamageTaken;
        m_unit.OnManaConsumed += OnManaConsumed;
    }

    private void OnDamageTaken(BaseUnit unit, int currentHP, int maxHP, int damage)
    {
        Debug.Log(currentHP + " " + maxHP);
        m_hpBar.value = (float)(currentHP) / (float)(maxHP);
    }

    private void OnManaConsumed(BaseUnit unit, int currentMana, int maxMana, int manaConsumed)
    {
        m_mpBar.value = (float)(currentMana) / (float)(maxMana);
    }

    private void OnUnitDies(BaseUnit obj)
    {

    }
}
