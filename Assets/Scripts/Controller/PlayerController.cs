using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(!BattleState.instance.activeUnit.unit.isPlayerUnit)
            return;

        if(Input.GetButtonDown("Submit"))
        {
            Attack();
        }
        if(Input.GetButtonDown("Cancel"))
        {
            Defend();
        }
    }

    private void FixedUpdate()
    {
        if(!BattleState.instance.activeUnit.unit.isPlayerUnit)
            return;


        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(inputDirection != Vector3.zero)
        {
            if(inputDirection.sqrMagnitude > 1)
                inputDirection.Normalize();
            MoveUnit(inputDirection);
        }
    }
}
