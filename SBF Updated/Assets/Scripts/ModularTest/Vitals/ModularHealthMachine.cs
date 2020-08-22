using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class ModularHealthMachine : ModularHealthRespawnOverTime, IRepair
{
    ModelMaintenance modelMaintenance;
    float healSpeed;
    [SerializeField] float repairRange = 8;
    Timer respawnTimer = new Timer(2);

    void Start()
    {
        if(modelMaintenance == null)
        {
            Init(null);
        }
    }
    protected override void Init(ModularController c)
    {
        base.Init(c);
        modelMaintenance = gameObject.AddComponent<ModelMaintenance>();
        modelMaintenance.Init(c);
        if(c)
        {
            healSpeed = c.AIStats().GetHealingSpeed();
        }
        else
            healSpeed = 1;
    }

    protected override void Tick()
    {
        base.Tick();
        if(Dead)
            if(respawnTimer.GetComplete())
            {
                Respawn();
            }
    }

    public bool NeedsRepair(int team)
    {
        if (team == this.Team || this.Team == -1)
        {
            if (GetHPPercent() < 1)
            { 
                return true;
            }
        }
        return false;
    }

    public bool Repair()
    {
        HP += healSpeed;
        if(HP >= MaxHP)
        {
            HP = MaxHP;
            if (Dead)
                Respawn();
            return true;
        }
        return false;
    }

    public bool InRange(Vector3 pos)
    {
        if (Vector3.Distance(pos, transform.position) < repairRange)
        {
            return true;
        }
        return false;
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public override void ForceKill()
    {
        if (Dead) return;
        base.ForceKill();
        modelMaintenance.Kill();
        respawnTimer.StartTimer();
    }

    protected override bool Respawn()
    {
        base.Respawn();
        modelMaintenance.Respawn();
        return true;
    }
}
