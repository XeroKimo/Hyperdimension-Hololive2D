using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version 0.2
[CreateAssetMenu(menuName = "Equipment/Weapon", order = 1)]
public class Weapon : EquipmentBase
{
    public UnitHitDetectorConfig hitbox;
}