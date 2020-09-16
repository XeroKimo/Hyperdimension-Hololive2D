using System.Collections.Generic;
using UnityEngine;

public struct ActiveUnit
{
    public BaseUnit unit;
    public Vector2 initialPos;

    public ActiveUnit(BaseUnit unit)
    {
        this.unit = unit;
        initialPos = unit.transform.position;
    }

    public void SetActiveUnit(BaseUnit unit)
    {
        this.unit = unit;
        initialPos = unit.transform.position;
    }
}

public class BattleState : MonoBehaviour
{
    public static BattleState instance;

    public SpriteRenderer movementLimitObj;

    public PlayerController playerController;
    public AIController aiController;

    public BaseUnit unitPrefab;
    public UnitData[] debugPlayerData;
    public UnitData[] debugEnemyData;

    public Transform[] playerSpawnPoints;
    public Transform[] enemySpawnPoints;

    public ActiveUnit activeUnit;

    List<BaseUnit> m_units = new List<BaseUnit>();

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        movementLimitObj.transform.localScale = new Vector3(Constants.unitMaxUnitTravelDistance, Constants.unitMaxUnitTravelDistance, 1) * 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        //For debugging
        InitializeBattle(debugPlayerData, debugEnemyData);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartNextTurn()
    {
        activeUnit.unit.OnUnitEndTurn();

        SetNextUnit();

    }

    public void InitializeBattle(UnitData[] playerUnits, UnitData[] enemyUnits)
    {
        BaseUnit spawnedUnit;

        for(int i = 0; i < playerUnits.Length; i++)
        {
            spawnedUnit = Instantiate(unitPrefab, playerSpawnPoints[i].position, Quaternion.identity);
            m_units.Add(spawnedUnit);
            spawnedUnit.isPlayerUnit = true;

            spawnedUnit.Initialize(playerUnits[i]);
            spawnedUnit.OnUnitDies += OnUnitDies;
        }

        for(int i = 0; i < playerUnits.Length; i++)
        {
            spawnedUnit = Instantiate(unitPrefab, enemySpawnPoints[i].position, Quaternion.identity);
            m_units.Add(spawnedUnit);
            spawnedUnit.Initialize(enemyUnits[i]);
            spawnedUnit.SetUnitRotation(180);
        }

        SetNextUnit();
    }

    private void SetNextUnit()
    {
        BaseUnit unit = GetNextUnit();
        activeUnit.SetActiveUnit(unit);
        activeUnit.unit.OnUnitTurnStart();

        ReduceUnitTimes(unit.time);
        UnitDetector.instance.SetUnit(unit);
        movementLimitObj.transform.position = activeUnit.unit.transform.position;

        movementLimitObj.enabled = unit.isPlayerUnit;
        UnitDetector.instance.visual.enabled = unit.isPlayerUnit;

        activeUnit.SetActiveUnit(unit);
    }

    private void ReduceUnitTimes(float time)
    {
        foreach(BaseUnit unit in m_units)
        {
            unit.time = Mathf.Max(0, unit.time - time);
        }
    }

    private BaseUnit GetNextUnit()
    {
        float lowestTime = float.MaxValue;
        BaseUnit selectedUnit = null;
        foreach(BaseUnit unit in m_units)
        {
            if(unit.time < lowestTime)
            {
                lowestTime = unit.time;
                selectedUnit = unit;
            }
        }
        return selectedUnit;
    }

    private void OnUnitDies(BaseUnit obj)
    {
        m_units.Remove(obj);
        obj.OnUnitDies -= OnUnitDies;

        if(m_units.TrueForAll((BaseUnit unit) => unit.isPlayerUnit))
        {
            //Player wins
        }
        else if(m_units.TrueForAll((BaseUnit unit) => !unit.isPlayerUnit))
        {
            //Enemy wins
        }
    }

}
