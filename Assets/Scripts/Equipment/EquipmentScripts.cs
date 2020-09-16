using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EquipmentStats
{
    public int maxHPModifier;
    public int maxMPModifier;
    public int attackModifier;
    public int defenseModifier;
    public int hitModifier;
    public int dodgeModifier;
    public int speedModifier;
}

public class EquipmentBase : ScriptableObject
{
    public EquipmentStats stats;
}

