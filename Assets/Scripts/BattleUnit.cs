using System.Collections;
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
    public UnitEquipment equipment;
    public UnitStats stats;

    public List<SkillData> skills;

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

public enum TargetTag
{
    Enemy,
    Ally
}

public enum HitboxStyle
{
    Attached
}

[System.Serializable]
public struct AttackDescription
{
    public int manaCost;
    public int energyCost;
    public int attackModifier;
    public TargetTag targetTag;

    public AttackDescription(int manaCost, int energyCost, int attackModifier, TargetTag targetTag)
    {
        this.manaCost = manaCost;
        this.energyCost = energyCost;
        this.attackModifier = attackModifier;
        this.targetTag = targetTag;
    }
}

[System.Serializable]
public struct HitboxDescription
{
    public HitboxStyle style;
    public UnitHitDetectorConfig hitboxConfig;

    public HitboxDescription(HitboxStyle style, UnitHitDetectorConfig hitboxConfig)
    {
        this.style = style;
        this.hitboxConfig = hitboxConfig;
    }
}

//Version: 0.23

[RequireComponent(typeof(Rigidbody2D))]
public class BattleUnit : MonoBehaviour
{
    public delegate void onUnitDies(BattleUnit affectedUnit);
    public delegate void onDamageTaken(BattleUnit affectedUnit, int damageTaken);
    public delegate void onManaConsumed(BattleUnit affectedUnit, int manaUsed);

    public event onUnitDies OnUnitDies;
    public event onDamageTaken OnDamageTaken;
    public event onManaConsumed OnManaConsumed;

    public float time;

    private UnitData m_unitData;
    public UnitData unitData { get => m_unitData; }
    public bool isPlayerUnit { get; private set; }

    Rigidbody2D m_rigidBody = null;
    public BoxCollider2D unitCollider { get; private set; }
    public SpriteRenderer sprite { get; private set; }

    bool m_isDefending = false;
    public bool isActiveUnit { get; private set; }
    Vector2 m_startingPos = Vector2.zero;
    float m_rotation = 0;

    AttackDescription m_attackDescription;
    HitboxDescription m_hitboxDescription;
    HitboxDescription m_defaultAttackHitbox;

