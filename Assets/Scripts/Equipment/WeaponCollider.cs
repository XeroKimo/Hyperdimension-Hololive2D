using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    const int maxOverlappingUnits = 7;

    List<BaseUnit> m_overlappingUnits = new List<BaseUnit>(maxOverlappingUnits);

    public List<BaseUnit> overlappingUnits { get => m_overlappingUnits; }
    public BaseUnit owningUnit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseUnit unit = collision.GetComponent<BaseUnit>();
        if(unit && unit.isPlayerUnit != owningUnit.isPlayerUnit)
        {
            m_overlappingUnits.Add(unit);
        }
        Debug.Log(collision.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_overlappingUnits.Remove(collision.GetComponent<BaseUnit>());
    }

}