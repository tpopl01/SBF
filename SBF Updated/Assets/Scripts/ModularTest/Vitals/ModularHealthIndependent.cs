using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularHealthIndependent : ModularHealthBase, IRepair
{
    [SerializeField] float repairRange = 3;
    ModelMaintenance modelMaintenance;

    void Start()
    {
        if (modelMaintenance == null)
        {
            Init(null);
        }
    }

    protected override void Init(ModularController c)
    {
        base.Init(c);
        modelMaintenance = GetComponent<ModelMaintenance>();
        if(!modelMaintenance)
        modelMaintenance = gameObject.AddComponent<ModelMaintenance>();
        modelMaintenance.Init(c);
    }

    public override void ForceKill()
    {
        base.ForceKill();
        modelMaintenance.Kill();
    }

    protected override bool Respawn()
    {
        base.Respawn();
        modelMaintenance.Respawn();
        return true;
    }

    public bool NeedsRepair(int team)
    {
        if (team == this.Team || this.Team == -1)
        {
            if (GetHPPercent() < 1)
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

    public bool Repair()
    {
        HP += 5;
        if (HP >= MaxHP)
        {
            HP = MaxHP;
            if(Dead)
                Respawn();
            return true;
        }
        return false;
    }

    public Vector3 Position()
    {
        return transform.position;
    }
}
