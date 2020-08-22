using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputTurret : InputBase
{
    [SerializeField] GameObject debugAim;

    public override void Execute(ModularController controller)
    {
        if(controller.debugAim && debugAim)
        {
            controller.Senses.TargetPos = debugAim.transform.position;
            controller.Aim(true);
            return;
        }
        if(controller.Senses.ClosestEnemy)
        {
          //  controller.Senses.TargetPos = controller.Senses.ClosestEnemy.Position;
            controller.Aim(true);
            controller.weaponSystem.Attack(controller.Senses.TargetPos, controller, controller.Senses.ClosestEnemy);
        }
        else
            controller.Aim(false);
    }
}
