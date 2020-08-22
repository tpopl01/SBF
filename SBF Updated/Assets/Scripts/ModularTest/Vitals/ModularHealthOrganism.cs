using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Events;
using tpopl001.Utils;

public class ModularHealthOrganism : ModularHealthRespawnOverTime, IHealable
{
    float healSpeed;
    float storedPos;
    Rigidbody rb;
    public Ragdoll r;
    Timer getupTimer = new Timer(3);
    bool forceAdded = false;
    Stats s;

    protected override void Init(ModularController c)
    {
        base.Init(c);
        rb = GetComponent<Rigidbody>();
        healSpeed = c.AIStats().GetHealingSpeed();
        r = c.GetComponentInChildren<Ragdoll>();
        if (r == null)
            r = c.gameObject.AddComponent<Ragdoll>();
        s = c.Stats;
    }

    public bool Heal()
    {
        HP += healSpeed;
        if (HP > MaxHP)
        {
            HP = MaxHP;
            return true;
        }
        return false;
    }

    public bool NeedsHealing(int team)
    {
        if (team == this.Team || this.Team == -1)
        {
            if (GetHPPercent() < 1)
                return true;
        }
        return false;
    }
    public Vector3 Position()
    {
        return transform.position;
    }

    protected override void Tick()
    {
        base.Tick();
        DetectFallDeath();
        if(forceAdded && getupTimer.GetComplete())
        {
            forceAdded = false;
            if (!Dead)
                r.GetUp();
        }
    }

    public void DamageHPExplosive(float damage, Vector3 origin, float radius, float force = 5)
    {
        //    Debug.Log("Explode");
        AddExplosiveForce(origin, radius, force);
    }

    void AddExplosiveForce(Vector3 origin, float radius, float force = 5)
    {
        if (!forceAdded)
        {
            forceAdded = true;
            r.AddExplosiveForce(origin, radius, force);
            getupTimer.StartTimer();
        }
    }

    public void RagdollChar()
    {
        if (!forceAdded)
        {
            forceAdded = true;
            r.RagdollCharacter();
            getupTimer.StartTimer();
        }
    }

    void DetectFallDeath()
    {
        if (rb.drag == 0)
        {
            if(storedPos > transform.position.y + 50)
            {
                Debug.Log("Fall to death");
                ForceKill();
                //temp
               // transform.position = Vector3.up;
            }
        }
        else storedPos = transform.position.y;
    }

    public override void ForceKill()
    {
        if (Dead) return;
        base.ForceKill();
        r.RagdollCharacter();
        EventHandling.Death(Team);
        s.OnDie();
    }

    protected override bool Respawn()
    {
        ISpawner c = ResourceManagerModular.instance.GetSpawner(Team);
        if (c != null)
        {
            //   gameObject.SetActive(false);
            //    string s = "Modular/Pickups/";
            //   if (Random.Range(0, 10) < 5)
            //       s += "ammo_pack";
            //  else s += "bacta_tank";

            //    Instantiate(Resources.Load(s), transform.position, Quaternion.identity);

            if (Random.Range(0, 10) < 5)
            {
                ResourceManagerModular.instance.SpawnAmmo(transform.position);
            }
            else
            {
                ResourceManagerModular.instance.SpawnHealth(transform.position);
            }
            transform.position = c.GetSpawnPosition();
            base.Respawn();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            gameObject.SetActive(true);
            if (r.Ragdolled) r.DeRagdoll();
            return true;
        }
        Debug.Log("Respawn Failed");
        gameObject.SetActive(false);
        return false;
    }
}
