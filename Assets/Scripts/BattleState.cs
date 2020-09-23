using System.Collections.Generic;
using UnityEngine;

//Version: 0.2

public class BattleState : MonoBehaviour
{
    public static BattleState instance;
    public event System.Action OnTurnStart;

    public SpriteRenderer movementLimitObj;


    public Transform[] playerSpawnPoints;
    public Transform[] enemySpawnPoints;

    [SerializeField]
    private BattleUnit m_unitPrefab;
    public BattleUnit activeUnit { get; private set; }
    public List<BattleUnit> units { get; private set; }

    public UnitStatusBar debugPlayerStatusBar;
    public UnitStatusBar debugEnemyStatusBar;

    public GameObject debugGameOverTitle;

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        units = new List<BattleUnit>();

        movementLimitObj.transform.localScale = new Vector3(BattleConstants.unitMaxUnitTravelDistance, BattleConstants.unitMaxUnitTravelDistance, 1) * 2;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartNextTurn()
    {
        activeUnit.OnUnitEndTurn();
        SetNextUnit();
    }

    public void InitializeBattle(UnitData[] playerUnits, UnitData[] enemyUnits)
    {
        BattleUnit spawnedUnit;

        Debug.LogWarning("Initialize battle is in debug mode, set loop count to the amount of units in the array");
        for(int i = 0; i < 1; i++)
        {
            spawnedUnit = Instantiate(m_unitPrefab, playerSpawnPoints[i].position, Quaternion.identity);
            units.Add(spawnedUnit);

            spawnedUnit.Initialize(playerUnits[i], true);
            spawnedUnit.OnUnitDies += OnUnitDies;

            debugPlayerStatusBar.TrackUnit(spawnedUnit);
        }

        for(int i = 0; i < 1; i++)
        {
            spawnedUnit = Instantiate(m_unitPrefab, enemySpawnPoints[i].position, Quaternion.identity);
            units.Add(spawnedUnit);
            spawnedUnit.Initialize(enemyUnits[i], false);
            spawnedUnit.OnUnitDies += OnUnitDies;

            debugEnemyStatusBar.TrackUnit(spawnedUnit);
        }
        SetNextUnit();
    }

    private void SetNextUnit()
    {
        BattleUnit unit = GetNextUnit();
        unit.OnUnitStartTurn();
        ReduceUnitTimes(unit.time);

        movementLimitObj.enabled = unit.isPlayerUnit;
        movementLimitObj.transform.position = unit.transform.position -
            (unit.sprite.transform.localPosition / 2);   //Offset by half the sprite's local position so visually
                                                                    //the sprite will hit the edge of the circle limit
        OnTurnStart?.Invoke();

        activeUnit = unit;
    }

    private void ReduceUnitTimes(float time)
    {
        foreach(BattleUnit unit in units)
        {
            unit.time = Mathf.Max(0, unit.time - time);
        }
    }

    private BattleUnit GetNextUnit()
    {
        float lowestTime = float.MaxValue;
        BattleUnit selectedUnit = null;
        foreach(BattleUnit unit in units)
        {
            if(unit.time < lowestTime && unit.IsAlive())
            {
                lowestTime = unit.time;
                selectedUnit = unit;
            }
        }
        return selectedUnit;
    }

    private void OnUnitDies(BattleUnit obj)
    {

        int enemyCount = 0;
        int allyCount = 0;
        foreach(BattleUnit unit in units)
        {
            if(!unit.isPlayerUnit && unit.IsAlive())
                enemyCount++;
            else if(unit.isPlayerUnit && unit.IsAlive())
                allyCount++;
        }

        if(enemyCount == 0)
        {
            //Player wins
            debugGameOverTitle.SetActive(true);
        }
        else if(allyCount == 0)
        {
            //Enemy wins
            debugGameOverTitle.SetActive(true);
        }
    }
}
