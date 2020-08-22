using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class ResourceManagerModular : MonoBehaviour
{
    IRepair[] repairs;
    IHealable[] healables;
    IGoal[] goals;
   // IAmmoAdd[] ammos;
    List<IAmmoGen> ammoGens = new List<IAmmoGen>();
    List<IHealer> healers = new List<IHealer>();
    List<IPickable> collectWeapons = new List<IPickable>();
    IEnter[] vehicles;
    ISpawner[] spawners;

    HealthPackPool healthPool;
    AmmoPackPool ammoPool;

   // Timer time_between_heal = new Timer(0.8f);
   // Timer time_between_ammo = new Timer(1.2f);
  //  int burstHeal = 0;
   // int burstAmmo = 0;

    ModularCommandPostCapturable[] modularCommandPosts = new ModularCommandPostCapturable[0];

    private void Awake()
    {
        instance = this;

        List<ISpawner> spawners = new List<ISpawner>();
        Transform[] allTransforms = GameObject.FindObjectsOfType<Transform>();
        for (int i = 0; i < allTransforms.Length; i++)
        {
            if (allTransforms[i].GetComponent<ISpawner>() != null)
            {
                spawners.Add(allTransforms[i].GetComponent<ISpawner>());
            }
        }
        this.spawners = spawners.ToArray();
    }

    private void Start()
    {
        healthPool = GetComponentInChildren<HealthPackPool>();
        ammoPool = GetComponentInChildren<AmmoPackPool>();
        modularCommandPosts = GameObject.FindObjectsOfType<ModularCommandPostCapturable>();
        List<IRepair> repairables = new List<IRepair>();
        List<IGoal> goals = new List<IGoal>();
        List<IEnter> vehicles = new List<IEnter>();
        List<IHealable> healables = new List<IHealable>();


        Transform[] allTransforms = GameObject.FindObjectsOfType<Transform>();
        for (int i = 0; i < allTransforms.Length; i++)
        {
            if(allTransforms[i].GetComponent<IHealable>()!=null)
            {
                healables.Add(allTransforms[i].GetComponent<IHealable>());
            }
            if(allTransforms[i].GetComponent<IRepair>() != null)
            {
                repairables.Add(allTransforms[i].GetComponent<IRepair>());
            }
            if (allTransforms[i].GetComponent<IGoal>() != null)
            {
                goals.Add(allTransforms[i].GetComponent<IGoal>());
            }
            if (allTransforms[i].GetComponent<IEnter>() != null)
            {
                vehicles.Add(allTransforms[i].GetComponent<IEnter>());
            }
            //     if (allTransforms[i].GetComponent<IAmmoAdd>() != null)
            //    {
            //       ammos.Add(allTransforms[i].GetComponent<IAmmoAdd>());
            //   }
        }

        this.goals = goals.ToArray();
        this.repairs = repairables.ToArray();
        this.vehicles = vehicles.ToArray();
        this.healables = healables.ToArray();
        //this.healables = healables.ToArray();
      //  this.ammos = ammos.ToArray();
       // Debug.Log("Healables " + healables.Count);
       // Debug.Log("Ammos " + ammos.Count);
    }

    public IHealable GetHealable(float range, int team, Vector3 pos)
    {
        for (int i = 0; i < healables.Length; i++)
        {
            if(healables[i].NeedsHealing(team))
            {
                if(Vector3.Distance(healables[i].Position(), pos)<range)
                {
                    return healables[i];
                }
            }
        }
        return null;
    }

    public void SpawnHealth(Vector3 pos)
    {
        healthPool.SpawnHealth(pos);
    }

    public void SpawnAmmo(Vector3 pos)
    {
        ammoPool.SpawnAmmo(pos);
    }

    public void ReturnAmmo(AmmoPickable a)
    {
        ammoPool.ReturnAmmo(a);
        RemoveAmmoGen(a);
    }

    public void ReturnHealth(HealthPickable h)
    {
        healthPool.ReturnHealth(h);
        RemoveHealer(h);
    }

    public IEnter GetNearestVehicle(Vector3 position)
    {
        float dist = Mathf.Infinity;
        IEnter g = null;
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i].CanEnter() == false)
                continue;

            float tempDist = Vector3.Distance(position, vehicles[i].GetPosition());
            if (tempDist < dist)
            {
                dist = tempDist;
                g = vehicles[i];
            }
        }

        return g;
    }

    public bool GetNearestRepair(int team, Vector3 position, out IRepair r)
    {
        float dist = Mathf.Infinity;
        r = null;
        for (int i = 0; i < repairs.Length; i++)
        {
            if(repairs[i].NeedsRepair(team))
            {
                float tempDist = Vector3.Distance(position, repairs[i].Position());
                if (tempDist < dist)
                {
                    dist = tempDist;
                    r = repairs[i];
                }
            }
        }
        return r != null;
    }

    //public bool GetNearestHealable(int team, Vector3 position, out IHealable r)
    //{
    //    float dist = Mathf.Infinity;
    //    r = null;
    //    for (int i = 0; i < healables.Length; i++)
    //    {
    //        if (healables[i].NeedsHealing(team))
    //        {
    //            float tempDist = Vector3.Distance(position, healables[i].Position());
    //            if (tempDist < dist)
    //            {
    //                dist = tempDist;
    //                r = healables[i];
    //            }
    //        }
    //    }
    //    return r != null;
    //}

    //public List<IHealable> GetHealablesInRange(Vector3 position, float range, int team = -1)
    //{
    //    List<IHealable> r = new List<IHealable>();
    //    for (int i = 0; i < healables.Length; i++)
    //    {
    //        if (healables[i].NeedsHealing(team))
    //        {
    //            float tempDist = Vector3.Distance(position, healables[i].Position());
    //            if (tempDist < range)
    //            {
    //                r.Add(healables[i]);
    //            }
    //        }
    //    }
    //    return r;
    //}

    public IGoal GetNearestGoal(Vector3 position, int team = -1)
    {
        float dist = Mathf.Infinity;
        IGoal g = null;
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].GetNearestGoal(team))
            {
                float tempDist = Vector3.Distance(position, goals[i].Position());
                if (tempDist < dist)
                {
                    dist = tempDist;
                    g = goals[i];
                }
            }
        }
        return g;
    }

    public IGoal GetNearestGoal(Vector3 position)
    {
        float dist = Mathf.Infinity;
        IGoal g = null;
        for (int i = 0; i < goals.Length; i++)
        {
            float tempDist = Vector3.Distance(position, goals[i].Position());
            if (tempDist < dist)
            {
                dist = tempDist;
                g = goals[i];
            }
        }

        return g;
    }

    public ModularCommandPostCapturable GetCaptureCP(Vector3 position)
    {
        float dist = Mathf.Infinity;
        ModularCommandPostCapturable g = null;
        for (int i = 0; i < modularCommandPosts.Length; i++)
        {
            float tempDist = Vector3.Distance(position, modularCommandPosts[i].Position());
            if (tempDist < dist)
            {
                dist = tempDist;
                g = modularCommandPosts[i];
            }
        }

        return g;
    }

    public ISpawner GetSpawner(int team)
    {
        int index = Random.Range(0, spawners.Length - 1);
        for (int i = index; i < spawners.Length; i++)
        {
            if (!spawners[i].CanSpawn(team)) continue;
            return spawners[i];
        }
        for (int i = 0; i < index; i++)
        {
            if (!spawners[i].CanSpawn(team)) continue;
            return spawners[i];
        }

        return null;
    }

    public ModularCommandPostCapturable GetCaptureCPTeam(Vector3 position, int team)
    {
        float dist = Mathf.Infinity;
        ModularCommandPostCapturable g = null;
        for (int i = 0; i < modularCommandPosts.Length; i++)
        {
            if (modularCommandPosts[i].GetTeam() != team) continue;

            float tempDist = Vector3.Distance(position, modularCommandPosts[i].Position());
            if (tempDist < dist)
            {
                dist = tempDist;
                g = modularCommandPosts[i];
            }
        }

        return g;
    }

    public IGoal GetNearestGoalNot(Vector3 position, int team = -1)
    {
        float dist = Mathf.Infinity;
        IGoal g = null;
        for (int n = 0; n < GameManagerModular.instance.Teams.Length; n++)
        {
            int t = GameManagerModular.instance.Teams[n].GetTeam();
            if (team == t)
                continue;
            for (int i = 0; i < goals.Length; i++)
            {
                if (goals[i].GetNearestGoal(t))
                {
                    float tempDist = Vector3.Distance(position, goals[i].Position());
                    if (tempDist < dist)
                    {
                        dist = tempDist;
                        g = goals[i];
                    }
                }
            }
        }


        return g;
    }

    //private void LateUpdate()
    //{
    //    ammoGens.Clear();
    //    healers.Clear();
    //}

    public bool GetNearestHealth(Vector3 position, out IHealer r)
    {
        float dist = Mathf.Infinity;
        r = null;
        for (int i = 0; i < healers.Count; i++)
        {
            if (healers[i] == null)
            {
                healers.RemoveAt(i);
                break;
            }
            float tempDist = Vector3.Distance(position, healers[i].Position());
            if (tempDist < dist)
            {
                dist = tempDist;
                r = healers[i];
            }
        }
        return r != null;
    }
    public bool GetNearestAmmoGen(Vector3 position, out IAmmoGen r)
    {
        float dist = Mathf.Infinity;
        r = null;
        for (int i = 0; i < ammoGens.Count; i++)
        {
            if(ammoGens[i] == null)
            {
                ammoGens.RemoveAt(i);
                break;
            }
            float tempDist = Vector3.Distance(position, ammoGens[i].Position());
            if (tempDist < dist)
            {
                dist = tempDist;
                r = ammoGens[i];
            }
        }
        return r != null;
    }

    public bool GetNearestWeapon(Vector3 position, out IPickable r)
    {
        float dist = Mathf.Infinity;
        r = null;
        int i = 0;
        while (i < collectWeapons.Count)
        {
            if (collectWeapons[i] == null)
            {
                collectWeapons.RemoveAt(i);
                continue;
            }

            float tempDist = Vector3.Distance(position, collectWeapons[i].Position());
            if (tempDist < dist)
            {
                dist = tempDist;
                r = collectWeapons[i];
            }

            i++;
        }
        return r != null;
    }

    public void AddPickable(IPickable r)
    {
        if (!collectWeapons.Contains(r))
            collectWeapons.Add(r);
    }

    public void RemovePickable(IPickable r)
    {
        collectWeapons.Remove(r);
    }

    public void AddAmmoGen(IAmmoGen ammoGen)
    {
        ammoGens.Add(ammoGen);
    }

    public void AddHealer(IHealer healer)
    {
        healers.Add(healer);
    }

    public void RemoveAmmoGen(IAmmoGen ammoGen)
    {
        ammoGens.Remove(ammoGen);
    }

    public void RemoveHealer(IHealer healer)
    {
        healers.Remove(healer);
    }

    public int GetCommandPostTeamCount(int team)
    {
        int count = 0;
        for (int i = 0; i < spawners.Length; i++)
        {
            if(spawners[i].CanSpawn(team))
            {
                count++;
            }
        }
        return count;
    }

    public static ResourceManagerModular instance;
}