    static readonly AttackDescription defaultAttackDescription = new AttackDescription(0, BattleConstants.attackEnergyCost, 0, TargetTag.Enemy);

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
    }

    public void Initialize(UnitData data, bool isPlayerUnit)
    {
        m_unitData = data;
        this.isPlayerUnit = isPlayerUnit;

        GameObject unitDisplay = Instantiate(data.unitDisplayPrefab, transform);

        unitCollider = unitDisplay.GetComponent<BoxCollider2D>();
        sprite = unitCollider.GetComponentInChildren<SpriteRenderer>();

        m_defaultAttackHitbox = new HitboxDescription(HitboxStyle.Attached, data.equipment.weapon.hitbox);
        m_attackDescription = defaultAttackDescription;

        UseEnergy(BattleConstants.startingEnergyCost);
    }

    public void OnUnitStartTurn()
    {
        m_isDefending = false;
        isActiveUnit = true;
        m_rigidBody.bodyType = RigidbodyType2D.Dynamic;
        m_startingPos = transform.position;

        sprite.material.SetColor("_OutlineColor", BattleConstants.selectedUnitOutlineColor);
        sprite.material.SetFloat("_OutlineWidth", BattleConstants.unitOutlineWidth);
        SkillMenu.instance.SetUnit(this);

        ConfigAttack(defaultAttackDescription, m_defaultAttackHitbox);
    }

    public void OnUnitEndTurn()
    {
        isActiveUnit = false;
        m_rigidBody.bodyType = RigidbodyType2D.Static;
        sprite.material.SetFloat("_OutlineWidth", 0);

        UnitHitDetector.instance.ClearCollisions();
    }

    public void Attack()
    {
        Debug.Assert(CanAttack(), "Can attack was not checked, before calling Attack");
        List<BattleUnit> targets = new List<BattleUnit>(
            (m_attackDescription.targetTag == TargetTag.Enemy) ?
            UnitHitDetector.instance.enemyCollisions :
            UnitHitDetector.instance.allyCollisions);

        if(targets.Count == 0)
            return;

        ConsumeMana(m_attackDescription.manaCost);

        foreach(BattleUnit enemy in targets)
        {
            UnitStats myData = unitData.totalStats;
            UnitStats enemyData = enemy.unitData.totalStats;

            int damage = myData.attack + m_attackDescription.attackModifier - enemyData.defense;

            damage = Mathf.Max(0, damage);

            enemy.ApplyDamage(damage);
        }

        UseEnergy(m_attackDescription.energyCost);
        BattleState.instance.StartNextTurn();
    }

    public void Defend()
    {
        m_isDefending = true;
        UseEnergy(BattleConstants.defendEnergyCost);
        BattleState.instance.StartNextTurn();
    }

    public bool MoveUnit(Vector2 direction)
    {
        Vector2 nextPos = (Vector2)(transform.position) + (direction * BattleConstants.unitTravelSpeed * Time.fixedDeltaTime);

        if(IsNextPositionInBounds(nextPos))
        {
            m_rigidBody.MovePosition(nextPos);
            RotateTowards(direction);
            return true;
        }

        return false;
    }

    public void ApplyDamage(int amount)
    {
        m_unitData.stats.currentHP = Mathf.Clamp(unitData.stats.currentHP - amount, 0, unitData.totalStats.maxHP);

        OnDamageTaken?.Invoke(this, amount);
        if(m_unitData.stats.currentHP == 0)
        {
            OnUnitDies?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public void ConsumeMana(int amount)
    {
        Debug.Assert(HasEnoughMana(amount), "Mana was not checked");

        m_unitData.stats.currentMP = Mathf.Clamp(m_unitData.stats.currentMP - amount, 0, m_unitData.totalStats.maxMP);
        OnManaConsumed?.Invoke(this, amount);
    }

    public bool IsAlive()
    {
        return m_unitData.stats.currentHP > 0;
    }

    public bool CanAttack()
    {
        Debug.Assert(isActiveUnit, gameObject.name + " is not the active Unit");

        return UnitHitDetector.instance.enemyCollisions.Count > 0;
    }

    public bool HasEnoughMana(int amount)
    {
        return unitData.stats.currentMP > amount;
    }

    private void AttachCollider(float hitboxSize)
    {
        Debug.Assert(isActiveUnit);
        float unitOffset = unitCollider.size.x;

        UnitHitDetector.instance.transform.parent = transform;
        UnitHitDetector.instance.transform.localPosition = Vector3.zero;
        UnitHitDetector.instance.transform.rotation = Quaternion.Euler(0, 0, m_rotation);
        //UnitHitDetector.instance.transform.localPosition = new Vector3(unitOffset + (hitboxSize - unitOffset) / 2, 0);
        UnitHitDetector.instance.AdjustOffset(new Vector2(unitOffset + (hitboxSize - unitOffset) / 2, 0));
    }

    private void DetachCollider()
    {
        Debug.Assert(isActiveUnit);
        UnitHitDetector.instance.transform.parent = null;
    }


    private void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion quat = Quaternion.Lerp(Quaternion.Euler(0, 0, m_rotation), Quaternion.Euler(0, 0, angle), Time.fixedDeltaTime * 5);

        m_rotation = quat.eulerAngles.z;

        if(isActiveUnit && m_hitboxDescription.style == HitboxStyle.Attached)
        {
            UnitHitDetector.instance.transform.rotation = quat;
        }
    }

    private void UseEnergy(int amount)
    {    
        time = amount / unitData.stats.speed;
    }

    private bool IsNextPositionInBounds(Vector2 nextPos)
    {
        return (nextPos - m_startingPos).sqrMagnitude < BattleConstants.unitMaxUnitTravelDistance * BattleConstants.unitMaxUnitTravelDistance; 
    }

    void ConfigAttack(AttackDescription attackDescription, HitboxDescription hitboxDescription)
    {
        m_attackDescription = attackDescription;
        m_hitboxDescription = hitboxDescription;

        UnitHitDetector.instance.SetUnit(this);
        hitboxDescription.hitboxConfig.ConfigureDetector(UnitHitDetector.instance);

        if(hitboxDescription.style == HitboxStyle.Attached)
        {
            AttachCollider(hitboxDescription.hitboxConfig.GetSize().x);
        }
    }
}
