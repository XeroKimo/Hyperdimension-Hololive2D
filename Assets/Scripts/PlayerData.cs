using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    static public PlayerData instance;

    public StaticUnitData[] debugUnitData;
    public UnitData[] activeUnits;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Player Data is in debug mode");
        activeUnits = new UnitData[4];
        for(int i = 0; i < 1; i++)
        {
            if(i == debugUnitData.Length)
                break;

            activeUnits[i] = debugUnitData[i].unitData;
        }
        DebugStartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DebugStartBattle()
    {
        FieldData.instance.IntializeBattle(activeUnits);
    }
}
