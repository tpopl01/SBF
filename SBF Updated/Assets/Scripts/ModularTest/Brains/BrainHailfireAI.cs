using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu
  (
      fileName = "Brain_Hailfire_AI",
      menuName = "Modular/Components/Brain/Land/Brain_Hailfire_AI"
  )]
public class BrainHailfireAI : BrainAIAgentBase
{
    public override void Execute(ModularController controller)
    {
        ModularControllerLandVehicle c = (ModularControllerLandVehicle)controller;
        if (c.Senses.ClosestEnemy)
        {
            c.weaponSystem.Attack(c.Senses.TargetPos, controller, c.Senses.ClosestEnemy);
        }
        else
        {
            MoveToRandomTarget(c, c.Agent, 20);
        }
        c.Move(controller.transform.forward, c.Agent.speed);
    }

}
