using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version: 0.23

public class UnitHitDetector : MonoBehaviour
{
    public static UnitHitDetector instance;

    public List<BattleUnit> allyCollisions { get; private set; }
    public List<BattleUnit> enemyCollisions { get; private set; }

    private BattleUnit m_unit;

    private System.Tuple<CircleCollider2D, SpriteRenderer> m_circleCollider;
    private System.Tuple<BoxCollider2D, SpriteRenderer> m_boxCollider;
    private System.Tuple<Collider2D, SpriteRenderer> m_activeCollider;

    private void Awake()
    {
        instance = this;

        allyCollisions = new List<BattleUnit>(3);
        enemyCollisions = new List<BattleUnit>(4);

        CircleCollider2D circleCollider = GetComponentInChildren<CircleCollider2D>(true);
        circleCollider.gameObject.SetActive(false);
        m_circleCollider = new System.Tuple<CircleCollider2D, SpriteRenderer>(circleCollider, circleCollider.GetComponentInChildren<SpriteRenderer>());
        m_circleCollider.Item1.GetComponent<CollisionCallbacks2D>().TriggerEnter2D += TriggerEnter2D;
        m_circleCollider.Item1.GetComponent<CollisionCallbacks2D>().TriggerExit2D += TriggerExit2D;

        BoxCollider2D boxCollider = GetComponentInChildren<BoxCollider2D>(true);
        boxCollider.gameObject.SetActive(false);
        m_boxCollider = new System.Tuple<BoxCollider2D, SpriteRenderer>(boxCollider, boxCollider.GetComponentInChildren<SpriteRenderer>());
        m_boxCollider.Item1.GetComponent<CollisionCallbacks2D>().TriggerEnter2D += TriggerEnter2D;
        m_boxCollider.Item1.GetComponent<CollisionCallbacks2D>().TriggerExit2D += TriggerExit2D;

        m_activeCollider = new System.Tuple<Collider2D, SpriteRenderer>(m_boxCollider.Item1, m_boxCollider.Item2);
    }

    public void SetUnit(BattleUnit unit)
    {
        m_unit = unit;
        m_activeCollider.Item2.enabled = unit.isPlayerUnit;
    }


    public void ConfigureBoxCollider(Vector2 size, Vector2 offset)
    {
        m_activeCollider.Item1.gameObject.SetActive(false);
        m_activeCollider = new System.Tuple<Collider2D, SpriteRenderer>(m_boxCollider.Item1, m_boxCollider.Item2);
        m_activeCollider.Item1.gameObject.SetActive(true);
        m_activeCollider.Item2.enabled = m_unit.isPlayerUnit;

        m_boxCollider.Item1.size = size;
        m_boxCollider.Item2.gameObject.transform.localScale = new Vector3(size.x, size.y);

        AdjustOffset(offset);
    }

    public void ConfigureCircleCollider(float radius, Vector2 offset)
    {
        m_activeCollider.Item1.gameObject.SetActive(false);
        m_activeCollider = new System.Tuple<Collider2D, SpriteRenderer>(m_circleCollider.Item1, m_boxCollider.Item2);
        m_activeCollider.Item1.gameObject.SetActive(true);
        m_activeCollider.Item2.enabled = m_unit.isPlayerUnit;

        m_circleCollider.Item1.radius = radius;
        m_circleCollider.Item2.gameObject.transform.localScale = new Vector3(radius, radius);

        AdjustOffset(offset);
    }

    public void AdjustOffset(Vector2 offset)
    {
        m_activeCollider.Item1.offset = offset;
        m_activeCollider.Item2.transform.localPosition = new Vector3(offset.x, offset.y);

        //RefreshCollisions();
    }

    public void RefreshCollisions()
    {
        ClearCollisions();
        CheckCollisions();
    }

    public void ClearCollisions()
    {
        List<BattleUnit> activeCollisions = new List<BattleUnit>(7);
        activeCollisions.AddRange(allyCollisions);
        activeCollisions.AddRange(enemyCollisions);

        foreach(BattleUnit unit in activeCollisions)
        {
            UntrackUnit(unit);
        }
        if(enemyCollisions.Count > 0)
        {
            Debug.Log(enemyCollisions.Count);
        }
    }

    public void CheckCollisions()
    {
        List<Collider2D> collisions = new List<Collider2D>();
        m_activeCollider.Item1.OverlapCollider(new ContactFilter2D().NoFilter(), collisions);

        foreach(Collider2D collision in collisions)
        {
            BattleUnit unit = collision.GetComponentInParent<BattleUnit>();

            if(unit)
            {
                TrackUnit(unit);
            }
        }
    }

    void TrackUnit(BattleUnit unit)
    {
        if(unit.isPlayerUnit == m_unit.isPlayerUnit && !allyCollisions.Exists((BattleUnit compare) => compare == unit))
        {
            allyCollisions.Add(unit);
        }
        if(unit.isPlayerUnit != m_unit.isPlayerUnit && !enemyCollisions.Exists((BattleUnit compare) => compare == unit))
        {
            enemyCollisions.Add(unit);

            unit.sprite.material.SetColor("_OutlineColor", BattleConstants.targetedUnitOutlineColor);
            unit.sprite.material.SetFloat("_OutlineWidth", 0.03f);
        }
    }

    void UntrackUnit(BattleUnit unit)
    {
        if(unit.isPlayerUnit == m_unit.isPlayerUnit)
        {
            allyCollisions.Remove(unit);
        }
        if(unit.isPlayerUnit != m_unit.isPlayerUnit)
        {
            enemyCollisions.Remove(unit);
            unit.sprite.material.SetFloat("_OutlineWidth", 0.00f);
        }
    }

    private void OnDestroy()
    {
        m_boxCollider.Item1.GetComponent<CollisionCallbacks2D>().TriggerEnter2D -= TriggerEnter2D;
        m_boxCollider.Item1.GetComponent<CollisionCallbacks2D>().TriggerExit2D -= TriggerExit2D;

        m_circleCollider.Item1.GetComponent<CollisionCallbacks2D>().TriggerEnter2D -= TriggerEnter2D;
        m_circleCollider.Item1.GetComponent<CollisionCallbacks2D>().TriggerExit2D -= TriggerExit2D;
    }

    private void TriggerEnter2D(Collider2D collision)
    {
        BattleUnit unit = collision.GetComponentInParent<BattleUnit>();
        if(unit)
        {
            TrackUnit(unit);
        }
    }

    private void TriggerExit2D(Collider2D collision)
    {
        BattleUnit unit = collision.GetComponentInParent<BattleUnit>();
        if(unit)
            UntrackUnit(unit);
    }
}
