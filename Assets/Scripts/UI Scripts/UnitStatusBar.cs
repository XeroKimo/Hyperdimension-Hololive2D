using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Version 0.2

public class UnitStatusBar : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_unitName;
    [SerializeField]
    Slider m_hpBar;
    [SerializeField]
    Slider m_mpBar;

    BattleUnit m_unit;

    public void TrackUnit(BattleUnit unit)
    {
        m_unit = unit;

        UnitStats totalStats = unit.unitData.totalStats;
        m_unitName.text = unit.unitData.unitDisplayPrefab.name;
        m_hpBar.value = (float)totalStats.currentHP / (float)totalStats.maxHP;
        m_mpBar.value = (float)totalStats.currentMP / (float)totalStats.maxMP;

        m_unit.OnUnitDies += OnUnitDies;
        m_unit.OnDamageTaken += OnDamageTaken;
        m_unit.OnManaConsumed += OnManaConsumed;
    }

    private void OnDamageTaken(BattleUnit unit, int damageTaken)
    {
        UnitStats totalStats = unit.unitData.totalStats;
        m_hpBar.value = (float)totalStats.currentHP / (float)totalStats.maxHP;
    }

    private void OnManaConsumed(BattleUnit unit, int manaConsumed)
    {
        UnitStats totalStats = unit.unitData.totalStats;
        m_mpBar.value = (float)totalStats.currentMP / (float)totalStats.maxMP;
    }

    private void OnUnitDies(BattleUnit obj)
    {

    }
}
