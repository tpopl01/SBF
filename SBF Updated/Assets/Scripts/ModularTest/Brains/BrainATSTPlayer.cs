using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Brain_ATST_Player",
      menuName = "Modular/Components/Brain/Land/Brain_ATST_Player"
  )]
public class BrainATSTPlayer : BrainVehiclePlayerBse
{
    protected override void PlayerInput(ModularController c)
    {
        base.PlayerInput(c);
        ModularControllerMoveable c1 = (ModularControllerMoveable)c;
        c1.input.Aim = true;
        c1.Move(c.transform.right * Input.GetAxis("Horizontal") + c.transform.forward*Input.GetAxis("Vertical"), 3);
        Transform cameraTrans = CameraManager.instance.cameraMain();
        c.Senses.TargetPos = cameraTrans.position + cameraTrans.forward * 30;
        c1.GetComponent<CreatureController>().SetRotateSpeed(c1.Senses.TargetPos);
    }
}
