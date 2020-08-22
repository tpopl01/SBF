using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Brain_Laser_Turret_Player",
      menuName = "Modular/Components/Brain/Player/Brain_Laser_Turret_Player"
  )]
public class BrainLaserTurretPlayer : BrainVehiclePlayerBse
{
    protected override void PlayerInput(ModularController c)
    {
        base.PlayerInput(c);
        Transform cameraTrans = CameraManager.instance.cameraMain();
        c.Senses.TargetPos = cameraTrans.position + cameraTrans.forward * c.weaponSystem.GetActualRange();
        c.input.Aim = true;
    }
}
