using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularControllerSpaceship : ModularControllerMoveable
{
    public Spaceship Spaceship { get; private set; }
    public SpaceshipRayDetection RayDetection { get; private set; }
    public EnterExit EnterExit { get; private set; }

    protected override void Initialise()
    {
        base.Initialise();
        Spaceship = GetComponentInChildren<Spaceship>();
        RayDetection = GetComponentInChildren<SpaceshipRayDetection>();
        EnterExit = GetComponentInChildren<EnterExit>();
    }

}
