using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public abstract class ModularHealthBase : MonoBehaviour, IHealth, ISetup, ITeamChange
{
    IRespawn[] respawns;
    IDeath[] deaths;
    protected bool Dead { get; private set; }
    protected float HP { get; set; }
    protected float MaxHP { get; private set; }
    protected float Armour { get; private set; }
    protected int Team { get; private set; }

    [SerializeField]bool debugDamageToggle;
    bool d;

    public void SetUp(Transform root)
    {
        ModularController c = root.GetComponent<ModularController>();
        if (c)
        {
            Team = c.Team;
        }
        respawns = root.GetComponentsInChildren<IRespawn>();
        deaths = GetComponentsInChildren<IDeath>();
        Init(c);
    }

    protected virtual void Init(ModularController c)
    {
        if (c)
        {
            MaxHP = c.AIStats().GetMaxHP();
            Armour = c.AIStats().GetArmour();
        }
        else
        {
            MaxHP = 100;
        }
        HP = MaxHP;
    }

    private void Update()
    {
        Tick();
    }

    protected virtual void Tick()
    {
        if (!Dead && d != debugDamageToggle)
        {
            d = debugDamageToggle;
            DamageHealth(5);
        }
    }

    public virtual bool DamageHealth(float amount)
    {
        if (Dead) return false;

        HP -= amount / Armour;
        if (HP <= 0)
        {
            ForceKill();
            return true;
        }
        return false;
    }

    public virtual void ForceKill()
    {
        if (Dead) return;
        Dead = true;
        HP = 0;
        Death();
    }


    public float GetHP()
    {
        return HP;
    }

    public float GetMaxHP()
    {
        return MaxHP;
    }

    public float GetHPPercent()
    {
        return HP / MaxHP;
    }

    public bool IsDead()
    {
        return Dead;
    }

    public void SetTeam(int team)
    {
        this.Team = team;
    }

    protected virtual bool Respawn()
    {
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);
        HP = MaxHP;
        Dead = false;
        if(respawns != null)
        for (int i = 0; i < respawns.Length; i++)
        {
            respawns[i].Respawn();
        }
        return true;
    }
    void Death()
    {
        if(deaths != null)
        for (int i = 0; i < deaths.Length; i++)
        {
            deaths[i].Death();
        }
    }
}
