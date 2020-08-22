using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrainAIBase : BrainBase
{
    protected override void Attack(ModularController controller, ModularController targetAgent)
    {
       // Debug.Log(targetAgent);
       // Debug.Log(controller.name);
        if (controller.Senses.InRange(targetAgent.Position))
        {
            controller.input.Aim = true;
            base.Attack(controller, targetAgent);
        }
    }
}
