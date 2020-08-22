using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class ModularHealthRespawnOverTime : ModularHealthBase
{
    Timer respawnTimer = new Timer(3);

    protected override void Tick()
    {
        base.Tick();
        if (Dead)
        {
            if (respawnTimer.GetComplete())
            {
                Respawn();
                respawnTimer.StartTimer();
            }
        }
    }

    public override void ForceKill()
    {
        base.ForceKill();
        respawnTimer.StartTimer();
    }

}
