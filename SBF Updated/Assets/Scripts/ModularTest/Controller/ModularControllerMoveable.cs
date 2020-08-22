using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularControllerMoveable : ModularController
{
    public IMove[] iMove { get; private set; }
    public bool OnGround { get; private set; } = false;
    [HideInInspector] public Grounded grounded;

    protected override void Initialise()
    {
        grounded = GetComponentInChildren<Grounded>();
        if (grounded == null)
            grounded = gameObject.AddComponent<Grounded>();
        iMove = GetComponentsInChildren<IMove>();
        base.Initialise();
    }

    protected override void Tick()
    {
        OnGround = grounded.IsGrounded();


        base.Tick();
    }

    public void Move(Vector3 input, float speed)
    {
        for (int i = 0; i < iMove.Length; i++)
        {
            iMove[i].Move(input, speed);
        }
    }

}
