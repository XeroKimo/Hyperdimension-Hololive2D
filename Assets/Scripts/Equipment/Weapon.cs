using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/Weapon", order = 1)]
public class Weapon : EquipmentBase
{
    public Vector2 hitboxSize = new Vector2(Constants.minUnitWidth, Constants.minUnitWidth);
    public Vector2 hitboxOffset = Vector2.zero;
}