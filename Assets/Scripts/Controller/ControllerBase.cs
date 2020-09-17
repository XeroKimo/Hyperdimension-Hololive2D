using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBase : MonoBehaviour
{

    protected void Attack()
    {
        List<BaseUnit> enemies = new List<BaseUnit>(UnitDetector.instance.enemyCollisions);
        if(enemies.Count == 0)
            return;

        foreach(BaseUnit enemy in enemies)
        {
            UnitStats myData = BattleState.instance.activeUnit.unit.GetUnitData().totalStats;
            UnitStats enemyData = enemy.GetUnitData().totalStats;

            int damage = myData.attack - enemyData.defense;

            damage = Mathf.Max(0, damage);

            enemy.ApplyDamage(damage);
        }
        BattleState.instance.StartNextTurn();
    }

    protected void Defend()
    {
        BattleState.instance.activeUnit.unit.Defend();
        BattleState.instance.StartNextTurn();
    }

    protected bool MoveUnit(Vector2 direction)
    {
        Vector2 nextPos = (Vector2)(BattleState.instance.activeUnit.unit.transform.position) + (direction * Constants.unitTravelSpeed * Time.fixedDeltaTime);
        
        if(IsNextPositionInBounds(nextPos))
        {
            BattleState.instance.activeUnit.unit.MoveUnit(nextPos);
            BattleState.instance.activeUnit.unit.RotateTowards(direction);
            return true;
        }

        return false;
    }
    private bool IsNextPositionInBounds(Vector2 nextPos)
    {
        return (nextPos - BattleState.instance.activeUnit.initialPos).sqrMagnitude < Constants.unitMaxUnitTravelDistance * Constants.unitMaxUnitTravelDistance;
    }
}
