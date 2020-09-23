using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version: 0.21

[RequireComponent(typeof(BoxCollider2D))]
public class BoxHitDetector : UnitHitDetectorConfig
{
    BoxCollider2D m_collider;

    void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }

    public override void ConfigureDetector(UnitHitDetector detector)
    {
        if(!m_collider)
            m_collider = GetComponent<BoxCollider2D>();
        m_collider = GetComponent<BoxCollider2D>();
        //Debug.Assert(m_collider != null);

        detector.ConfigureBoxCollider(m_collider.size, m_collider.offset);
    }

    public override Vector2 GetSize()
    {
        return m_collider.size;
    }
}
