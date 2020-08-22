using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainVehiclePlayerBse : BrainPlayerBase
{
    int skipFrame = 0;
    protected override void PlayerInput(ModularController c)
    {
        if (skipFrame != 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                c.GetComponent<EnterExit>().Exit();
                skipFrame = 0;
            }
        }
        else skipFrame++;
    }
}
