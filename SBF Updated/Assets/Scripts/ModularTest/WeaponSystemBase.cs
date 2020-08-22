using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tpopl001.Weapons;
using UnityEngine;

public abstract class WeaponSystemBase : MonoBehaviour, ISetup, IWeaponSystem
{
    public virtual bool Attack(Vector3 target, ModularController self, ModularController targetAgent)
    {
        bool attacking = false;
        Weapon[] currentWeapons = GetCurrentWeapons();
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Attack(target, 0, self, targetAgent))
            {
                attacking = true;
            }
        }
        return attacking;
    }

    public abstract Weapon[] GetCurrentWeapons();

    public abstract Weapon[] GetWeapons();

    public float hitChance(float distance)
    {
        Weapon[] w = GetCurrentWeapons();
        if (w.Length > 0)
            return w.Max(x => x.HitChance(distance));
        return 0;
    }

    public float Range(float distance)
    {
        Weapon[] w = GetCurrentWeapons();
        if (w.Length > 0)
            return w.Max(x => x.GetRange(distance));
        return 0;
    }

    public float GetActualRange()
    {
        Weapon[] w = GetCurrentWeapons();
        if (w != null)
            if (w.Length > 0)
                return w.Max(x => x.GetActualRange());
        return 0;
    }

    public float GetDamageAtDistance(float distance)
    {
        Weapon[] w = GetCurrentWeapons();
        if (w.Length > 0)
            return w.Max(x => x.GetDamageAtDistance(distance));
        return 0;
    }

    public abstract void SetUp(Transform root);
}
