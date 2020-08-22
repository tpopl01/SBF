using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollPlayer : RollAnim
{
    InputBase inp;
    protected override void Init(Transform root)
    {
        inp = root.GetComponent<InputBase>();
        base.Init(root);
    }

    protected override void Begin()
    {
        if(inp.MoveAxis != Vector3.zero)
            base.Begin();
    }
}
