using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version: 0.2

public class PlayerController : ControllerBase
{

    // Start is called before the first frame update
    void Start()
    {
        //Debug.LogWarning("Controller Base Debug mode enabled");

        //unit.OnUnitTurnStart();
    }

    private void Update()
    {
        if(!BattleState.instance.activeUnit.isPlayerUnit)
            return;

        if(Input.GetButtonDown("Submit"))
        {
            BattleState.instance.activeUnit.Attack();
        }
        if(Input.GetButtonDown("Cancel"))
        {
            BattleState.instance.activeUnit.Defend();
        }
    }

    private void FixedUpdate()
    {
        if(!BattleState.instance.activeUnit.isPlayerUnit)
            return;


        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(inputDirection != Vector3.zero)
        {
            if(inputDirection.sqrMagnitude > 1)
                inputDirection.Normalize();
            BattleState.instance.activeUnit.MoveUnit(inputDirection);
        }
    }
}
