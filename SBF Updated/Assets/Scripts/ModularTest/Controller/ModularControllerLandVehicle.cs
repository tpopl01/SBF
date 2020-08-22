using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ModularControllerLandVehicle : ModularControllerMoveable
{
    public EnterExit EnterExit { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    protected override void Initialise()
    {
        base.Initialise();
        EnterExit = GetComponentInChildren<EnterExit>();
        Agent = GetComponentInChildren<NavMeshAgent>();
    }
}
