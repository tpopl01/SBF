using System.Collections;
using System.Collections.Generic;
using tpopl001.Weapons;
using UnityEngine;

public class WeaponSystemVehicle : WeaponSystemBase
{
    public Weapon[] weaps;


    public override Weapon[] GetWeapons()
    {
        return weaps;
    }

    public override bool Attack(Vector3 target, ModularController self, ModularController targetAgent)
    {
        bool retval = true;
        for (int i = 0; i < weaps.Length; i++)
        {
            if (!base.Attack(target, self, targetAgent))
                retval = false;
        }
        return retval;
    }

    public override Weapon[] GetCurrentWeapons()
    {
        return weaps;
    }

    public override void SetUp(Transform root)
    {
        weaps = GetComponentsInChildren<Weapon>();
        //for (int i = 0; i < weaps.Length; i++)
        //{
        //    weaps[i].Initialise();
        //}
    }
}
