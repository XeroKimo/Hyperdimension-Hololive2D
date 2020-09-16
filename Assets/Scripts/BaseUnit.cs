﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitStats
{
    public int currentHP;
    public int maxHP;
    public int currentMP;
    public int maxMP;

    public int attack;
    public int defense;
    public int hit;
    public int dodge;
    public int speed;
}

[System.Serializable]
public struct UnitEquipment
{
    public HeadGear headGear;
    public Weapon weapon;
    public BodyGear bodyGear;
    public LegGear legGear;

    public EquipmentStats[] GetEquipmentStats()
    {
        EquipmentStats[] stats = new EquipmentStats[4];

        stats[0] = headGear.stats;
        stats[1] = weapon.stats;
        stats[2] = bodyGear.stats;
        stats[3] = legGear.stats;

        return stats;
    }
}

[System.Serializable]
public struct UnitData
{
    public GameObject unitDisplayPrefab;
    public Vector2 unitSize;
    public UnitEquipment equipment;
    public UnitStats stats;

    public UnitStats totalStats
    {
        get
        {
            UnitStats totalUnitStat = stats;
            EquipmentStats[] equipmentStats = equipment.GetEquipmentStats();
            foreach(EquipmentStats equipment in equipmentStats)
            {
                stats.maxHP += equipment.maxHPModifier;
                stats.maxMP += equipment.maxMPModifier;
                stats.attack += equipment.attackModifier;
                stats.defense += equipment.defenseModifier;
                stats.hit += equipment.hitModifier;
                stats.dodge += equipment.dodgeModifier;
                stats.speed += equipment.speedModifier;
            }

            return totalUnitStat;
        }
    }
}


[RequireComponent(typeof(Rigidbody2D))]
public class BaseUnit : MonoBehaviour
{
    const int startingEnergyCost = 100;

    [SerializeField]
    UnitData m_unitData;
    bool m_isDefending = false;

    Rigidbody2D m_rigidBody;
    [SerializeField]
    SpriteRenderer m_sprite;

    public event System.Action<BaseUnit> OnUnitDies;

    public float time = 0;
    public bool isPlayerUnit = false;
    public float rotation { get; private set; }
    public BoxCollider2D unitCollider { get; private set; }

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(UnitData data)
    {
        m_unitData = data;
        GameObject unitDisplay = Instantiate(data.unitDisplayPrefab, transform);
        m_sprite = unitDisplay.GetComponent<SpriteRenderer>();
        unitCollider = unitDisplay.GetComponent<BoxCollider2D>();

        Vector2 adjustedColliderSize = unitCollider.size;

        if(adjustedColliderSize.x < Constants.minUnitWidth)
        {
            adjustedColliderSize.x = Constants.minUnitWidth;
        }
        unitCollider.size = adjustedColliderSize;

        UseEnergy(startingEnergyCost);
    }

    public void OnUnitTurnStart()
    {
        m_isDefending = false;


        m_rigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnUnitEndTurn()
    {
        m_rigidBody.bodyType = RigidbodyType2D.Static;
    }

    public void Defend()
    {
        m_isDefending = true;
        UseEnergy(50);
    }

    public void ApplyDamage(int amount)
    {
        m_unitData.stats.currentHP = Mathf.Clamp(m_unitData.stats.currentHP - amount, 0, m_unitData.totalStats.maxHP);

        if(m_unitData.stats.currentHP == 0)
        {
            OnUnitDies?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void UseMana(int amount)
    {
        m_unitData.stats.currentMP = Mathf.Clamp(m_unitData.stats.currentMP - amount, 0, m_unitData.totalStats.maxMP);
    }

    public bool HasEnoughMana(int amount)
    {
        return m_unitData.stats.currentMP > amount;
    }   

    public void MoveUnit(Vector2 nextPos)
    {
        m_rigidBody.MovePosition(nextPos);
    }

    public void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion quat = Quaternion.Lerp(Quaternion.Euler(0, 0, rotation), Quaternion.Euler(0, 0, angle), Time.fixedDeltaTime * 5);

        float nextAngle = Mathf.Lerp(rotation, angle, Time.fixedDeltaTime * 5);

        SetUnitRotation(quat.eulerAngles.z);
    }

    public void SetUnitRotation(float angle)
    {
        rotation = angle;
        if((angle > 90 && angle < 270) ||
            (angle < -90 && angle > -180) ||
            (angle > 90 && angle < 180))
        {
            m_sprite.flipX = true;
        }
        else if((angle < 90 && angle > -90) ||
            angle > 270)
        {
            m_sprite.flipX = false;
        }
    }

    public UnitData GetUnitData()
    {
        return m_unitData;
    }

    public void UseEnergy(int amount)
    {
        time = amount / m_unitData.stats.speed;
    }
}