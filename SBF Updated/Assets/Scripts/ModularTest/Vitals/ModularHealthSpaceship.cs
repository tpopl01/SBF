using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularHealthSpaceship : ModularHealthMachine
{


    protected override bool Respawn()
    {
        bool retVal = base.Respawn();
        if (SpaceshipHangerManager.instance) {
            Spaceship s = GetComponent<Spaceship>();
            SpaceshipHangerManager.instance.AddShipToSpawnList(s, s.spaceshipTeam);
            Debug.Log("hanger");
        }
        return retVal;
    }
}
