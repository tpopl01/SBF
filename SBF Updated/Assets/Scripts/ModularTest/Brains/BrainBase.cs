using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BrainBase : ScriptableObject
{
    public abstract void Execute(ModularController controller);

    protected virtual void Attack(ModularController controller, ModularController targetAgent)
    {
        InputBase inp = controller.input;
        if (inp.Aim)
        {
            if (controller.Aiming && targetAgent.Health.IsDead()==false)
            {
                controller.Senses.TargetPos = targetAgent.Senses.IdealHitPos;
                Vector3 dir = controller.Senses.TargetPos - controller.Position;
                if (Vector3.Angle(controller.transform.forward, dir) < 30)
                {
                    if (controller.Senses.TargetPos != Vector3.zero)
                    {
                        inp.Attack = true;
                        controller.weaponSystem.Attack(controller.Senses.TargetPos, controller, targetAgent);
                    }
                }
            }
        }
    }
}
