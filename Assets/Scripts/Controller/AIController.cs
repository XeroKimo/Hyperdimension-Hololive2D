using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Version 0.2
public class AIController : ControllerBase
{

    private void FixedUpdate()
    {
        if(BattleState.instance.activeUnit.isPlayerUnit)
            return;

        if(!IsInvoking("Defend"))
            Invoke("Defend", 1);
    }

    void Defend()
    {
        BattleState.instance.activeUnit.Defend();
    }
}
