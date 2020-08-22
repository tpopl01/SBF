using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Brain_Spaceship_Player",
      menuName = "Modular/Components/Brain/Spaceship/Brain_Spaceship_Player"
  )]
public class BrainSpaceshipPlayer : BrainPlayerBase
{
    protected override void PlayerInput(ModularController c)
    {
        ModularControllerSpaceship c1 = (ModularControllerSpaceship)c;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            c1.Spaceship.TryTakeOff();
            c1.Spaceship.TryLand();
        }

        if (c1.Spaceship.GetState() == ShipState.Idle)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                c1.EnterExit.Exit();
            }
        }
        else if (c1.Spaceship.GetState() == ShipState.Air)
        {
            c1.input.Aim = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (c1.Spaceship.GetState() == ShipState.Air)
                {
                    c1.Spaceship.BeginBoost();

                }
            }
        }

        c.input.MoveAxis = Camera.main.transform.position + Camera.main.transform.forward * 20; //new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) + controller.transform.forward + controller.transform.position;
        c.input.Speed += Input.GetAxis("Vertical") * Time.deltaTime * 10;
        if (c.input.Speed > c.AIStats().GetSprintSpeed()) c.input.Speed = c.AIStats().GetSprintSpeed();
        c1.Move(c.input.MoveAxis, c.input.Speed);
    }
}
