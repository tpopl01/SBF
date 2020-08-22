using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBrainBase : InputBase
{
    public BrainBase brain;


    public override void Execute(ModularController controller)
    {
        if (brain)
        {
            brain.Execute(controller);
        }
    }
}
