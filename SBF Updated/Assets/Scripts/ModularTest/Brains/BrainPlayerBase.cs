using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrainPlayerBase : BrainBase
{
    public override void Execute(ModularController controller)
    {
        controller.input.Aim = Input.GetKeyDown(KeyCode.Mouse1);
        controller.input.Attack = Input.GetKeyDown(KeyCode.Mouse0);
        PlayerInput(controller);
        if (controller.input.Attack)
        {
            RaycastShoot(controller);
        }
    }

    protected abstract void PlayerInput(ModularController c);

    protected void RaycastShoot(ModularController c)
    {
        if (c.Aiming)
        {
            Debug.Log("Shoot");
            Transform cameraTrans = CameraManager.instance.cameraMain();
            Debug.Log(CameraManager.instance.cameraMain());
            c.Senses.TargetPos = cameraTrans.position + cameraTrans.forward * c.weaponSystem.GetActualRange();
            c.weaponSystem.Attack(c.Senses.TargetPos, c, c.Senses.ClosestEnemy);
        }
    }
}
