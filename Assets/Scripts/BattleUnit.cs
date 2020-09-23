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

//Version: 0.21

[RequireComponent(typeof(Rigidbody2D))]
public class BattleUnit : MonoBehaviour
{
    //const int startingEnergyCost = 100;

    //[SerializeField]
    //UnitData m_unitData;
    //bool m_isDefending = false;

    //Rigidbody2D m_rigidBody;


    //public event System.Action<BattleUnit> OnUnitDies;
    //public event System.Action<BattleUnit, int, int, int> OnDamageTaken;
    //public event System.Action<BattleUnit, int, int, int> OnManaConsumed;

    //public float time = 0;
    //public bool isPlayerUnit = false;
    //public float rotation { get; private set; }
    //public BoxCollider2D unitCollider { get; private set; }
    //public SpriteRenderer sprite { get; private set; }

    //Color m_originalColor;

    //private void Awake()
    //{
    //    m_rigidBody = GetComponent<Rigidbody2D>();
    //}

    //public void Initialize(UnitData data)
    //{
    //    m_unitData = data;
    //    GameObject unitDisplay = Instantiate(data.unitDisplayPrefab, transform);
    //    sprite = unitDisplay.GetComponentInChildren<SpriteRenderer>();
    //    unitCollider = unitDisplay.GetComponent<BoxCollider2D>();

    //    Vector2 adjustedColliderSize = unitCollider.size;

    //    if(adjustedColliderSize.x < Constants.minUnitWidth)
    //    {
    //        adjustedColliderSize.x = Constants.minUnitWidth;
    //    }
    //    unitCollider.size = adjustedColliderSize;
    //    m_originalColor = sprite.material.GetColor("_OutlineColor");
    //    UseEnergy(startingEnergyCost);
    //}

    //public void OnUnitTurnStart()
    //{
    //    m_isDefending = false;
    //    m_rigidBody.bodyType = RigidbodyType2D.Dynamic;

    //    sprite.material.SetColor("_OutlineColor", m_originalColor);
    //    sprite.material.SetFloat("_OutlineWidth", Constants.unitOutlineWidth);
    //}

    //public void OnUnitEndTurn()
    //{
    //    m_rigidBody.bodyType = RigidbodyType2D.Static;
    //    sprite.material.SetFloat("_OutlineWidth", 0);
    //}

    //public void Defend()
    //{
    //    m_isDefending = true;
    //    UseEnergy(Constants.defendEnergyCost);
    //}

    //public void ApplyDamage(int amount)
    //{
    //    m_unitData.stats.currentHP = Mathf.Clamp(m_unitData.stats.currentHP - amount, 0, m_unitData.totalStats.maxHP);

    //    OnDamageTaken?.Invoke(this, m_unitData.stats.currentHP, m_unitData.totalStats.maxHP, amount);
    //    if(m_unitData.stats.currentHP == 0)
    //    {
    //        OnUnitDies?.Invoke(this);
    //        gameObject.SetActive(false);
    //    }
    //}

    //public bool IsAlive()
    //{
    //    return m_unitData.stats.currentHP > 0;
    //}

    //public void UseMana(int amount)
    //{
    //    m_unitData.stats.currentMP = Mathf.Clamp(m_unitData.stats.currentMP - amount, 0, m_unitData.totalStats.maxMP);
    //    OnDamageTaken?.Invoke(this, m_unitData.stats.currentMP, m_unitData.totalStats.maxMP, amount);
    //}

    //public bool HasEnoughMana(int amount)
    //{
    //    return m_unitData.stats.currentMP > amount;
    //}   

    //public void MoveUnit(Vector2 nextPos)
    //{
    //    m_rigidBody.MovePosition(nextPos);
    //}

    //public void RotateTowards(Vector2 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //    Quaternion quat = Quaternion.Lerp(Quaternion.Euler(0, 0, rotation), Quaternion.Euler(0, 0, angle), Time.fixedDeltaTime * 5);

    //    SetUnitRotation(quat.eulerAngles.z);
    //}

    //public void SetUnitRotation(float angle)
    //{
    //    rotation = angle;
    //    if((angle > 90 && angle < 270) ||
    //        (angle < -90 && angle > -180) ||
    //        (angle > 90 && angle < 180))
    //    {
    //        sprite.flipX = true;
    //    }
    //    else if((angle < 90 && angle > -90) ||
    //        angle > 270)
    //    {
    //        sprite.flipX = false;
    //    }
    //}

    //public UnitData GetUnitData()
    //{
    //    return m_unitData;
    //}

    //public void UseEnergy(int amount)
    //{
    //    time = amount / m_unitData.stats.speed;
    //}

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

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(!isActiveUnit)
            return;
    }

    public void Initialize(UnitData data, bool isPlayerUnit)
    {
        m_unitData = data;
        this.isPlayerUnit = isPlayerUnit;

        GameObject unitDisplay = Instantiate(data.unitDisplayPrefab, transform);

        unitCollider = unitDisplay.GetComponent<BoxCollider2D>();
        sprite = unitCollider.GetComponentInChildren<SpriteRenderer>();

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

        UnitHitDetector.instance.SetUnit(this);
        m_unitData.equipment.weapon.hitbox.ConfigureDetector(UnitHitDetector.instance);
        AttachCollider(m_unitData.equipment.weapon.hitbox.GetSize().x);
    }

    public void OnUnitEndTurn()
    {
        isActiveUnit = false;
        m_rigidBody.bodyType = RigidbodyType2D.Static;
        sprite.material.SetFloat("_OutlineWidth", 0);
    }

    public void Attack()
    {
        Debug.Assert(CanAttack(), "Can attack was not checked, before calling Attack");
        List<BattleUnit> enemies = new List<BattleUnit>(UnitHitDetector.instance.enemyCollisions);
        if(enemies.Count == 0)
            return;

        foreach(BattleUnit enemy in enemies)
        {
            UnitStats myData = unitData.totalStats;
            UnitStats enemyData = enemy.unitData.totalStats;

            int damage = myData.attack - enemyData.defense;

            damage = Mathf.Max(0, damage);

            enemy.ApplyDamage(damage);
        }

        UseEnergy(BattleConstants.attackEnergyCost);
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
        //UnitHitDetector.instance.transform.localPosition = new Vector3(unitOffset + (hitboxSize - unitOffset) / 2, 0);
        UnitHitDetector.instance.AdjustOffset(new Vector2(unitOffset + (hitboxSize - unitOffset) / 2, 0));
        UnitHitDetector.instance.transform.rotation = Quaternion.Euler(0, 0, m_rotation);
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

        if(isActiveUnit)
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
}
