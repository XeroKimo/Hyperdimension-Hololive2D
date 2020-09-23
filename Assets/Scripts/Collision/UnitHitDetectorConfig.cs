using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version: 0.21

public abstract class UnitHitDetectorConfig : MonoBehaviour
{
    public abstract void ConfigureDetector(UnitHitDetector detector);
    public abstract Vector2 GetSize();
}
