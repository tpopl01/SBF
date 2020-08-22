using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularHealthLandMachine : ModularHealthEnterable, ITick
{
    Rigidbody rb;
    float storedPos;

    protected override void Init(ModularController c)
    {
        base.Init(c);
        rb=c.GetComponentInChildren<Rigidbody>();
    }

    public void Tick()
    {
      //  base.Tick();
        DetectFallDeath();
    }

    void DetectFallDeath()
    {
        if (rb.drag == 0)
        {
            if (storedPos > transform.position.y + 50)
            {
                Debug.Log("Fall to death");
                ForceKill();
                //temp
                // transform.position = Vector3.up;
            }
        }
        else storedPos = transform.position.y;
    }
}
