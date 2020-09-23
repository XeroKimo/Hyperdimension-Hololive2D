using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version 0.2

public static class BattleConstants
{
    public const float unitMaxUnitTravelDistance = 144;
    public const float unitTravelSpeed = 144;

    public const float minUnitWidth = 32;

    public const int attackEnergyCost = 100;
    public const int defendEnergyCost = 50;
    public const int startingEnergyCost = 100;

    public static readonly Color selectedUnitOutlineColor = Color.green;
    public static readonly Color targetedUnitOutlineColor = Color.red;
    public const float unitOutlineWidth = 0.03f;

}
