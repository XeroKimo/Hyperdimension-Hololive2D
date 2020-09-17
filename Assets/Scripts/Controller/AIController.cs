using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : ControllerBase
{

    private void FixedUpdate()
    {
        if(BattleState.instance.activeUnit.unit.isPlayerUnit)
            return;

        if(!IsInvoking("Defend"))
            Invoke("Defend", 1);
    }
}
