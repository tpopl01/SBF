using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu
  (
      fileName = "Brain_ATST_AI",
      menuName = "Modular/Components/Brain/Land/Brain_ATST_AI"
  )]
public class BrainATSTAI : BrainAIAgentBase
{
    public override void Execute(ModularController controller)
    {
        ModularControllerLandVehicle c = (ModularControllerLandVehicle)controller;
        if (c.Senses.ClosestEnemy)
        {
            Attack(c, c.Senses.ClosestEnemy, c.Agent);
         //   c.weaponSystem.Attack(c.Senses.TargetPos, controller, c.Senses.ClosestEnemy);
        }
        else
        {
            MoveToRandomTarget(c, c.Agent, 20);
        }
        controller.GetComponent<CreatureController>().SetRotateSpeed(c.Agent.steeringTarget);
        c.Move(controller.transform.forward, c.AIStats().GetWalkSpeed());

    }
}
