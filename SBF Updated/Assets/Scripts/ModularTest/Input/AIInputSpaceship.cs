using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputSpaceship : InputBase//, ISetup
{
    //Spaceship s;
    //SpaceshipRayDetection rD;
    public BrainBase brain;


    public override void Execute(ModularController controller)
    {
        if(brain)
        {
            brain.Execute(controller);
        }
        //s.TryTakeOff();
        //Speed = controller.AIStats().GetWalkSpeed();

        //Vector3 moveDir = rD.CheckHits(out bool hit);
        //if(!hit) Speed = controller.AIStats().GetSprintSpeed();
        //if (moveDir == Vector3.zero) {
        //    moveDir = rD.GenerateRandom();
        //    Speed = controller.AIStats().GetRunSpeed();
        //}

        //s.Move(moveDir, Speed);
    }

   

    //public void SetUp(Transform root)
    //{
    //    s = root.GetComponentInChildren<Spaceship>();
    //    rD = GetComponentInChildren<SpaceshipRayDetection>();
    //}
}
