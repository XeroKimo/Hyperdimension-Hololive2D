using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version 0.2

public class FieldData : MonoBehaviour
{
    public static FieldData instance;

    public StaticUnitData[] enemyUnitData;

    private void Awake()
    {
        instance = this;
    }

    public void IntializeBattle(UnitData[] playerUnits)
    {
        Debug.LogWarning("Field data is operating in Debug mode");
        int randomUnitCount = UnityEngine.Random.Range(1, 2);

        UnitData[] units = new UnitData[randomUnitCount];

        for(int i = 0; i < randomUnitCount; i++)
        {
            units[i] = enemyUnitData[Random.Range(0, enemyUnitData.Length)].unitData;
        }

        BattleState.instance.InitializeBattle(playerUnits, units);

    }
}
