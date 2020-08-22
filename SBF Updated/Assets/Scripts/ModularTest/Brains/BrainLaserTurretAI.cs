using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Brain_Laser_Turret_AI",
      menuName = "Modular/Components/Brain/Turrets/Brain_Laser_Turret_AI"
  )]
public class BrainLaserTurretAI : BrainAIBase
{
    public override void Execute(ModularController controller)
    {
        ModularController closestEnemy = controller.Senses.ClosestEnemy;
        if(closestEnemy)
            Attack(controller, closestEnemy);
        //if(controller.Aiming)
        //{
        //    controller.weaponSystem.Attack(controller.Senses.TargetPos, controller, closestEnemy);
        //}
    }
}
