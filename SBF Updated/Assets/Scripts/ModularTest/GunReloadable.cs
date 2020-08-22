using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using tpopl001.Weapons;
using UnityEngine;

public class GunReloadable : Gun1
{
    GunStats gunStats;
    float currentAmmo;
    protected Timer reloadTimer;
    protected int ammoInClip;
    protected int totalAmmo;
    protected bool reloading;
    protected const float reloadTime = 3.3f;

    protected override bool Attack()
    {
        if(reloadTimer.GetComplete() == false || (ammoInClip < 1))
        {
            Reload();
            return false;
        }

        return base.Attack();
    }

    protected override bool Reload1()
    {
        return base.Reload1();
    }

}
