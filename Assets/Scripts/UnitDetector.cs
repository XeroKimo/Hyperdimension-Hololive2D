using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetector : MonoBehaviour
{
    public static UnitDetector instance;

    BaseUnit m_unit;

    BoxCollider2D m_collider;

    public List<BaseUnit> allyCollisions = new List<BaseUnit>(3);
    public List<BaseUnit> enemyCollisions = new List<BaseUnit>(4);


    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        m_collider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, m_unit.rotation);
    }

    public void SetUnit(BaseUnit unit)
    {
        transform.parent = unit.transform;
        transform.localPosition = Vector3.zero;
        m_unit = unit;

        ResizeCollider(unit.GetUnitData().equipment.weapon.hitboxSize, unit.GetUnitData().equipment.weapon.hitboxOffset);
    }

    public void ResizeCollider(Vector2 size, Vector2 offset)
    {
        allyCollisions.Clear();
        enemyCollisions.Clear();

        m_collider.size = size;
        m_collider.offset = offset + new Vector2(m_unit.unitCollider.size.x, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseUnit unit = collision.GetComponent<BaseUnit>();
        if(unit)
        {
            if(unit.isPlayerUnit == m_unit.isPlayerUnit)
            {
                allyCollisions.Add(unit);
            }
            else
            {
                enemyCollisions.Add(unit);
            }
        }
        Debug.Log(collision.name);
    }
    private void OnTriggerExcit2D(Collider2D collision)
    {
        BaseUnit unit = collision.GetComponent<BaseUnit>();
        if(unit)
        {
            if(unit.isPlayerUnit == m_unit.isPlayerUnit)
            {
                allyCollisions.Remove(unit);
            }
            else
            {
                enemyCollisions.Remove(unit);
            }
        }
    }
}
