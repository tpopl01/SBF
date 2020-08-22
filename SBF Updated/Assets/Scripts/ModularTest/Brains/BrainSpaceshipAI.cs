using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Brain_Spaceship_AI",
      menuName = "Modular/Components/Brain/Spaceship/Brain_Spaceship_AI"
  )]
public class BrainSpaceshipAI : BrainAISpaceshipBase
{
    //public override void Execute(ModularController controller)
    //{
    //    ModularControllerSpaceship c = (ModularControllerSpaceship)controller;
    //    //c.Spaceship.TryTakeOff();

    //    //c.input.Speed = controller.AIStats().GetWalkSpeed();

    //    //Vector3 moveDir = c.RayDetection.CheckHits(out bool hit);
    //    //if (!hit) c.input.Speed = controller.AIStats().GetSprintSpeed();
    //    //if (moveDir == Vector3.zero)
    //    //{
    //    //    moveDir = c.RayDetection.GenerateRandom();
    //    //}

    //}
    protected override void BehaviourTree(ModularControllerSpaceship c)
    {
        TakeOff(c);
        if (c.Spaceship.GetState() == ShipState.Air)
        {
            if (EscapeCollision(c))
            {

            }
            else
            {
                if (c.Senses.ClosestEnemy)
                {
                    if (TargetInRange(c, c.Senses.ClosestEnemy))
                    {
                        Attack(c, c.Senses.ClosestEnemy);
                    }
                    else
                    {
                        Approach(c, c.Senses.ClosestEnemy);
                    }
                }
                else
                {
                    if (c.Senses.ShotAtFrom != Vector3.zero)
                        FleeFrom(c, c.Senses.ShotAtFrom);
                    else
                        MoveToRandomTarget(c);
                }
            }
        }
    }
}
