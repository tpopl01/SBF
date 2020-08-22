using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Brain_Hailfire_Player",
      menuName = "Modular/Components/Brain/Land/Brain_Hailfire_Player"
  )]
public class BrainHailfirePlayer : BrainVehiclePlayerBse
{
    protected override void PlayerInput(ModularController c)
    {
        base.PlayerInput(c);
        ModularControllerLandVehicle c1 = (ModularControllerLandVehicle)c;
        c.input.Aim = true;
        c.Senses.TargetPos = c.transform.position + c.transform.forward * 20;
        c.input.MoveAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        c.input.Speed += Input.GetAxis("Vertical") * Time.deltaTime;
        c.input.Speed = Mathf.Clamp(c.input.Speed, 0, 1);
        c1.Move(c.input.MoveAxis, c.input.Speed);
    }
}
