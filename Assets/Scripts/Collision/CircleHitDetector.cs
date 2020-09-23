using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version: 0.21

[RequireComponent(typeof(CircleCollider2D))]
public class CircleHitDetector : UnitHitDetectorConfig
{
    CircleCollider2D m_collider;

    void Awake()
    {
        m_collider = GetComponent<CircleCollider2D>();
    }

    public override void ConfigureDetector(UnitHitDetector detector)
    {
        if(!m_collider)
            m_collider = GetComponent<CircleCollider2D>();

        detector.ConfigureCircleCollider(m_collider.radius, m_collider.offset);
    }

    public override Vector2 GetSize()
    {
        return new Vector2(m_collider.radius, m_collider.radius);
    }
}
