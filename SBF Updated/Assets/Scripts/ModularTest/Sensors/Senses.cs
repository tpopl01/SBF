using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tpopl001.Utils;
using UnityEngine;

//Attach to head
public class Senses : SensesBase, ISetup
{
    IHealer nearestHealth;
    IAmmoGen nearestAmmo;
    IRepair nearestRepairable;
    public IPickable collectWeapon;
    IGoal hostileGoal;
    IGoal goal;
    IGoal allyGoal;
    IEnter vehicle;
    Animator anim;
    

    #region Goals
    public Vector3 GetHostileGoal()
    {
        if (hostileGoal != null)
            return hostileGoal.Position();

        return Vector3.zero;
    }
    public Vector3 GetAllyGoal()
    {
        if (allyGoal != null)
            return allyGoal.Position();

        return Vector3.zero;
    }
    public Vector3 GetGoal()
    {
        if (goal != null)
            return goal.Position();

        return Vector3.zero;
    }

    private void SetGoal()
    {
        goal = ResourceManagerModular.instance.GetNearestGoal(transform.position, -1);
      //  if (goal != null)
       //     goal.Capture(self.Team, self.Position);
    }

    private void SetAllyGoal()
    {
        allyGoal = ResourceManagerModular.instance.GetNearestGoal(transform.position, self.Team);
     //   if (allyGoal != null)
        //    allyGoal.Capture(self.Team, self.Position);
    }

    private void SetHostileGoal()
    {
        hostileGoal = ResourceManagerModular.instance.GetNearestGoalNot(transform.position, self.Team);
     //   if(hostileGoal != null)
   //         hostileGoal.Capture(self.Team, self.Position);
    }
    #endregion

    public IEnter NearestVehicle()
    {
        if(vehicle == null)
        {
            vehicle = ResourceManagerModular.instance.GetNearestVehicle(transform.position);
        }
        return vehicle;
    }

    public IPickable NearestWeapon()
    {
        if (collectWeapon == null)
        {
            ResourceManagerModular.instance.GetNearestWeapon(transform.position, out collectWeapon);
        }
        return collectWeapon;
    }

    public IHealer NearestHealth()
    {
        return nearestHealth;
    }

    public IAmmoGen NearestAmmo()
    {
        return nearestAmmo;
    }

    public IRepair NearestRepair(int team)
    {
        if (nearestHealth == null)
        {
            ResourceManagerModular.instance.GetNearestRepair(team, transform.position, out nearestRepairable);
        }
        return nearestRepairable;
    }



    public override void Tick()
    {
        base.Tick();
        Transform hitBone = anim.GetBoneTransform(HumanBodyBones.Chest);
        IdealHitPos = hitBone.position;
        if (goal != null)
        {
            goal.Capture(self.Team, self.Position);
        }
        else if(hostileGoal != null)
        {
            hostileGoal.Capture(self.Team, self.Position);
        }
        else if (allyGoal != null)
        {
            allyGoal.Capture(self.Team, self.Position);
        }

        if(collectWeapon != null)
        {
            collectWeapon.Pickup((IWeaponSystomChangeable)self.weaponSystem, self.Position);
        }

        if (nearestAmmo != null)
        {
            nearestAmmo.AddAmmo((IAmmoAdd)self.weaponSystem);
        }
        if (nearestHealth != null)
        {
            nearestHealth.Heal((IHealable)self.Health);
        }
    }


    protected override void ScatteredTick()
    {
        base.ScatteredTick();
        SetGoal();
        SetHostileGoal();
        SetAllyGoal();
        // nearestAmmo = null;
        // nearestHealth = null;
        nearestAmmo = null;
        nearestHealth = null;
        collectWeapon = null;
        nearestRepairable = null;

        if (self.Health.GetHPPercent() != 1)
        {
            ResourceManagerModular.instance.GetNearestHealth(transform.position, out nearestHealth);
        }
       // if(((IWeaponSystomChangeable)self.weaponSystem).GetAmmoRemainingInGunPercent() != 1)
            ResourceManagerModular.instance.GetNearestAmmoGen(transform.position, out nearestAmmo);
        vehicle = null;
        
    }

    public Vector3 GetNearestHealth()
    {
        if (NearestHealth() != null)
        {
            return NearestHealth().Position();
        }
        return Vector3.zero;
    }
    public Vector3 GetNearestAmmo()
    {
        if (NearestAmmo() != null)
        {
            return NearestAmmo().Position();
        }
        return Vector3.zero;
    }
    public Vector3 GetNearestRepair(int team)
    {
        if (NearestRepair(team) != null)
        {
            return NearestRepair(team).Position();
        }
        return Vector3.zero;
    }
    public Vector3 GetNearestWeapon()
    {
        if (NearestWeapon() != null)
        {
            return NearestWeapon().Position();
        }
        return Vector3.zero;
    }

    public void SetUp(Transform root)
    {
        anim = root.GetComponentInChildren<Animator>();
    }
}
